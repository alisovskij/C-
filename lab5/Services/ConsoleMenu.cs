using lab5.Models;

namespace lab5.Services;

public class ConsoleMenu
{
    private const string Separator = "────────────────────────────────────────────────────────────";
    private const string DoubleSeparator = "============================================================";

    private readonly TaxiPark _taxiPark;
    private readonly AuthenticationService _authService;
    private readonly string _dataFilePath;
    private bool _running;

    public ConsoleMenu(TaxiPark taxiPark, AuthenticationService authService, string dataFilePath)
    {
        _taxiPark = taxiPark;
        _authService = authService;
        _dataFilePath = dataFilePath;
        _running = true;
    }

    public void Run()
    {
        while (_running)
        {
            DisplayMainMenu();
            ProcessChoice(Console.ReadLine() ?? "");
        }
    }

    private void DisplayMainMenu()
    {
        Console.Clear();
        ShowHeader("СИСТЕМА УПРАВЛЕНИЯ ТАКСОПАРКОМ");

        Console.WriteLine($"\nПользователь: {_authService.CurrentUser?.FullName} ({_authService.CurrentUser?.Role})");
        Console.WriteLine($"Таксопарк: {_taxiPark.Name}");
        Console.WriteLine($"Автомобилей в парке: {_taxiPark.GetVehicleCount()}");
        Console.WriteLine(Separator);
        Console.WriteLine("\n[1]  Просмотр всех автомобилей");
        Console.WriteLine("[2]  Информация об автопарке");
        Console.WriteLine("[3]  Сортировка автомобилей");
        Console.WriteLine("[4]  Поиск автомобилей");
        Console.WriteLine("[5]  Группировка по типам");
        Console.WriteLine("[6]  Статистика по типам");
        Console.WriteLine("[7]  Добавить новый автомобиль");
        Console.WriteLine("[8]  Удалить автомобиль");
        Console.WriteLine("[9]  Выйти из аккаунта");
        Console.WriteLine("[0]  Выход из программы");
        Console.WriteLine(Separator);
        Console.Write("\nВыберите пункт меню: ");
    }

    private void ShowHeader(string title)
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine($"║{title.PadLeft((60 + title.Length) / 2).PadRight(60)}║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
    }

