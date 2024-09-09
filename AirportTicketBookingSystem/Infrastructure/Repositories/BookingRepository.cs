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
    public class BookingRepository : IBookingRepository
    {
        private const string BookingsFilePath = "bookings.csv";
        private readonly IPassengerRepository passengerRepository;
        private readonly IFlightRepository flightRepository;

        public BookingRepository(IPassengerRepository passengerRepo, IFlightRepository flightRepo)
        {
            passengerRepository = passengerRepo;
            flightRepository = flightRepo;
        }

        public BookingRepository()
        {
        }

        public IEnumerable<Booking> GetAllBookings()
        {
            if (!File.Exists(BookingsFilePath)) return Enumerable.Empty<Booking>();

            return File.ReadAllLines(BookingsFilePath)
                .Skip(1)
                .Select(line => line.Split(','))
                .Select(columns => new Booking
                {
                    Id = columns[0],
                    Passenger = passengerRepository.GetPassengerById(columns[1]),
                    Flight = flightRepository.GetFlightById(columns[2]),
                    Class =(FlightClass) Enum.Parse(typeof(FlightClass) , columns[3], true),
                    Price = decimal.Parse(columns[4])
                }
                );
        }

        public void AddBooking(Booking booking)
        {
            string line = $"{booking.Id},{booking.Passenger.Name},{booking.Flight.FlightId},{booking.Class},{booking.Price}";
            File.AppendAllLines(BookingsFilePath, new[] { line });
        }

        public Booking GetBookingById(string bookingId)
        {
            return GetAllBookings().FirstOrDefault(b => b.Id == bookingId);
        }

        public IEnumerable<Booking> GetBookingsByPassengerId(string passengerId)
        {
            return GetAllBookings().Where(b => b.Passenger.Id == passengerId);
        }

        public void RemoveBooking(string bookingId)
        {
            List<Booking> allBookings = GetAllBookings().ToList();
            Booking bookingToRemove = allBookings.FirstOrDefault(b => b.Id == bookingId);
            if (bookingToRemove != null)
            {
                allBookings.Remove(bookingToRemove);
                using (StreamWriter writer = new StreamWriter(BookingsFilePath))
                {
                    writer.WriteLine("BookingId,PassengerId,FlightId,Class,Price");

                    foreach (Booking booking in allBookings)
                    {
                        string line = $"{booking.Id},{booking.Passenger.Id},{booking.Flight.FlightId},{booking.Class},{booking.Price}";
                        writer.WriteLine(line);
                    }
                }
            }
            else
            {
                throw new ArgumentException($"Booking with ID '{bookingId}' not found.");
            }
        
        }

        public void UpdateBooking(Booking updatedBooking)
        {
            List<Booking> allBookings = GetAllBookings().ToList();

            int index = allBookings.FindIndex(b => b.Id == updatedBooking.Id);

            if (index != -1)
            {
                allBookings[index] = updatedBooking;

                using (StreamWriter writer = new StreamWriter(BookingsFilePath))
                {
                    writer.WriteLine("BookingId,PassengerId,FlightId,Class,Price");

                    foreach (Booking booking in allBookings)
                    {
                        string line = $"{booking.Id},{booking.Passenger.Id},{booking.Flight.FlightId},{booking.Class},{booking.Price}";
                        writer.WriteLine(line);
                    }
                }
            }
            else
            {
                throw new ArgumentException($"Booking with ID '{updatedBooking.Id}' not found.");
            }
        }
    }

}
