namespace kr;

public class TextBook : Item
{
    public string Name { get; set; }

    public TextBook()
    {
    }

    public TextBook(int weight, string name) : base(weight)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be empty");
        }
        Name = name;
    }

    public override string ToString()
    {
        return $"TextBook: {Name}, {base.ToString()}";
    }
}
