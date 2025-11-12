using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

[Serializable]
public class Sentence
{
    [XmlIgnore]
    public List<Token> Tokens { get; set; }

    [XmlElement("Word")]
    public List<Word> Words
    {
        get => Tokens.OfType<Word>().ToList();
        set => Tokens = value?.Cast<Token>().ToList() ?? new List<Token>();
    }

    public Sentence()
    {
        Tokens = new List<Token>();
    }

    public void AddToken(Token token)
    {
        if (token != null)
        {
            Tokens.Add(token);
        }
    }

    public int WordCount => Tokens.OfType<Word>().Count();

    public int TokenCount => Tokens.Count;

    public int Length
    {
        get
        {
            int length = 0;
            foreach (var token in Tokens)
            {
                length += token.Length;
            }
            // Добавляем пробелы между словами
            int spaces = Math.Max(0, Tokens.Count - 1);
            return length + spaces;
        }
    }

    public bool IsQuestion()
    {
        var lastPunct = Tokens.OfType<Punctuation>().LastOrDefault();
        return lastPunct?.IsQuestion() ?? false;
    }

    public List<Word> GetWords()
    {
        return Tokens.OfType<Word>().ToList();
    }

    public List<Word> GetWordsOfLength(int length)
    {
        return Tokens.OfType<Word>()
            .Where(w => w.Length == length)
            .ToList();
    }

    public void RemoveWordsOfLength(int length, bool consonantOnly = false)
    {
        Tokens = Tokens.Where(token =>
        {
            if (token is Word word)
            {
                if (word.Length != length) return true;
                if (consonantOnly && !word.StartsWithConsonant()) return true;
                return false;
            }
            return true;
        }).ToList();
    }

    public void ReplaceWordsOfLength(int length, string replacement)
    {
        for (int i = 0; i < Tokens.Count; i++)
        {
            if (Tokens[i] is Word word && word.Length == length)
            {
                Tokens[i] = new Word(replacement);
            }
        }
    }

    public void RemoveStopWords(HashSet<string> stopWords)
    {
        Tokens = Tokens.Where(token =>
        {
            if (token is Word word)
            {
                return !stopWords.Contains(word.Value.ToLower());
            }
            return true;
        }).ToList();
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < Tokens.Count; i++)
        {
            if (i > 0 && Tokens[i] is Word)
            {
                sb.Append(' ');
            }
            sb.Append(Tokens[i].ToString());
        }
        return sb.ToString();
    }
}
