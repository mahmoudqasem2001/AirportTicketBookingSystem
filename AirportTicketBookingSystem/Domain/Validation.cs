using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.Domain
{
    public static class Validation
    {
        public static bool ValidateFlight(Flight flight, out List<string> errors)
        {
            errors = new List<string>();
            if (string.IsNullOrEmpty(flight.DepartureCountry))
                errors.Add("Departure country is required.");
            if (string.IsNullOrEmpty(flight.DestinationCountry))
                errors.Add("Destination country is required.");
            if (flight.DepartureDate < DateTime.Now)
                errors.Add("Departure date must be in the future.");
            return !errors.Any();
        }
    }

}
