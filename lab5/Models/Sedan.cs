namespace lab5.Models;

public class Sedan : Vehicle
{
    public int TrunkVolume { get; set; }
    public bool HasSunroof { get; set; }

    public Sedan(string brand, string model, int year, decimal price,
        double fuelConsumption, int maxSpeed, string color, int mileage,
        int trunkVolume, bool hasSunroof)
        : base(brand, model, year, price, fuelConsumption, maxSpeed, color, mileage)
    {
        TrunkVolume = trunkVolume;
        HasSunroof = hasSunroof;
    }

    public override string GetVehicleType()
    {
        return "Седан";
    }

    public override string GetFullInfo()
    {
        return base.GetFullInfo() +
               $"\n  Объем багажника: {TrunkVolume} л" +
               $"\n  Люк: {(HasSunroof ? "Да" : "Нет")}";
    }

    public override object Clone()
    {
        return new Sedan(Brand, Model, Year, Price, FuelConsumption,
            MaxSpeed, Color, Mileage, TrunkVolume, HasSunroof);
    }
}
