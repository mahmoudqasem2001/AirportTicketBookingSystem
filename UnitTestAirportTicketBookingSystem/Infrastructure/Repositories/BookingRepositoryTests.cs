using AirportTicketBookingSystem.Domain;
using AirportTicketBookingSystem.Domain.Interfaces;
using AirportTicketBookingSystem.Infrastructure.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTestAirportTicketBookingSystem.Infrastructure.Repositories
{
    public class BookingRepositoryTests
    {
        private readonly Mock<IPassengerRepository> _passengerRepoMock;
        private readonly Mock<IFlightRepository> _flightRepoMock;
        private readonly BookingRepository _bookingRepository;

        public BookingRepositoryTests()
        {
            _passengerRepoMock = new Mock<IPassengerRepository>();
            _flightRepoMock = new Mock<IFlightRepository>();
            _bookingRepository = new BookingRepository(_passengerRepoMock.Object, _flightRepoMock.Object);
        }

        [Fact]
        public void GetAllBookings_ShouldReturnEmpty_WhenNoFileExists()
        {
            // Arrange
            if (File.Exists("bookings.csv"))
                File.Delete("bookings.csv");

            // Act
            var result = _bookingRepository.GetAllBookings();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void AddBooking_ShouldAddBookingToFile()
        {
            // Arrange
            var booking = new Booking
            {
                Id = "B123",
                Passenger = new Passenger { Id = "P1", Name = "John" },
                Flight = new Flight { FlightId = "F1" },
                Class = FlightClass.Economy,
                Price = 200m
            };

            _passengerRepoMock.Setup(pr => pr.GetPassengerById("P1")).Returns(booking.Passenger);
            _flightRepoMock.Setup(fr => fr.GetFlightById("F1")).Returns(booking.Flight);

            // Act
            _bookingRepository.AddBooking(booking);

            // Assert
            var savedBooking = _bookingRepository.GetBookingById("B123");
            Assert.NotNull(savedBooking);
            Assert.Equal("B123", savedBooking.Id);
            Assert.Equal("P1", savedBooking.Passenger.Id);
        }

        [Fact]
        public void RemoveBooking_ShouldRemoveBooking_WhenExists()
        {
            // Arrange
            var booking = new Booking
            {
                Id = "B123",
                Passenger = new Passenger { Id = "P1", Name = "John" },
                Flight = new Flight { FlightId = "F1" },
                Class = FlightClass.Economy,
                Price = 200m
            };

            _bookingRepository.AddBooking(booking);

            // Act
            _bookingRepository.RemoveBooking("B123");

            // Assert
            var removedBooking = _bookingRepository.GetBookingById("B123");
            Assert.Null(removedBooking);
        }
    }

}
