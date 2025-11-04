using System;
using System.Xml.Serialization;

[Serializable]
[XmlInclude(typeof(Word))]
[XmlInclude(typeof(Punctuation))]
public abstract class Token
{
    [XmlIgnore]
    public abstract int Length { get; }

    public abstract override string ToString();
}
