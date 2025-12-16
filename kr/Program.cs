namespace kr;

using System.Xml.Serialization;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string fileDir = "file";
            SchoolBag bag = new SchoolBag();

            LoadNoteBooksFromFile(bag, Path.Combine(fileDir, "notebooks.txt"));
            LoadTextBooksFromFile(bag, Path.Combine(fileDir, "textbooks.txt"));

            bag.SortByWeight();

            if (bag.Weight > 3000)
            {
                Console.WriteLine("Weight exceeds 3kg, removing heaviest item");
                Item removed = bag.RemoveHeaviest();
                if (removed != null)
                {
                    Console.WriteLine($"Removed: {removed}");
                }
            }

            bool hasMath = bag.HasMathTextBook();
            Console.WriteLine($"Has Math textbook: {hasMath}");

            Console.WriteLine(bag);

            SaveToXml(bag, Path.Combine(fileDir, "result.xml"));
            Console.WriteLine($"\nResult saved to {Path.Combine(fileDir, "result.xml")}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void LoadNoteBooksFromFile(SchoolBag bag, string filename)
    {
        try
        {
            string[] lines = File.ReadAllLines(filename);
            foreach (string line in lines)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] parts = line.Split(',');
                    if (parts.Length != 2)
                    {
                        Console.WriteLine($"Invalid format in notebooks: {line}");
                        continue;
                    }

                    int weight = int.Parse(parts[0]);
                    int pages = int.Parse(parts[1]);

                    bag.Add(new NoteBook(weight, pages));
                }
                catch (FormatException)
                {
                    Console.WriteLine($"Invalid number format in notebooks: {line}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing notebook line: {ex.Message}");
                }
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"File not found: {filename}");
        }
    }

    static void LoadTextBooksFromFile(SchoolBag bag, string filename)
    {
        try
        {
            string[] lines = File.ReadAllLines(filename);
            foreach (string line in lines)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] parts = line.Split(',');
                    if (parts.Length != 2)
                    {
                        Console.WriteLine($"Invalid format in textbooks: {line}");
                        continue;
                    }

                    int weight = int.Parse(parts[0]);
                    string name = parts[1].Trim();

                    if (string.IsNullOrEmpty(name))
                    {
                        Console.WriteLine($"Empty name in textbooks: {line}");
                        continue;
                    }

                    bag.Add(new TextBook(weight, name));
                }
                catch (FormatException)
                {
                    Console.WriteLine($"Invalid number format in textbooks: {line}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing textbook line: {ex.Message}");
                }
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"File not found: {filename}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void SaveToXml(SchoolBag bag, string filename)
    {
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SchoolBag));
            using (StreamWriter writer = new StreamWriter(filename))
            {
                serializer.Serialize(writer, bag);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving XML: {ex.Message}");
        }
    }
}
