namespace lab5.Models;

public class StationWagon : Vehicle
{
    public int CargoVolume { get; set; }
    public bool HasRoofRails { get; set; }
    public int SeatingCapacity { get; set; }

    public StationWagon(string brand, string model, int year, decimal price,
        double fuelConsumption, int maxSpeed, string color, int mileage,
        int cargoVolume, bool hasRoofRails, int seatingCapacity)
        : base(brand, model, year, price, fuelConsumption, maxSpeed, color, mileage)
    {
        CargoVolume = cargoVolume;
        HasRoofRails = hasRoofRails;
        SeatingCapacity = seatingCapacity;
    }

    public override string GetVehicleType()
    {
        return "Универсал";
    }

    public override string GetFullInfo()
    {
        return base.GetFullInfo() +
               $"\n  Объем грузового отсека: {CargoVolume} л" +
               $"\n  Рейлинги на крыше: {(HasRoofRails ? "Да" : "Нет")}" +
               $"\n  Вместимость: {SeatingCapacity} мест";
    }

    public override object Clone()
    {
        return new StationWagon(Brand, Model, Year, Price, FuelConsumption,
            MaxSpeed, Color, Mileage, CargoVolume, HasRoofRails, SeatingCapacity);
    }
}
