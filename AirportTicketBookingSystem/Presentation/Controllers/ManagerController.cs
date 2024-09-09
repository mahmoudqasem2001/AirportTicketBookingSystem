using AirportTicketBookingSystem.Application.UseCases;
using AirportTicketBookingSystem.Domain;
using AirportTicketBookingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.Presentation.Controllers
{
    public class ManagerController
    {
        private readonly ManagerUseCase managerUseCase;
        private readonly IManagerRepository managerRepository;

        public ManagerController(ManagerUseCase managerUseCase, IManagerRepository managerRepo)
        {
            this.managerUseCase = managerUseCase;
            managerRepository = managerRepo;
        }

        public void FilterBookings(string flightId, decimal? price, string departureCountry, string destinationCountry, DateTime? departureDate, string departureAirport, string arrivalAirport, string passengerId, FlightClass? flightClass)
        {
            IEnumerable<Booking> bookings = managerRepository.FilterBookings(flightId, price, departureCountry, destinationCountry, departureDate, departureAirport, arrivalAirport, passengerId, flightClass);
            foreach (Booking booking in bookings)
            {
                Console.WriteLine($"Booking {booking.Id}: Flight {booking.Flight.FlightId}, Passenger: {booking.Passenger.Name}, Price: {booking.Price}");
            }
        }

        public void ImportFlights(string csvFilePath)
        {
            try
            {
                managerRepository.ImportFlightsFromCsv(csvFilePath);
                Console.WriteLine("Flights imported successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error importing flights: {ex.Message}");
            }
        }

        public void ShowValidationDetails()
        {
            Dictionary<string, List<string>> validationDetails = managerUseCase.GetValidationDetails();
            foreach (var field in validationDetails)
            {
                Console.WriteLine($"{field.Key}:");
                foreach (string detail in field.Value)
                {
                    Console.WriteLine($"  - {detail}");
                }
            }
        }
    }
}
