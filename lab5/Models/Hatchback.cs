namespace lab5.Models;

public class Hatchback : Vehicle
{
    public int NumberOfDoors { get; set; }
    public bool HasFoldingSeats { get; set; }

    public Hatchback(string brand, string model, int year, decimal price,
        double fuelConsumption, int maxSpeed, string color, int mileage,
        int numberOfDoors, bool hasFoldingSeats)
        : base(brand, model, year, price, fuelConsumption, maxSpeed, color, mileage)
    {
        NumberOfDoors = numberOfDoors;
        HasFoldingSeats = hasFoldingSeats;
    }

    public override string GetVehicleType()
    {
        return "Хэтчбек";
    }

    public override string GetFullInfo()
    {
        return base.GetFullInfo() +
               $"\n  Количество дверей: {NumberOfDoors}" +
               $"\n  Складывающиеся сиденья: {(HasFoldingSeats ? "Да" : "Нет")}";
    }

    public override object Clone()
    {
        return new Hatchback(Brand, Model, Year, Price, FuelConsumption,
            MaxSpeed, Color, Mileage, NumberOfDoors, HasFoldingSeats);
    }
}
