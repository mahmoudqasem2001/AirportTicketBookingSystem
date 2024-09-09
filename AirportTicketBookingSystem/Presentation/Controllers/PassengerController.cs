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
    public class PassengerController
    {
        private readonly BookFlightUseCase bookFlightUseCase;
        private readonly SearchFlightsUseCase searchFlightsUseCase;
        private readonly ManageBookingUseCase manageBookingUseCase;
        private readonly IPassengerRepository passengerRepository;

        public PassengerController(
            BookFlightUseCase bookFlight,
            SearchFlightsUseCase searchFlights,
            ManageBookingUseCase manageBooking,
            IPassengerRepository passengerRepo)
        {
            bookFlightUseCase = bookFlight;
            searchFlightsUseCase = searchFlights;
            manageBookingUseCase = manageBooking;
            passengerRepository = passengerRepo;
        }

        public void BookFlight(string passengerId, string flightId, FlightClass flightClass)
        {
            Passenger passenger = passengerRepository.GetPassengerById(passengerId);
            if (passenger == null)
            {
                Console.WriteLine("Passenger not found.");
                return;
            }

            Booking booking = bookFlightUseCase.BookFlight(passenger, flightId, flightClass);
            Console.WriteLine($"Booking confirmed: {booking.Id}, Price: {booking.Price}");
        }

        public void SearchFlights(decimal? price = null, string departureCountry = null, string destinationCountry = null, DateTime? departureDate = null, string departureAirport = null, string arrivalAirport = null, FlightClass? flightClass = null)
        {
            IEnumerable<Flight> flights = searchFlightsUseCase.SearchFlights(price, departureCountry, destinationCountry, departureDate, departureAirport, arrivalAirport, flightClass);

            if (!flights.Any())
            {
                Console.WriteLine("No flights found matching the search criteria.");
                return;
            }

            Console.WriteLine("Available Flights:");
            foreach (Flight flight in flights)
            {
                Console.WriteLine($"{flight.FlightId}: From {flight.DepartureCountry} to {flight.DestinationCountry}, Departure: {flight.DepartureDate}, Price: {flight.Price}, Class: {flight.Class}");
            }
        }

        public void ViewBookings(string passengerId)
        {
            Passenger passenger = passengerRepository.GetPassengerById(passengerId);
            if (passenger == null)
            {
                Console.WriteLine("Passenger not found.");
                return;
            }

            IEnumerable<Booking> bookings = manageBookingUseCase.GetPassengerBookings(passenger);
            foreach (Booking booking in bookings)
            {
                Console.WriteLine($"Booking {booking.Id}: Flight {booking.Flight.FlightId}, Class: {booking.Class}, Price: {booking.Price}");
            }
        }

        public void CancelBooking(string bookingId)
        {
            manageBookingUseCase.CancelBooking(bookingId);
            Console.WriteLine($"Booking {bookingId} has been canceled.");
        }

        public void ModifyBooking(string bookingId, FlightClass newClass)
        {
            manageBookingUseCase.ModifyBooking(bookingId, newClass);
            Console.WriteLine($"Booking {bookingId} has been updated to class {newClass}.");
        }
    }
}
