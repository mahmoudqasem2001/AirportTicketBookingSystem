using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.Domain.Interfaces
{
    public interface IPassengerRepository
    {
        Passenger GetPassengerById(string passengerId); 
        IEnumerable<Passenger> GetAllPassengers();     
        void AddPassenger(Passenger passenger);        
        void RemovePassenger(string passengerId);       
        void UpdatePassenger(Passenger passenger);     
    }
}
