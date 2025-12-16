using lab5.Models;

namespace lab5.Services;

public class TaxiPark
{
    private readonly List<Vehicle> _vehicles;
    public string Name { get; set; }

    public TaxiPark(string name)
    {
        Name = name;
        _vehicles = new List<Vehicle>();
    }

    public void AddVehicle(Vehicle vehicle) => _vehicles.Add(vehicle);

    public void AddVehicles(IEnumerable<Vehicle> vehicles) => _vehicles.AddRange(vehicles);

    public bool RemoveVehicle(int index)
    {
        if (index >= 0 && index < _vehicles.Count)
        {
            _vehicles.RemoveAt(index);
            return true;
        }
        return false;
    }

    public decimal CalculateTotalCost() => _vehicles.Sum(v => v.Price);

    public int GetVehicleCount() => _vehicles.Count;

    public IReadOnlyList<Vehicle> GetAllVehicles() => _vehicles.AsReadOnly();

    public void SortByFuelConsumption() => _vehicles.Sort();

    public void SortByPrice() => _vehicles.Sort((v1, v2) => v1.Price.CompareTo(v2.Price));

    public void SortByMaxSpeed() => _vehicles.Sort((v1, v2) => v1.MaxSpeed.CompareTo(v2.MaxSpeed));

    public List<Vehicle> FindVehiclesBySpeedRange(int minSpeed, int maxSpeed) =>
        _vehicles.Where(v => v.MaxSpeed >= minSpeed && v.MaxSpeed <= maxSpeed).ToList();

    public List<Vehicle> FindVehiclesByPriceRange(decimal minPrice, decimal maxPrice) =>
        _vehicles.Where(v => v.Price >= minPrice && v.Price <= maxPrice).ToList();

    public List<Vehicle> FindVehiclesByType(Type vehicleType) =>
        _vehicles.Where(v => v.GetType() == vehicleType).ToList();

    public void DisplayVehiclesSummary()
    {
        Console.WriteLine("\nКраткая информация об автомобилях:");
        Console.WriteLine(new string('-', 60));

        foreach (var group in _vehicles.GroupBy(v => v.GetVehicleType()))
        {
            Console.WriteLine($"\n{group.Key} ({group.Count()} шт.):");
            foreach (var vehicle in group)
                Console.WriteLine($"  - {vehicle}");
        }

        Console.WriteLine(new string('-', 60));
    }
}