    private void WaitForKey()
    {
        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    private void ProcessChoice(string choice)
    {
        Console.WriteLine();

        switch (choice)
        {
            case "1":
                ShowAllVehicles();
                break;
            case "2":
                ShowParkInfo();
                break;
            case "3":
                SortingMenu();
                break;
            case "4":
                SearchMenu();
                break;
            case "5":
                ShowGroupedByType();
                break;
            case "6":
                ShowStatistics();
                break;
            case "7":
                AddNewVehicle();
                break;
            case "8":
                RemoveVehicle();
                break;
            case "9":
                Logout();
                return;
            case "0":
                _running = false;
                Console.WriteLine("До свидания!");
                return;
            default:
                Console.WriteLine("Неверный выбор. Попробуйте снова.");
                break;
        }

        if (_running)
            WaitForKey();
    }

    private void ShowAllVehicles()
    {
        ShowHeader("ВСЕ АВТОМОБИЛИ В ПАРКЕ");

        var vehicles = _taxiPark.GetAllVehicles();
        if (vehicles.Count == 0)
        {
            Console.WriteLine("\nВ парке нет автомобилей.");
            return;
        }

        for (int i = 0; i < vehicles.Count; i++)
        {
            Console.WriteLine($"\n[{i + 1}] {vehicles[i].GetFullInfo()}");
            Console.WriteLine(Separator);
        }
    }

    private void ShowParkInfo()
    {
        ShowHeader("ИНФОРМАЦИЯ ОБ АВТОПАРКЕ");

        var vehicles = _taxiPark.GetAllVehicles();
        if (!CheckVehiclesExist(vehicles))
            return;

        Console.WriteLine($"\nНазвание: {_taxiPark.Name}");
        Console.WriteLine($"Количество автомобилей: {_taxiPark.GetVehicleCount()}");
        Console.WriteLine($"Общая стоимость: {_taxiPark.CalculateTotalCost():C}");
        Console.WriteLine($"Средняя стоимость: {_taxiPark.CalculateTotalCost() / _taxiPark.GetVehicleCount():C}");
        Console.WriteLine($"Средний расход: {vehicles.Average(v => v.FuelConsumption):F2} л/100км");
        Console.WriteLine($"Средняя скорость: {vehicles.Average(v => v.MaxSpeed):F0} км/ч");
    }

    private bool CheckVehiclesExist(IReadOnlyList<Vehicle> vehicles)
    {
        if (vehicles.Count == 0)
        {
            Console.WriteLine("\nВ парке нет автомобилей.");
            return false;
        }
        return true;
    }

    private void SortingMenu()
    {
        ShowHeader("СОРТИРОВКА АВТОМОБИЛЕЙ");

        Console.WriteLine("[1] Сортировка по расходу топлива");
        Console.WriteLine("[2] Сортировка по цене");
        Console.WriteLine("[3] Сортировка по максимальной скорости");
        Console.Write("\nВыберите тип сортировки: ");

        string choice = Console.ReadLine() ?? "";
        Console.WriteLine();

        switch (choice)
        {
            case "1":
                _taxiPark.SortByFuelConsumption();
                Console.WriteLine("✓ Автомобили отсортированы по расходу топлива\n");
                DisplaySortedVehicles("расходу топлива", v => $"{v.FuelConsumption:F2} л/100км");
                break;
            case "2":
                _taxiPark.SortByPrice();
                Console.WriteLine("✓ Автомобили отсортированы по цене\n");
                DisplaySortedVehicles("цене", v => $"{v.Price:C}");
                break;
            case "3":
                _taxiPark.SortByMaxSpeed();
                Console.WriteLine("✓ Автомобили отсортированы по максимальной скорости\n");
                DisplaySortedVehicles("максимальной скорости", v => $"{v.MaxSpeed} км/ч");
                break;
            default:
                Console.WriteLine("Неверный выбор.");
                break;
        }
    }

    private void DisplaySortedVehicles(string sortBy, Func<Vehicle, string> valueSelector)
    {
        var vehicles = _taxiPark.GetAllVehicles();
        Console.WriteLine($"Топ-10 автомобилей по {sortBy}:\n");

        int count = 0;
        foreach (var vehicle in vehicles.Take(10))
        {
            count++;
            Console.WriteLine($"{count,2}. {vehicle.Brand} {vehicle.Model,-20} - {valueSelector(vehicle)}");
        }
    }

    private void SearchMenu()
    {
        ShowHeader("ПОИСК АВТОМОБИЛЕЙ");

        Console.WriteLine("[1] Поиск по диапазону скорости");
        Console.WriteLine("[2] Поиск по диапазону цены");
        Console.WriteLine("[3] Поиск по типу автомобиля");
        Console.Write("\nВыберите тип поиска: ");

        string choice = Console.ReadLine() ?? "";
        Console.WriteLine();

        switch (choice)
        {
            case "1":
                SearchBySpeedRange();
                break;
            case "2":
                SearchByPriceRange();
                break;
            case "3":
                SearchByType();
                break;
            default:
                Console.WriteLine("Неверный выбор.");
                break;
        }
    }

    private void SearchBySpeedRange()
    {
        Console.Write("Введите минимальную скорость (км/ч): ");
        if (!int.TryParse(Console.ReadLine(), out int minSpeed))
        {
            Console.WriteLine("Неверный ввод.");
            return;
        }

        Console.Write("Введите максимальную скорость (км/ч): ");
        if (!int.TryParse(Console.ReadLine(), out int maxSpeed))
        {
            Console.WriteLine("Неверный ввод.");
            return;
        }

        var results = _taxiPark.FindVehiclesBySpeedRange(minSpeed, maxSpeed);
        Console.WriteLine($"\n✓ Найдено автомобилей: {results.Count}\n");

        if (results.Count > 0)
        {
            foreach (var vehicle in results)
            {
                Console.WriteLine($"  • {vehicle.Brand} {vehicle.Model,-20} - {vehicle.MaxSpeed} км/ч");
            }
        }
        else
        {
            Console.WriteLine("Автомобили в заданном диапазоне не найдены.");
        }
    }

    private void SearchByPriceRange()
    {
        Console.Write("Введите минимальную цену: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal minPrice))
        {
            Console.WriteLine("Неверный ввод.");
            return;
        }

        Console.Write("Введите максимальную цену: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal maxPrice))
        {
            Console.WriteLine("Неверный ввод.");
            return;
        }

        var results = _taxiPark.FindVehiclesByPriceRange(minPrice, maxPrice);
        Console.WriteLine($"\n✓ Найдено автомобилей: {results.Count}\n");

        if (results.Count > 0)
        {
            foreach (var vehicle in results)
            {
                Console.WriteLine($"  • {vehicle.Brand} {vehicle.Model,-20} - {vehicle.Price:C}");
            }
        }
        else
        {
            Console.WriteLine("Автомобили в заданном диапазоне не найдены.");
        }
    }

    private void SearchByType()
    {
        Console.WriteLine("\nВыберите тип автомобиля:");
        Console.WriteLine("[1] Седан");
        Console.WriteLine("[2] Хэтчбек");
        Console.WriteLine("[3] Внедорожник");
        Console.WriteLine("[4] Кроссовер");
        Console.WriteLine("[5] Универсал");
        Console.Write("\nВыбор: ");

        string choice = Console.ReadLine() ?? "";
        Type? vehicleType = choice switch
        {
            "1" => typeof(Sedan),
            "2" => typeof(Hatchback),
            "3" => typeof(SUV),
            "4" => typeof(Crossover),
            "5" => typeof(StationWagon),
            _ => null
        };

        if (vehicleType == null)
        {
            Console.WriteLine("Неверный выбор.");
            return;
        }

        var results = _taxiPark.FindVehiclesByType(vehicleType);
        Console.WriteLine($"\n✓ Найдено автомобилей: {results.Count}\n");

        if (results.Count > 0)
        {
            foreach (var vehicle in results)
            {
                Console.WriteLine($"  • {vehicle.Brand} {vehicle.Model,-20} - {vehicle.Price:C}");
            }
        }
    }

    private void ShowGroupedByType()
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║            ГРУППИРОВКА ПО ТИПАМ АВТОМОБИЛЕЙ                ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

        _taxiPark.DisplayVehiclesSummary();
    }

    private void ShowStatistics()
    {
        ShowHeader("СТАТИСТИКА ПО ТИПАМ");

        var vehicles = _taxiPark.GetAllVehicles();
        if (!CheckVehiclesExist(vehicles))
            return;

        var grouped = vehicles.GroupBy(v => v.GetVehicleType());

        foreach (var group in grouped)
        {
            var avgPrice = group.Average(v => (double)v.Price);
            var avgFuel = group.Average(v => v.FuelConsumption);
            var avgSpeed = group.Average(v => v.MaxSpeed);
            var totalPrice = group.Sum(v => v.Price);

            Console.WriteLine($"{group.Key}:");
            Console.WriteLine($"  Количество: {group.Count()} шт.");
            Console.WriteLine($"  Общая стоимость: {totalPrice:C}");
            Console.WriteLine($"  Средняя цена: {avgPrice:C}");
            Console.WriteLine($"  Средний расход: {avgFuel:F2} л/100км");
            Console.WriteLine($"  Средняя макс. скорость: {avgSpeed:F0} км/ч");
            Console.WriteLine();
        }
    }

    private void AddNewVehicle()
    {
        ShowHeader("ДОБАВЛЕНИЕ НОВОГО АВТОМОБИЛЯ");

        Console.WriteLine("Выберите тип автомобиля:");
        Console.WriteLine("[1] Седан");
        Console.WriteLine("[2] Хэтчбек");
        Console.WriteLine("[3] Внедорожник");
        Console.WriteLine("[4] Кроссовер");
        Console.WriteLine("[5] Универсал");
        Console.Write("\nВыбор: ");

        string typeChoice = Console.ReadLine() ?? "";

        try
        {
            Console.Write("\nМарка: ");
            string brand = Console.ReadLine() ?? "";

            Console.Write("Модель: ");
            string model = Console.ReadLine() ?? "";

            Console.Write("Год выпуска: ");
            int year = int.Parse(Console.ReadLine() ?? "2023");

            Console.Write("Цена: ");
            decimal price = decimal.Parse(Console.ReadLine() ?? "0");

            Console.Write("Расход топлива (л/100км): ");
            double fuelConsumption = double.Parse(Console.ReadLine() ?? "0");

            Console.Write("Максимальная скорость (км/ч): ");
            int maxSpeed = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("Цвет: ");
            string color = Console.ReadLine() ?? "";

            Console.Write("Пробег (км): ");
            int mileage = int.Parse(Console.ReadLine() ?? "0");

            Vehicle? newVehicle = null;

            switch (typeChoice)
            {
                case "1":
                    Console.Write("Объем багажника (л): ");
                    int trunkVolume = int.Parse(Console.ReadLine() ?? "400");
                    Console.Write("Есть люк? (да/нет): ");
                    bool hasSunroof = Console.ReadLine()?.ToLower() == "да";
                    newVehicle = new Sedan(brand, model, year, price, fuelConsumption,
                        maxSpeed, color, mileage, trunkVolume, hasSunroof);
                    break;

                case "2":
                    Console.Write("Количество дверей: ");
                    int doors = int.Parse(Console.ReadLine() ?? "5");
                    Console.Write("Складывающиеся сиденья? (да/нет): ");
                    bool foldingSeats = Console.ReadLine()?.ToLower() == "да";
                    newVehicle = new Hatchback(brand, model, year, price, fuelConsumption,
                        maxSpeed, color, mileage, doors, foldingSeats);
                    break;

                case "3":
                    Console.Write("Полный привод? (да/нет): ");
                    bool awd = Console.ReadLine()?.ToLower() == "да";
                    Console.Write("Клиренс (мм): ");
                    int clearance = int.Parse(Console.ReadLine() ?? "200");
                    Console.Write("Количество мест: ");
                    int seats = int.Parse(Console.ReadLine() ?? "7");
                    newVehicle = new SUV(brand, model, year, price, fuelConsumption,
                        maxSpeed, color, mileage, awd, clearance, seats);
                    break;

                case "4":
                    Console.Write("Панорамная крыша? (да/нет): ");
                    bool panoramicRoof = Console.ReadLine()?.ToLower() == "да";
                    Console.Write("Клиренс (мм): ");
                    int clearance2 = int.Parse(Console.ReadLine() ?? "180");
                    Console.Write("Тип привода (Передний/Задний/Полный): ");
                    string driveType = Console.ReadLine() ?? "Передний";
                    newVehicle = new Crossover(brand, model, year, price, fuelConsumption,
                        maxSpeed, color, mileage, panoramicRoof, clearance2, driveType);
                    break;

                case "5":
                    Console.Write("Объем грузового отсека (л): ");
                    int cargoVolume = int.Parse(Console.ReadLine() ?? "500");
                    Console.Write("Рейлинги на крыше? (да/нет): ");
                    bool roofRails = Console.ReadLine()?.ToLower() == "да";
                    Console.Write("Количество мест: ");
                    int seats2 = int.Parse(Console.ReadLine() ?? "5");
                    newVehicle = new StationWagon(brand, model, year, price, fuelConsumption,
                        maxSpeed, color, mileage, cargoVolume, roofRails, seats2);
                    break;

                default:
                    Console.WriteLine("Неверный выбор типа автомобиля.");
                    return;
            }

            if (newVehicle != null)
            {
                _taxiPark.AddVehicle(newVehicle);
                Console.WriteLine("\n✓ Автомобиль успешно добавлен!");
                Console.WriteLine($"\n{newVehicle.GetFullInfo()}");

                Console.WriteLine("\nСохранение в файл...");
                bool saved = DataLoader.SaveVehiclesToJson(_dataFilePath, _taxiPark.GetAllVehicles());

                if (saved)
                {
                    Console.WriteLine("✓ Данные успешно сохранены в vehicles.json");
                }
                else
                {
                    Console.WriteLine("Не удалось сохранить данные в файл");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nОшибка при добавлении автомобиля: {ex.Message}");
        }
    }

    private void RemoveVehicle()
    {
        ShowHeader("УДАЛЕНИЕ АВТОМОБИЛЯ");

        var vehicles = _taxiPark.GetAllVehicles();
        if (!CheckVehiclesExist(vehicles))
            return;

        Console.WriteLine("\nВсе автомобили в парке:\n");
        for (int i = 0; i < vehicles.Count; i++)
        {
            Console.WriteLine($"[{i + 1}] {vehicles[i].Brand} {vehicles[i].Model} ({vehicles[i].Year}) - {vehicles[i].Price:C}");
        }

        Console.WriteLine(Separator);
        Console.Write("\nВведите номер автомобиля для удаления (0 - отмена): ");

        if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 0 || choice > vehicles.Count)
        {
            Console.WriteLine("Неверный ввод.");
            return;
        }

        if (choice == 0)
        {
            Console.WriteLine("Операция отменена.");
            return;
        }

        int index = choice - 1;
        var vehicleToRemove = vehicles[index];

        Console.WriteLine($"\nВы действительно хотите удалить автомобиль:");
        Console.WriteLine($"{vehicleToRemove.GetFullInfo()}");
        Console.Write("\nПодтвердите удаление (да/нет): ");

        string confirmation = Console.ReadLine()?.ToLower() ?? "";
        if (confirmation != "да" && confirmation != "yes")
        {
            Console.WriteLine("Операция отменена.");
            return;
        }

        if (_taxiPark.RemoveVehicle(index))
        {
            Console.WriteLine("\n✓ Автомобиль успешно удален из парка!");

            Console.WriteLine("\nСохранение в файл...");
            bool saved = DataLoader.SaveVehiclesToJson(_dataFilePath, _taxiPark.GetAllVehicles());

            if (saved)
            {
                Console.WriteLine("✓ Данные успешно сохранены в vehicles.json");
            }
            else
            {
                Console.WriteLine("Не удалось сохранить данные в файл");
            }
        }
        else
        {
            Console.WriteLine("\nОшибка при удалении автомобиля.");
        }
    }

    private void Logout()
    {
        Console.WriteLine($"\n{_authService.CurrentUser?.FullName}, вы вышли из системы.");
        WaitForKey();
        _authService.Logout();
        _running = false;
    }
}
