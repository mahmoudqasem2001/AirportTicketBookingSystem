using AirportTicketBookingSystem.Domain;
using AirportTicketBookingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.Infrastructure.Repositories
{
    public class ManagerRepository : IManagerRepository
    {
        private readonly IFlightRepository flightRepository;
        private readonly IBookingRepository bookingRepository;

        public ManagerRepository(IFlightRepository flightRepo, IBookingRepository bookingRepo)
        {
            flightRepository = flightRepo;
            bookingRepository = bookingRepo;
        }

        public void ImportFlightsFromCsv(string csvFilePath)
        {
            if (!File.Exists(csvFilePath))
            {
                throw new FileNotFoundException("CSV file not found.");
            }

            var flightData = File.ReadAllLines(csvFilePath)
                .Skip(1) 
                .Select(line => line.Split(','))
                .Select(columns => new Flight
                {
                    FlightId = columns[0],
                    DepartureCountry = columns[1],
                    DestinationCountry = columns[2],
                    DepartureDate = DateTime.Parse(columns[3]),
                    DepartureAirport = columns[4],
                    ArrivalAirport = columns[5],
                    Price = decimal.Parse(columns[6]),
                    Class = (FlightClass)Enum.Parse(typeof(FlightClass), columns[7], true)
                });

            foreach (var flight in flightData)
            {
                flightRepository.AddFlight(flight);
            }

            Console.WriteLine("Flights imported successfully from CSV.");
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
            var allBookings = bookingRepository.GetAllBookings();

            return allBookings.Where(b =>
                (string.IsNullOrEmpty(flightId) || b.Flight.FlightId == flightId) &&
                (!price.HasValue || b.Price == price.Value) &&
                (string.IsNullOrEmpty(departureCountry) || b.Flight.DepartureCountry.Equals(departureCountry, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(destinationCountry) || b.Flight.DestinationCountry.Equals(destinationCountry, StringComparison.OrdinalIgnoreCase)) &&
                (!departureDate.HasValue || b.Flight.DepartureDate.Date == departureDate.Value.Date) &&
                (string.IsNullOrEmpty(departureAirport) || b.Flight.DepartureAirport.Equals(departureAirport, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(arrivalAirport) || b.Flight.ArrivalAirport.Equals(arrivalAirport, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(passengerId) || b.Passenger.Id == passengerId) &&
                (!flightClass.HasValue || flightClass == b.Class));
        }
    }
}
