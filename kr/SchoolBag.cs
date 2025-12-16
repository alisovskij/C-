namespace kr;

public class SchoolBag
{
    public List<Item> Items { get; set; }

    public int Weight
    {
        get
        {
            int total = 0;
            foreach (var item in Items)
            {
                total += item.Weight;
            }
            return total;
        }
    }

    public SchoolBag()
    {
        Items = new List<Item>();
    }

    public void Add(Item item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }
        Items.Add(item);
    }

    public void SortByWeight()
    {
        Items.Sort();
    }

    public Item RemoveHeaviest()
    {
        if (Items.Count == 0) return null;

        Item heaviest = Items[0];
        foreach (var item in Items)
        {
            if (item.Weight > heaviest.Weight)
            {
                heaviest = item;
            }
        }
        Items.Remove(heaviest);
        return heaviest;
    }

    public bool HasMathTextBook()
    {
        foreach (var item in Items)
        {
            if (item is TextBook textBook && textBook.Name == "Math")
            {
                return true;
            }
        }
        return false;
    }

    public override string ToString()
    {
        string result = $"SchoolBag (Total weight: {Weight}g):\n";
        foreach (var item in Items)
        {
            result += $"  {item}\n";
        }
        return result;
    }
}
