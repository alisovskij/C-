using System;
using System.Xml.Serialization;

[Serializable]
public class Punctuation
{
    [XmlAttribute]
    public string Symbol { get; set; }

    public Punctuation() { }

    public Punctuation(string symbol)
    {
        Symbol = symbol;
    }

    public bool IsEndOfSentence()
    {
        return Symbol == "." || Symbol == "!" || Symbol == "?";
    }

    public bool IsQuestion()
    {
        return Symbol == "?";
    }

    public override string ToString()
    {
        return Symbol;
    }
}
