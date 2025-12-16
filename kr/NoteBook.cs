namespace kr;

public class NoteBook : Item
{
    public int Pages { get; set; }

    public NoteBook()
    {
    }

    public NoteBook(int weight, int pages) : base(weight)
    {
        if (pages > 0)
        {
            Pages = pages;
        }
    }

    public override string ToString()
    {
        return $"NoteBook: {Pages} pages, {base.ToString()}";
    }
}
