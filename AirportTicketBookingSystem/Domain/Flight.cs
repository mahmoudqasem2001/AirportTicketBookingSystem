using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.Domain
{
   public class Flight
    {
        public string FlightId { get; set; }
        public string DepartureCountry { get; set; }
        public string DestinationCountry { get; set; }
        public DateTime DepartureDate { get; set; }
        public string DepartureAirport { get; set; }
        public string ArrivalAirport { get; set; }
        public decimal Price { get; set; }
    }
}
