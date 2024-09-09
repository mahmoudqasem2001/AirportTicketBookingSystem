using System;
using System.Collections.Generic;
using System.Linq;
using AirportTicketBookingSystem.Domain;
using AirportTicketBookingSystem.Infrastructure;
using AirportTicketBookingSystem.Presentation.Controllers;
using AirportTicketBookingSystem.Application.UseCases;
using AirportTicketBookingSystem.Domain.Interfaces;
using AirportTicketBookingSystem.Infrastructure.Repositories;

class Program
{
    static void Main(string[] args)
    {
        // Initialize repositories
        IFlightRepository flightRepository = new FlightRepository();
        IBookingRepository bookingRepository = new BookingRepository();
        IPassengerRepository passengerRepository = new PassengerRepository();
        IManagerRepository managerRepository = new ManagerRepository(flightRepository, bookingRepository);

        // Initialize use cases
        var bookFlightUseCase = new BookFlightUseCase(flightRepository, bookingRepository);
        var searchFlightsUseCase = new SearchFlightsUseCase(flightRepository);
        var manageBookingUseCase = new ManageBookingUseCase(bookingRepository);
        var managerUseCase = new ManagerUseCase(bookingRepository);

        // Initialize controllers
        var passengerController = new PassengerController(bookFlightUseCase, searchFlightsUseCase, manageBookingUseCase, passengerRepository);
        var managerController = new ManagerController(managerUseCase, managerRepository);
        
        // Start the menu-based console application
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Airport Ticket Booking System");
            Console.WriteLine("1. Passenger Menu");
            Console.WriteLine("2. Manager Menu");
            Console.WriteLine("3. Exit");
            Console.Write("Select an option: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ShowPassengerMenu(passengerController);
                    break;
                case "2":
                    ShowManagerMenu(managerController);
                    break;
                case "3":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }
        }
    }

    static void ShowPassengerMenu(PassengerController passengerController)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Passenger Menu");
            Console.WriteLine("1. Search Flights");
            Console.WriteLine("2. Book Flight");
            Console.WriteLine("3. View Bookings");
            Console.WriteLine("4. Cancel Booking");
            Console.WriteLine("5. Modify Booking");
            Console.WriteLine("6. Back to Main Menu");
            Console.Write("Select an option: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ExecuteSearchFlights(passengerController);
                    break;
                case "2":
                    ExecuteBookFlight(passengerController);
                    break;
                case "3":
                    ExecuteViewBookings(passengerController);
                    break;
                case "4":
                    ExecuteCancelBooking(passengerController);
                    break;
                case "5":
                    ExecuteModifyBooking(passengerController);
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }
        }
    }

    static void ShowManagerMenu(ManagerController managerController)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Manager Menu");
            Console.WriteLine("1. Filter Bookings");
            Console.WriteLine("2. Import Flights from CSV");
            Console.WriteLine("3. Show Validation Details");
            Console.WriteLine("4. Back to Main Menu");
            Console.Write("Select an option: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ExecuteFilterBookings(managerController);
                    break;
                case "2":
                    ExecuteImportFlights(managerController);
                    break;
                case "3":
                    managerController.ShowValidationDetails();
                    Console.WriteLine("Press any key to return.");
                    Console.ReadKey();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }
        }
    }

    // ========== Passenger Menu Methods ==========

    static void ExecuteSearchFlights(PassengerController passengerController)
    {
        Console.Clear();
        Console.WriteLine("Search for Flights");

        Console.Write("Enter max price (leave blank for no filter): ");
        string priceInput = Console.ReadLine();
        decimal? price = string.IsNullOrEmpty(priceInput) ? (decimal?)null : decimal.Parse(priceInput);

        Console.Write("Enter departure country (leave blank for no filter): ");
        string departureCountry = Console.ReadLine();

        Console.Write("Enter destination country (leave blank for no filter): ");
        string destinationCountry = Console.ReadLine();

        Console.Write("Enter departure date (YYYY-MM-DD) (leave blank for no filter): ");
        string dateInput = Console.ReadLine();
        DateTime? departureDate = string.IsNullOrEmpty(dateInput) ? (DateTime?)null : DateTime.Parse(dateInput);

        Console.Write("Enter departure airport (leave blank for no filter): ");
        string departureAirport = Console.ReadLine();

        Console.Write("Enter arrival airport (leave blank for no filter): ");
        string arrivalAirport = Console.ReadLine();

        Console.Write("Enter flight class (Economy, Business, FirstClass) (leave blank for no filter): ");
        string classInput = Console.ReadLine();
        FlightClass? flightClass = string.IsNullOrEmpty(classInput) ? (FlightClass?)null : (FlightClass)Enum.Parse(typeof(FlightClass), classInput, true);

        passengerController.SearchFlights(price, departureCountry, destinationCountry, departureDate, departureAirport, arrivalAirport, flightClass);

        Console.WriteLine("Press any key to return.");
        Console.ReadKey();
    }

    static void ExecuteBookFlight(PassengerController passengerController)
    {
        Console.Clear();
        Console.WriteLine("Book a Flight");

        Console.Write("Enter passenger ID: ");
        string passengerId = Console.ReadLine();

        Console.Write("Enter flight ID: ");
        string flightId = Console.ReadLine();

        Console.Write("Enter flight class (Economy, Business, FirstClass): ");
        string classInput = Console.ReadLine();
        FlightClass flightClass = (FlightClass)Enum.Parse(typeof(FlightClass), classInput, true);

        passengerController.BookFlight(passengerId, flightId, flightClass);

        Console.WriteLine("Press any key to return.");
        Console.ReadKey();
    }

    static void ExecuteViewBookings(PassengerController passengerController)
    {
        Console.Clear();
        Console.WriteLine("View Bookings");

        Console.Write("Enter passenger ID: ");
        string passengerId = Console.ReadLine();

        passengerController.ViewBookings(passengerId);

        Console.WriteLine("Press any key to return.");
        Console.ReadKey();
    }

    static void ExecuteCancelBooking(PassengerController passengerController)
    {
        Console.Clear();
        Console.WriteLine("Cancel Booking");

        Console.Write("Enter booking ID: ");
        string bookingId = Console.ReadLine();

        passengerController.CancelBooking(bookingId);

        Console.WriteLine("Press any key to return.");
        Console.ReadKey();
    }

    static void ExecuteModifyBooking(PassengerController passengerController)
    {
        Console.Clear();
        Console.WriteLine("Modify Booking");

        Console.Write("Enter booking ID: ");
        string bookingId = Console.ReadLine();

        Console.Write("Enter new flight class (Economy, Business, FirstClass): ");
        string classInput = Console.ReadLine();
        FlightClass flightClass = (FlightClass)Enum.Parse(typeof(FlightClass), classInput, true);

        passengerController.ModifyBooking(bookingId, flightClass);

        Console.WriteLine("Press any key to return.");
        Console.ReadKey();
    }

    // ========== Manager Menu Methods ==========

    static void ExecuteFilterBookings(ManagerController managerController)
    {
        Console.Clear();
        Console.WriteLine("Filter Bookings");

        Console.Write("Enter flight ID (leave blank for no filter): ");
        string flightId = Console.ReadLine();

        Console.Write("Enter max price (leave blank for no filter): ");
        string priceInput = Console.ReadLine();
        decimal? price = string.IsNullOrEmpty(priceInput) ? (decimal?)null : decimal.Parse(priceInput);

        Console.Write("Enter departure country (leave blank for no filter): ");
        string departureCountry = Console.ReadLine();

        Console.Write("Enter destination country (leave blank for no filter): ");
        string destinationCountry = Console.ReadLine();

        Console.Write("Enter departure date (YYYY-MM-DD) (leave blank for no filter): ");
        string dateInput = Console.ReadLine();
        DateTime? departureDate = string.IsNullOrEmpty(dateInput) ? (DateTime?)null : DateTime.Parse(dateInput);

        Console.Write("Enter departure airport (leave blank for no filter): ");
        string departureAirport = Console.ReadLine();

        Console.Write("Enter arrival airport (leave blank for no filter): ");
        string arrivalAirport = Console.ReadLine();

        Console.Write("Enter passenger ID (leave blank for no filter): ");
        string passengerId = Console.ReadLine();

        Console.Write("Enter flight class (Economy, Business, FirstClass) (leave blank for no filter): ");
        string classInput = Console.ReadLine();
        FlightClass? flightClass = string.IsNullOrEmpty(classInput) ? (FlightClass?)null : (FlightClass)Enum.Parse(typeof(FlightClass), classInput, true);

        managerController.FilterBookings(flightId, price, departureCountry, destinationCountry, departureDate, departureAirport, arrivalAirport, passengerId, flightClass);

        Console.WriteLine("Press any key to return.");
        Console.ReadKey();
    }

    static void ExecuteImportFlights(ManagerController managerController)
    {
        Console.Clear();
        Console.WriteLine("Import Flights from CSV");

        Console.Write("Enter path to CSV file: ");
        string csvFilePath = Console.ReadLine();

        managerController.ImportFlights(csvFilePath);

        Console.WriteLine("Press any key to return.");
        Console.ReadKey();
    }
}
