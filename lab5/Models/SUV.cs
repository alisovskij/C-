namespace lab5.Models;

public class SUV : Vehicle
{
    public bool HasAllWheelDrive { get; set; }
    public int GroundClearance { get; set; }
    public int SeatingCapacity { get; set; }

    public SUV(string brand, string model, int year, decimal price,
        double fuelConsumption, int maxSpeed, string color, int mileage,
        bool hasAllWheelDrive, int groundClearance, int seatingCapacity)
        : base(brand, model, year, price, fuelConsumption, maxSpeed, color, mileage)
    {
        HasAllWheelDrive = hasAllWheelDrive;
        GroundClearance = groundClearance;
        SeatingCapacity = seatingCapacity;
    }

    public override string GetVehicleType()
    {
        return "Внедорожник";
    }

    public override string GetFullInfo()
    {
        return base.GetFullInfo() +
               $"\n  Полный привод: {(HasAllWheelDrive ? "Да" : "Нет")}" +
               $"\n  Клиренс: {GroundClearance} мм" +
               $"\n  Вместимость: {SeatingCapacity} мест";
    }

    public override object Clone()
    {
        return new SUV(Brand, Model, Year, Price, FuelConsumption,
            MaxSpeed, Color, Mileage, HasAllWheelDrive, GroundClearance, SeatingCapacity);
    }
}
