using System;
using System.Xml.Serialization;

public class Punctuation : Token
{   
    [XmlIgnore]
    public string Symbol { get; set; }

    public Punctuation() { }

    public Punctuation(string symbol)
    {
        Symbol = symbol;
    }

    public override int Length => Symbol?.Length ?? 0;

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
