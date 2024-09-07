using AirportTicketBookingSystem.Domain;
using AirportTicketBookingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.Application.UseCases
{
    public class ManageBookingUseCase
    {
        private readonly IBookingRepository bookingRepository;

        public ManageBookingUseCase(IBookingRepository bookingRepo)
        {
            bookingRepository = bookingRepo;
        }

        public IEnumerable<Booking> GetPassengerBookings(Passenger passenger)
        {
            return bookingRepository.GetBookingsByPassengerId(passenger.Id);
        }

        public void CancelBooking(string bookingId)
        {
            bookingRepository.RemoveBooking(bookingId);
        }

        public void ModifyBooking(string bookingId, FlightClass newClass)
        {
            Booking booking = bookingRepository.GetBookingById(bookingId);
            booking.Class = newClass;
            booking.Price = CalculateNewPrice(booking.Flight, newClass);
        }

        private decimal CalculateNewPrice(Flight flight, FlightClass flightClass)
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

}
