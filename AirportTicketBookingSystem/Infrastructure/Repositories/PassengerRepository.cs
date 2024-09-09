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
    public class PassengerRepository : IPassengerRepository
    {
        private const string PassengersFilePath = "passengers.csv";

        public IEnumerable<Passenger> GetAllPassengers()
        {
            if (!File.Exists(PassengersFilePath)) return Enumerable.Empty<Passenger>();

            return File.ReadAllLines(PassengersFilePath)
                .Skip(1)
                .Select(line => line.Split(','))
                .Select(columns => new Passenger
                {
                    Id = columns[0],
                    Name = columns[1],
                    Email = columns[2]
                });
        }

        public Passenger GetPassengerById(string passengerId)
        {
            return GetAllPassengers().FirstOrDefault(p => p.Id == passengerId);
        }

        public void AddPassenger(Passenger passenger)
        {
            using (StreamWriter writer = new StreamWriter(PassengersFilePath, true))
            {
                if (new FileInfo(PassengersFilePath).Length == 0)
                {
                    writer.WriteLine("PassengerId,Name,Email");
                }

                writer.WriteLine($"{passenger.Id},{passenger.Name},{passenger.Email}");
            }
        }

        public void RemovePassenger(string passengerId)
        {
            List<Passenger> allPassengers = GetAllPassengers().ToList();
            Passenger passengerToRemove = allPassengers.FirstOrDefault(p => p.Id == passengerId);

            if (passengerToRemove != null)
            {
                allPassengers.Remove(passengerToRemove);

                using (StreamWriter writer = new StreamWriter(PassengersFilePath))
                {
                    writer.WriteLine("PassengerId,Name,Email");

                    foreach (var passenger in allPassengers)
                    {
                        writer.WriteLine($"{passenger.Id},{passenger.Name},{passenger.Email}");
                    }
                }
            }
            else
            {
                throw new ArgumentException($"Passenger with ID '{passengerId}' not found.");
            }
        }

        public void UpdatePassenger(Passenger updatedPassenger)
        {
            List<Passenger> allPassengers = GetAllPassengers().ToList();
            int index = allPassengers.FindIndex(p => p.Id == updatedPassenger.Id);

            if (index != -1)
            {
                allPassengers[index] = updatedPassenger;

                using (StreamWriter writer = new StreamWriter(PassengersFilePath))
                {
                    writer.WriteLine("PassengerId,Name,Email");

                    foreach (var passenger in allPassengers)
                    {
                        writer.WriteLine($"{passenger.Id},{passenger.Name},{passenger.Email}");
                    }
                }
            }
            else
            {
                throw new ArgumentException($"Passenger with ID '{updatedPassenger.Id}' not found.");
            }
        }
    }
}
