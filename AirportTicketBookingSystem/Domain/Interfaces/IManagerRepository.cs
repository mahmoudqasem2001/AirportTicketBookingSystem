using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.Domain.Interfaces
{
    public interface IManagerRepository
    {
        void ImportFlightsFromCsv(string csvFilePath);  

        IEnumerable<Booking> FilterBookings(          
            string flightId = null,
            decimal? price = null,
            string departureCountry = null,
            string destinationCountry = null,
            DateTime? departureDate = null,
            string departureAirport = null,
            string arrivalAirport = null,
            string passengerId = null,
            FlightClass? flightClass = null);
    }
}
