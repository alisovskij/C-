using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string path = "/home/artem/labs/3 sem/c #/lab4/input.txt";
        var testText = File.ReadAllText(path);

        Text text = TextParser.ParseText(testText);

        Concordance concordance = text.BuildConcordance();
        Console.WriteLine(concordance.ToString());
    }
}
