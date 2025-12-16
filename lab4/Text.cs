using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

[Serializable]
[XmlRoot("Text")]
public class Text
{
    [XmlElement("Sentence")]
    public List<Sentence> Sentences { get; set; }

    public Text()
    {
        Sentences = new List<Sentence>();
    }

    public void AddSentence(Sentence sentence)
    {
        Sentences.Add(sentence);
    }

    // Подсчет общего количества токенов в тексте
    public int TokenCount => Sentences.Sum(s => s.TokenCount);

    // Подсчет общего количества слов в тексте
    public int WordCount => Sentences.Sum(s => s.WordCount);

    // Вывести все предложения в порядке возрастания количества слов
    public List<Sentence> GetSentencesSortedByWordCount()
    {
        return Sentences.OrderBy(s => s.WordCount).ToList();
    }

    // Вывести все предложения в порядке возрастания длины предложения
    public List<Sentence> GetSentencesSortedByLength()
    {
        return Sentences.OrderBy(s => s.Length).ToList();
    }

    // Найти слова заданной длины в вопросительных предложениях (без повторов)
    public List<string> FindWordsInQuestions(int length)
    {
        return Sentences
            .Where(s => s.IsQuestion())
            .SelectMany(s => s.GetWordsOfLength(length))
            .Select(w => w.Value)
            .Distinct()
            .ToList();
    }

    // Удалить из текста все слова заданной длины, начинающиеся с согласной
    public void RemoveConsonantWordsOfLength(int length)
    {
        foreach (var sentence in Sentences)
        {
            sentence.RemoveWordsOfLength(length, consonantOnly: true);
        }
    }

    // Заменить слова заданной длины в указанном предложении
    public void ReplaceWordsInSentence(int sentenceIndex, int wordLength, string replacement)
    {
        if (sentenceIndex >= 0 && sentenceIndex < Sentences.Count)
        {
            Sentences[sentenceIndex].ReplaceWordsOfLength(wordLength, replacement);
        }
    }

    // Удалить стоп-слова из текста
    public void RemoveStopWords(string stopWordsFilePath)
    {
        if (!File.Exists(stopWordsFilePath))
        {
            throw new FileNotFoundException($"Файл со стоп-словами не найден: {stopWordsFilePath}");
        }

        var stopWords = new HashSet<string>(
            File.ReadAllLines(stopWordsFilePath, Encoding.UTF8)
                .Select(line => line.Trim().ToLower())
                .Where(line => !string.IsNullOrEmpty(line))
        );

        foreach (var sentence in Sentences)
        {
            sentence.RemoveStopWords(stopWords);
        }
    }

    public void ExportToXml(string filePath)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Text));
        using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            serializer.Serialize(writer, this);
        }
    }

    public static Text ImportFromXml(string filePath)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Text));
        using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8))
        {
            return (Text)serializer.Deserialize(reader);
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var sentence in Sentences)
        {
            sb.AppendLine(sentence.ToString());
        }
        return sb.ToString();
    }

    public string ToStringNumbered()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < Sentences.Count; i++)
        {
            sb.AppendLine($"{i + 1}. {Sentences[i]}");
        }
        return sb.ToString();
    }

    // Построить конкорданс для текста
    public Concordance BuildConcordance()
    {
        Concordance concordance = new Concordance();

        // Проходим по каждому предложению
        for (int i = 0; i < Sentences.Count; i++)
        {
            int lineNumber = i + 1;

            // Получаем все слова из предложения
            var words = Sentences[i].GetWords();

            // Добавляем каждое слово в конкорданс
            foreach (var word in words)
            {
                concordance.AddWord(word.Value, lineNumber);
            }
        }

        return concordance;
    }
}
