using AirportTicketBookingSystem.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTestAirportTicketBookingSystem.Domain.Entites
{
    public class ValidationTests
    {
        [Theory]
        [InlineData("USA", "UK", 5, true, null)]
        [InlineData("", "UK", 5, false, "Departure country is required.")]
        [InlineData("USA", "", 5, false, "Destination country is required.")]
        [InlineData("USA", "UK", -1, false, "Departure date must be in the future.")]
        public void ValidateFlight_ShouldReturnExpectedResult(
            string departureCountry,
            string destinationCountry,
            int daysUntilDeparture,
            bool expectedValidity,
            string expectedError)
        {
            var flight = new Flight
            {
                FlightId = "FL123",
                DepartureCountry = departureCountry,
                DestinationCountry = destinationCountry,
                DepartureDate = DateTime.Now.AddDays(daysUntilDeparture),
                DepartureAirport = "JFK",
                ArrivalAirport = "LHR",
                Price = 500m,
                Class = FlightClass.Economy
            };

            var isValid = Validation.ValidateFlight(flight, out var errors);

            Assert.Equal(expectedValidity, isValid);

            if (!expectedValidity)
            {
                Assert.Contains(expectedError, errors);
            }
            else
            {
                Assert.Empty(errors);
            }
        }



    }

}
