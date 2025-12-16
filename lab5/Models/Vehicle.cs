namespace lab5.Models;

public abstract class Vehicle : IComparable<Vehicle>, ICloneable
{
    public string Brand { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public decimal Price { get; set; }
    public double FuelConsumption { get; set; }
    public int MaxSpeed { get; set; }
    public string Color { get; set; }
    public int Mileage { get; set; }

    protected Vehicle(string brand, string model, int year, decimal price,
        double fuelConsumption, int maxSpeed, string color, int mileage)
    {
        Brand = brand;
        Model = model;
        Year = year;
        Price = price;
        FuelConsumption = fuelConsumption;
        MaxSpeed = maxSpeed;
        Color = color;
        Mileage = mileage;
    }

    public abstract string GetVehicleType();

    public virtual string GetFullInfo()
    {
        return $"{GetVehicleType()}: {Brand} {Model} ({Year})\n" +
               $"  Цена: {Price:C}\n" +
               $"  Расход топлива: {FuelConsumption} л/100км\n" +
               $"  Макс. скорость: {MaxSpeed} км/ч\n" +
               $"  Цвет: {Color}\n" +
               $"  Пробег: {Mileage} км";
    }

    public int CompareTo(Vehicle? other)
    {
        if (other == null) return 1;
        return FuelConsumption.CompareTo(other.FuelConsumption);
    }

    public abstract object Clone();

    public override string ToString()
    {
        return $"{Brand} {Model} ({Year}) - {Price:C}";
    }
}
