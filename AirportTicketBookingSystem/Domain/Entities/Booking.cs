using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.Domain
{
    public class Booking
    {
        public string Id { get; set; }
        public Passenger Passenger { get; set; }
        public Flight Flight { get; set; }
        public FlightClass Class { get; set; }
        public decimal Price { get; set; }
    }
}
