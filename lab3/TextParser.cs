using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public class TextParser
{
    public static string[] ParseTextToSentences(string filePath)
    {
        string text = File.ReadAllText(filePath, Encoding.UTF8);
        text = Regex.Replace(text, @"\s+", " ").Trim();

        string pattern = @".*?[.!?]";

        var matches = Regex.Matches(text, pattern);

        return matches
            .Cast<Match>()
            .Select(m => m.Value.Trim())
            .ToArray();
    }
}