using AirportTicketBookingSystem.Domain;
using AirportTicketBookingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.Application.UseCases
{
    public class ManagerUseCase
    {
        private readonly IBookingRepository bookingRepository;
        private readonly IFlightRepository flightRepository;

        public ManagerUseCase(IBookingRepository bookingRepo)
        {
            bookingRepository = bookingRepo;
        }

        public IEnumerable<Booking> FilterBookings(
            string flightId = null,
            decimal? price = null,
            string departureCountry = null,
            string destinationCountry = null,
            DateTime? departureDate = null,
            string departureAirport = null,
            string arrivalAirport = null,
            string passengerId = null,
            FlightClass? flightClass = null)
        {
            IEnumerable<Booking> allBookings = bookingRepository.GetAllBookings();

            return allBookings.Where(b =>
                (string.IsNullOrEmpty(flightId) || b.Flight.FlightId == flightId) &&
                (!price.HasValue || b.Price == price) &&
                (string.IsNullOrEmpty(departureCountry) || b.Flight.DepartureCountry == departureCountry) &&
                (string.IsNullOrEmpty(destinationCountry) || b.Flight.DestinationCountry == destinationCountry) &&
                (!departureDate.HasValue || b.Flight.DepartureDate == departureDate) &&
                (string.IsNullOrEmpty(departureAirport) || b.Flight.DepartureAirport == departureAirport) &&
                (string.IsNullOrEmpty(arrivalAirport) || b.Flight.ArrivalAirport == arrivalAirport) &&
                (string.IsNullOrEmpty(passengerId) || b.Passenger.Id == passengerId) &&
                (!flightClass.HasValue || b.Class == flightClass));
        }

        public void ImportAndValidateFlights(string csvFilePath)
        {
            var flightData = File.ReadAllLines(csvFilePath)
                .Skip(1) 
                .Select(line => line.Split(','))
                .Select(columns => new Flight
                {
                    FlightId = columns[0],
                    DepartureCountry = columns[1],
                    DestinationCountry = columns[2],
                    DepartureDate = DateTime.Parse(columns[3]),
                    Price = decimal.Parse(columns[4])
                });

            foreach (Flight flight in flightData)
            {
                if (Validation.ValidateFlight(flight, out List<string> errors))
                {
                    flightRepository.AddFlight(flight);
                }
                else
                {
                    Console.WriteLine($"Errors for flight {flight.FlightId}: {string.Join(", ", errors)}");
                }
            }
        }

        public Dictionary<string, List<string>> GetValidationDetails()
        {
            var validationDetails = new Dictionary<string, List<string>>
    {
        {
            "Departure Country", new List<string>
            {
                "Type: Free Text",
                "Constraint: Required"
            }
        },
        {
            "Destination Country", new List<string>
            {
                "Type: Free Text",
                "Constraint: Required"
            }
        },
        {
            "Departure Date", new List<string>
            {
                "Type: Date Time",
                "Constraint: Required, Allowed Range (today → future)"
            }
        },
        {
            "Price", new List<string>
            {
                "Type: Decimal",
                "Constraint: Required, Positive Number"
            }
        }
    };

            return validationDetails;
        }
    }

}
