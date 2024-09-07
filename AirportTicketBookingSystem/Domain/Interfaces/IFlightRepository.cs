using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.Domain.Interfaces
{
    public interface IFlightRepository
    {
        IEnumerable<Flight> GetAllFlights();           
        Flight GetFlightById(string flightId);        
        void AddFlight(Flight flight);                 
        void RemoveFlight(string flightId);           
    }
}
