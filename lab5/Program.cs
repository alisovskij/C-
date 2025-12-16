using lab5.Models;
using lab5.Services;

namespace lab5;

class Program
{
    private const string UsersFilePath = "Data/users.json";
    private const string VehiclesFilePath = "Data/vehicles.json";

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        if (!TryAuthenticate(out var authService))
            return;

        var taxiPark = InitializeTaxiPark();
        RunApplication(taxiPark, authService);

        Console.WriteLine("\nСпасибо за использование системы управления таксопарком!");
    }

    private static bool TryAuthenticate(out AuthenticationService authService)
    {
        authService = new AuthenticationService();

        ShowHeader();
        authService.LoadUsers(UsersFilePath);

        if (!authService.AttemptLogin())
        {
            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
            return false;
        }

        return true;
    }

    private static TaxiPark InitializeTaxiPark()
    {
        Console.Clear();
        var taxiPark = new TaxiPark("Городское Такси");
        var vehicles = DataLoader.LoadVehiclesFromJson(VehiclesFilePath);

        if (vehicles.Count == 0)
        {
            vehicles = CreateSampleVehicles();
            DataLoader.SaveVehiclesToJson(VehiclesFilePath, vehicles);
        }

        taxiPark.AddVehicles(vehicles);
        return taxiPark;
    }

    private static void RunApplication(TaxiPark taxiPark, AuthenticationService authService)
    {
        while (true)
        {
            var menu = new ConsoleMenu(taxiPark, authService, VehiclesFilePath);
            menu.Run();

            if (authService.IsAuthenticated())
                break;

            if (!PromptRelogin(authService))
                break;
        }
    }

    private static bool PromptRelogin(AuthenticationService authService)
    {
        Console.Clear();
        Console.WriteLine("Хотите войти под другим пользователем? (да/нет)");
        Console.Write("Ответ: ");

        var answer = Console.ReadLine()?.ToLower();
        return (answer == "да" || answer == "yes") && authService.AttemptLogin();
    }

    private static void ShowHeader()
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║              СИСТЕМА УПРАВЛЕНИЯ ТАКСОПАРКОМ                ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");
    }

    static List<Vehicle> CreateSampleVehicles()
    {
        return new List<Vehicle>
        {
            new Sedan("Toyota", "Camry", 2022, 2500000, 7.2, 210, "Серебристый", 15000, 524, true),
            new Sedan("Hyundai", "Sonata", 2021, 2200000, 7.8, 205, "Черный", 32000, 510, false),
            new Sedan("Volkswagen", "Passat", 2023, 2800000, 6.9, 220, "Синий", 8000, 586, true),
            new Hatchback("Volkswagen", "Golf", 2022, 1800000, 6.5, 200, "Красный", 21000, 5, true),
            new Hatchback("Ford", "Focus", 2021, 1600000, 7.0, 195, "Белый", 45000, 5, true),
            new SUV("Toyota", "Land Cruiser", 2023, 5500000, 12.5, 210, "Черный", 5000, true, 230, 7),
            new SUV("Nissan", "Patrol", 2022, 4800000, 13.2, 200, "Белый", 18000, true, 225, 7),
            new Crossover("Hyundai", "Tucson", 2023, 2400000, 8.1, 190, "Серый", 12000, true, 185, "Полный"),
            new Crossover("Kia", "Sportage", 2022, 2300000, 8.3, 185, "Красный", 25000, false, 182, "Передний"),
            new Crossover("Mazda", "CX-5", 2023, 2700000, 7.9, 195, "Синий", 9000, true, 178, "Полный"),
            new StationWagon("Skoda", "Octavia Combi", 2022, 2100000, 6.8, 210, "Зеленый", 28000, 640, true, 5),
            new StationWagon("Volkswagen", "Passat Variant", 2023, 2900000, 7.1, 220, "Серебристый", 11000, 650, true, 5)
        };
    }
}
