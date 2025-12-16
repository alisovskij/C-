using System;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

[Serializable]
public class Word : Token
{
    [XmlAttribute]
    public string Value { get; set; }

    public Word() { }

    public Word(string value)
    {
        Value = value;
    }

    public override int Length => Value?.Length ?? 0;

    public bool StartsWithConsonant()
    {
        if (string.IsNullOrEmpty(Value)) return false;

        char firstChar = char.ToLower(Value[0]);

        // Гласные для русского и английского
        string vowels = "аеёиоуыэюяaeiouy";

        return char.IsLetter(firstChar) && !vowels.Contains(firstChar);
    }
    public static string WordChecker(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return "word";

        // Паттерн для email
        string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        if (Regex.IsMatch(token, emailPattern))
            return "email";

        // Паттерн для даты (DD.MM.YYYY, DD-MM-YYYY, DD/MM/YYYY, YYYY-MM-DD, YYYY.MM.DD, YYYY/MM/DD)
        string datePattern = @"^(\d{1,2}[./-]\d{1,2}[./-]\d{2,4}|\d{4}[./-]\d{1,2}[./-]\d{1,2})$";
        if (Regex.IsMatch(token, datePattern))
            return "date";

        // Паттерн для телефонного номера (начинается с + и содержит цифры, пробелы, дефисы)
        string phonePattern = @"^\+\d{1,3}[\d\s-]{7,}$";
        if (Regex.IsMatch(token, phonePattern))
            return "phone";

        return "word";
    }

    public override string ToString()
    {
        return Value;
    }
}
