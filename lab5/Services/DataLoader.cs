using System.Text.Json;
using lab5.Models;

namespace lab5.Services;

public static class DataLoader
{
    public static List<Vehicle> LoadVehiclesFromJson(string filePath)
    {
        var vehicles = new List<Vehicle>();

        try
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Файл {filePath} не найден.");
                return vehicles;
            }

            string jsonContent = File.ReadAllText(filePath);
            var vehicleDataList = JsonSerializer.Deserialize<List<VehicleData>>(jsonContent);

            if (vehicleDataList == null)
            {
                Console.WriteLine("Ошибка при десериализации данных.");
                return vehicles;
            }

            foreach (var data in vehicleDataList)
            {
                Vehicle? vehicle = data.Type.ToLower() switch
                {
                    "sedan" => new Sedan(
                        data.Brand, data.Model, data.Year, data.Price,
                        data.FuelConsumption, data.MaxSpeed, data.Color, data.Mileage,
                        data.TrunkVolume ?? 400, data.HasSunroof ?? false),

                    "hatchback" => new Hatchback(
                        data.Brand, data.Model, data.Year, data.Price,
                        data.FuelConsumption, data.MaxSpeed, data.Color, data.Mileage,
                        data.NumberOfDoors ?? 5, data.HasFoldingSeats ?? true),

                    "suv" => new SUV(
                        data.Brand, data.Model, data.Year, data.Price,
                        data.FuelConsumption, data.MaxSpeed, data.Color, data.Mileage,
                        data.HasAllWheelDrive ?? true, data.GroundClearance ?? 200, data.SeatingCapacity ?? 7),

                    "crossover" => new Crossover(
                        data.Brand, data.Model, data.Year, data.Price,
                        data.FuelConsumption, data.MaxSpeed, data.Color, data.Mileage,
                        data.HasPanoramicRoof ?? false, data.GroundClearance ?? 180, data.DriveType ?? "Передний"),

                    "stationwagon" => new StationWagon(
                        data.Brand, data.Model, data.Year, data.Price,
                        data.FuelConsumption, data.MaxSpeed, data.Color, data.Mileage,
                        data.CargoVolume ?? 500, data.HasRoofRails ?? true, data.SeatingCapacity ?? 5),

                    _ => null
                };

                if (vehicle != null)
                {
                    vehicles.Add(vehicle);
                }
            }

            Console.WriteLine($"Загружено {vehicles.Count} автомобилей из файла {filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке данных: {ex.Message}");
        }

        return vehicles;
    }

    public static bool SaveVehiclesToJson(string filePath, IEnumerable<Vehicle> vehicles)
    {
        try
        {
            var vehicleDataList = new List<VehicleData>();

            foreach (var vehicle in vehicles)
            {
                var data = new VehicleData
                {
                    Brand = vehicle.Brand,
                    Model = vehicle.Model,
                    Year = vehicle.Year,
                    Price = vehicle.Price,
                    FuelConsumption = vehicle.FuelConsumption,
                    MaxSpeed = vehicle.MaxSpeed,
                    Color = vehicle.Color,
                    Mileage = vehicle.Mileage
                };

                switch (vehicle)
                {
                    case Sedan sedan:
                        data.Type = "sedan";
                        data.TrunkVolume = sedan.TrunkVolume;
                        data.HasSunroof = sedan.HasSunroof;
                        break;

                    case Hatchback hatchback:
                        data.Type = "hatchback";
                        data.NumberOfDoors = hatchback.NumberOfDoors;
                        data.HasFoldingSeats = hatchback.HasFoldingSeats;
                        break;

                    case SUV suv:
                        data.Type = "suv";
                        data.HasAllWheelDrive = suv.HasAllWheelDrive;
                        data.GroundClearance = suv.GroundClearance;
                        data.SeatingCapacity = suv.SeatingCapacity;
                        break;

                    case Crossover crossover:
                        data.Type = "crossover";
                        data.HasPanoramicRoof = crossover.HasPanoramicRoof;
                        data.GroundClearance = crossover.GroundClearance;
                        data.DriveType = crossover.DriveType;
                        break;

                    case StationWagon wagon:
                        data.Type = "stationwagon";
                        data.CargoVolume = wagon.CargoVolume;
                        data.HasRoofRails = wagon.HasRoofRails;
                        data.SeatingCapacity = wagon.SeatingCapacity;
                        break;
                }

                vehicleDataList.Add(data);
            }

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            string jsonContent = JsonSerializer.Serialize(vehicleDataList, options);
            File.WriteAllText(filePath, jsonContent);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при сохранении данных: {ex.Message}");
            return false;
        }
    }

    private class VehicleData
    {
        public string Type { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal Price { get; set; }
        public double FuelConsumption { get; set; }
        public int MaxSpeed { get; set; }
        public string Color { get; set; } = string.Empty;
        public int Mileage { get; set; }

        public int? TrunkVolume { get; set; }
        public bool? HasSunroof { get; set; }
        public int? NumberOfDoors { get; set; }
        public bool? HasFoldingSeats { get; set; }
        public bool? HasAllWheelDrive { get; set; }
        public int? GroundClearance { get; set; }
        public int? SeatingCapacity { get; set; }
        public bool? HasPanoramicRoof { get; set; }
        public string? DriveType { get; set; }
        public int? CargoVolume { get; set; }
        public bool? HasRoofRails { get; set; }
    }
}
