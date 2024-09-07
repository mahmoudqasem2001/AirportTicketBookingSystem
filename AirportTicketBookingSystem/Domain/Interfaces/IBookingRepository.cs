using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.Domain.Interfaces
{
    public interface IBookingRepository
    {
        IEnumerable<Booking> GetAllBookings();         
        Booking GetBookingById(string bookingId);     
        IEnumerable<Booking> GetBookingsByPassengerId(string passengerId);  
        void AddBooking(Booking booking);            
        void RemoveBooking(string bookingId);        
        void UpdateBooking(Booking booking);          
    }
}
