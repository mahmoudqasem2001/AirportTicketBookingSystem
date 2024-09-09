using AirportTicketBookingSystem.Domain;
using AirportTicketBookingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.Infrastructure.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private const string FlightsFilePath = "flights.csv";

        public IEnumerable<Flight> GetAllFlights()
        {
            if (!File.Exists(FlightsFilePath)) return Enumerable.Empty<Flight>();

            return File.ReadAllLines(FlightsFilePath)
                .Skip(1)
                .Select(line => line.Split(','))
                .Select(columns => new Flight
                {
                    FlightId = columns[0],
                    DepartureCountry = columns[1],
                    DestinationCountry = columns[2],
                    DepartureDate = DateTime.Parse(columns[3]),
                    Price = decimal.Parse(columns[4])
                });
        }

        public void AddFlight(Flight flight)
        {
            if (!File.Exists(FlightsFilePath))
            {
                throw new FileNotFoundException("Flight data file not found.");
            }

            string line = $"{flight.FlightId},{flight.DepartureCountry},{flight.DestinationCountry},{flight.DepartureDate},{flight.Price}";
            File.AppendAllLines(FlightsFilePath, new[] { line });
        }

        public Flight GetFlightById(string flightId)
        {
            return GetAllFlights().FirstOrDefault(f => f.FlightId == flightId);
        }

        public void RemoveFlight(string flightId)
        {
            if (!File.Exists(FlightsFilePath))
            {
                throw new FileNotFoundException("Flight data file not found.");
            }

            List<string> flightLines = File.ReadAllLines(FlightsFilePath).ToList();

            var header = flightLines.First(); 
            var flights = flightLines.Skip(1) 
                                      .Where(line => !line.StartsWith(flightId + ","))
                                      .ToList();

            if (flights.Count == flightLines.Count - 1)
            {
                throw new ArgumentException($"Flight with ID '{flightId}' not found.");
            }

            flights.Insert(0, header); 
            File.WriteAllLines(FlightsFilePath, flights);
        }
    }

}
