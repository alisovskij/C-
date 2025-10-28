using System;
using System.Xml.Serialization;

[Serializable]
public class Word
{
    [XmlAttribute]
    public string Value { get; set; }

    public Word() { }

    public Word(string value)
    {
        Value = value;
    }

    public int Length => Value?.Length ?? 0;

    public bool StartsWithConsonant()
    {
        if (string.IsNullOrEmpty(Value)) return false;

        char firstChar = char.ToLower(Value[0]);

        // Гласные для русского и английского
        string vowels = "аеёиоуыэюяaeiouy";

        return char.IsLetter(firstChar) && !vowels.Contains(firstChar);
    }

    public override string ToString()
    {
        return Value;
    }
}
