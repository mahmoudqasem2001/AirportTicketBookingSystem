using AirportTicketBookingSystem.Domain;
using AirportTicketBookingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.Application.UseCases
{
    public class BookFlightUseCase
    {
        private readonly IFlightRepository flightRepository;
        private readonly IBookingRepository bookingRepository;

        public BookFlightUseCase(IFlightRepository flightRepo, IBookingRepository bookingRepo)
        {
            flightRepository = flightRepo;
            bookingRepository = bookingRepo;
        }

        public Booking BookFlight(Passenger passenger, string flightId, FlightClass flightClass)
        {
            Flight flight = flightRepository.GetFlightById(flightId);
            if (flight == null) throw new ArgumentException("Flight not found");

            Booking booking = new Booking
            {
                Id = Guid.NewGuid().ToString(),
                Passenger = passenger,
                Flight = flight,
                Class = flightClass,
                Price = CalculatePrice(flight, flightClass)
            };

            bookingRepository.AddBooking(booking);
            return booking;
        }

        private decimal CalculatePrice(Flight flight, FlightClass flightClass)
        {

            decimal multiplier;

            switch (flightClass)
            {
                case FlightClass.Business:
                    multiplier = 1.5m;
                    break;
                case FlightClass.FirstClass:
                    multiplier = 2.0m;
                    break;
                default:
                    multiplier = 1.0m;
                    break;
            }

            return flight.Price * multiplier;
        }
    }

}
