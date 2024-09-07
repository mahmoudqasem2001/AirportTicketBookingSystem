using AirportTicketBookingSystem.Domain;
using AirportTicketBookingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.Application.UseCases
{
    public class SearchFlightsUseCase
    {
        private readonly IFlightRepository flightRepository;

        public SearchFlightsUseCase(IFlightRepository flightRepo)
        {
            flightRepository = flightRepo;
        }

        public IEnumerable<Flight> SearchFlights(
            decimal? price = null,
            string departureCountry = null,
            string destinationCountry = null,
            DateTime? departureDate = null,
            string departureAirport = null,
            string arrivalAirport = null,
            FlightClass? flightClass = null)
        {
            IEnumerable<Flight> allFlights = flightRepository.GetAllFlights();

            return allFlights.Where(f =>
                (!price.HasValue || f.Price <= price.Value) &&
                (string.IsNullOrEmpty(departureCountry) || f.DepartureCountry.Equals(departureCountry, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(destinationCountry) || f.DestinationCountry.Equals(destinationCountry, StringComparison.OrdinalIgnoreCase)) &&
                (!departureDate.HasValue || f.DepartureDate.Date == departureDate.Value.Date) &&
                (string.IsNullOrEmpty(departureAirport) || f.DepartureAirport.Equals(departureAirport, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(arrivalAirport) || f.ArrivalAirport.Equals(arrivalAirport, StringComparison.OrdinalIgnoreCase)) &&
                (!flightClass.HasValue || flightClass.Value == f.Class)
            );
        }
    }

}
