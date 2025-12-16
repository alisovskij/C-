namespace lab5.Models;

public class Crossover : Vehicle
{
    public bool HasPanoramicRoof { get; set; }
    public int GroundClearance { get; set; }
    public string DriveType { get; set; }

    public Crossover(string brand, string model, int year, decimal price,
        double fuelConsumption, int maxSpeed, string color, int mileage,
        bool hasPanoramicRoof, int groundClearance, string driveType)
        : base(brand, model, year, price, fuelConsumption, maxSpeed, color, mileage)
    {
        HasPanoramicRoof = hasPanoramicRoof;
        GroundClearance = groundClearance;
        DriveType = driveType;
    }

    public override string GetVehicleType()
    {
        return "Кроссовер";
    }

    public override string GetFullInfo()
    {
        return base.GetFullInfo() +
               $"\n  Панорамная крыша: {(HasPanoramicRoof ? "Да" : "Нет")}" +
               $"\n  Клиренс: {GroundClearance} мм" +
               $"\n  Тип привода: {DriveType}";
    }

    public override object Clone()
    {
        return new Crossover(Brand, Model, Year, Price, FuelConsumption,
            MaxSpeed, Color, Mileage, HasPanoramicRoof, GroundClearance, DriveType);
    }
}
