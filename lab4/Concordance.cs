using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Concordance
{
    // Класс для хранения информации о слове
    public class WordInfo
    {
        public string Word { get; set; }
        public int Frequency { get; set; }
        public SortedSet<int> LineNumbers { get; set; }

        public WordInfo(string word)
        {
            Word = word;
            Frequency = 0;
            LineNumbers = new SortedSet<int>();
        }

        public void AddOccurrence(int lineNumber)
        {
            Frequency++;
            LineNumbers.Add(lineNumber);
        }

        public override string ToString()
        {
            var lineNumbersStr = string.Join(" ", LineNumbers);
            return $"{Word.PadRight(20, '.')}{Frequency}: {lineNumbersStr}";
        }
    }

    // Dictionary для хранения информации о каждом слове
    // Ключ - слово в нижнем регистре, значение - информация о слове
    private Dictionary<string, WordInfo> wordDictionary;

    public Concordance()
    {
        wordDictionary = new Dictionary<string, WordInfo>();
    }

    // Добавить слово с указанием номера строки
    public void AddWord(string word, int lineNumber)
    {
        if (string.IsNullOrWhiteSpace(word))
            return;

        string normalizedWord = word.ToLower();

        if (!wordDictionary.ContainsKey(normalizedWord))
        {
            wordDictionary[normalizedWord] = new WordInfo(normalizedWord);
        }

        wordDictionary[normalizedWord].AddOccurrence(lineNumber);
    }

    // Получить отсортированный список всех слов в алфавитном порядке
    public List<WordInfo> GetSortedWords()
    {
        return wordDictionary.Values
            .OrderBy(w => w.Word)
            .ToList();
    }

    // Получить информацию о конкретном слове
    public WordInfo GetWordInfo(string word)
    {
        string normalizedWord = word.ToLower();
        return wordDictionary.ContainsKey(normalizedWord) ? wordDictionary[normalizedWord] : null;
    }

    // Получить общее количество уникальных слов
    public int UniqueWordCount => wordDictionary.Count;

    // Получить общее количество слов (с учетом повторений)
    public int TotalWordCount => wordDictionary.Values.Sum(w => w.Frequency);

    // Вывести конкорданс в формате, как в задании
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Concordance:");
        sb.AppendLine(new string('-', 50));

        foreach (var wordInfo in GetSortedWords())
        {
            sb.AppendLine(wordInfo.ToString());
        }

        sb.AppendLine(new string('-', 50));
        sb.AppendLine($"Total unique words: {UniqueWordCount}");
        sb.AppendLine($"Total words: {TotalWordCount}");

        return sb.ToString();
    }
}
