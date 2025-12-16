namespace kr;

using System.Xml.Serialization;

[XmlInclude(typeof(NoteBook))]
[XmlInclude(typeof(TextBook))]
public abstract class Item : IComparable<Item>
{
    private int weightInGrams;

    public int Weight
    {
        get { return weightInGrams; }
        set { weightInGrams = value; }
    }

    protected Item()
    {
    }

    protected Item(int weight)
    {
        if (weight > 0)
        {
            weightInGrams = weight;
        }
    }

    public int CompareTo(Item other)
    {
        if (other == null) return 1;
        return weightInGrams.CompareTo(other.weightInGrams);
    }


    public override string ToString()
    {
        return $"Weight: {weightInGrams}g";
    }
}
