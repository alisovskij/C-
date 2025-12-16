using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public class TextParser
{
    private static readonly string PunctuationMarks = ".!?,;:()\"'«»—-";

    // Паттерны для специальных токенов
    private static readonly string EmailPattern = @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}";
    private static readonly string DatePattern = @"\d{1,2}[./-]\d{1,2}[./-]\d{2,4}|\d{4}[./-]\d{1,2}[./-]\d{1,2}";
    private static readonly string PhonePattern = @"\+\d{1,3}[\d\s-]{7,}";

    public static Text ParseFile(string filePath)
    {
        string content = File.ReadAllText(filePath, Encoding.UTF8);
        return ParseText(content);
    }

    public static Text ParseText(string content)
    {
        Text text = new Text();

        // Нормализуем пробелы, но сохраняем переносы строк
        content = Regex.Replace(content, @"[ \t]+", " ");
        content = content.Trim();

        // Шаг 1: Находим все специальные токены (email, даты, телефоны) во всем тексте
        var specialTokenRanges = FindSpecialTokenRanges(content);

        // Шаг 2: Разбиваем на предложения, игнорируя точки в специальных токенах
        List<string> sentences = new List<string>();
        int currentStart = 0;

        for (int i = 0; i < content.Length; i++)
        {
            char ch = content[i];

            // Проверяем, не находимся ли мы внутри специального токена
            bool insideSpecialToken = specialTokenRanges.Any(r => i >= r.Start && i < r.End);

            if (!insideSpecialToken && (ch == '.' || ch == '!' || ch == '?'))
            {
                // Проверяем, не является ли это многоточием
                bool isEllipsis = false;
                if (ch == '.' && i + 1 < content.Length && content[i + 1] == '.')
                {
                    // Пропускаем многоточие
                    while (i < content.Length && content[i] == '.')
                    {
                        i++;
                    }
                    i--; // вернуться на последнюю точку многоточия
                    isEllipsis = true;
                }

                if (!isEllipsis)
                {
                    // Собираем все последовательные знаки конца предложения
                    int endIndex = i;
                    while (endIndex + 1 < content.Length &&
                           !specialTokenRanges.Any(r => endIndex + 1 >= r.Start && endIndex + 1 < r.End) &&
                           (content[endIndex + 1] == '.' || content[endIndex + 1] == '!' || content[endIndex + 1] == '?'))
                    {
                        endIndex++;
                    }

                    string sentenceText = content.Substring(currentStart, endIndex - currentStart + 1).Trim();
                    if (!string.IsNullOrWhiteSpace(sentenceText))
                    {
                        sentences.Add(sentenceText);
                    }

                    currentStart = endIndex + 1;
                    i = endIndex;
                }
            }
            else if (ch == '\n' || ch == '\r')
            {
                // Разбиваем также по переносам строк
                string sentenceText = content.Substring(currentStart, i - currentStart).Trim();
                if (!string.IsNullOrWhiteSpace(sentenceText))
                {
                    sentences.Add(sentenceText);
                }
                currentStart = i + 1;
            }
        }

        // Добавляем последнее предложение, если оно есть
        if (currentStart < content.Length)
        {
            string lastSentence = content.Substring(currentStart).Trim();
            if (!string.IsNullOrWhiteSpace(lastSentence))
            {
                sentences.Add(lastSentence);
            }
        }

        // Шаг 3: Парсим каждое предложение
        foreach (string sentenceText in sentences)
        {
            Sentence sentence = ParseSentence(sentenceText);
            if (sentence.Tokens.Count > 0)
            {
                text.AddSentence(sentence);
            }
        }

        return text;
    }

    private static List<(int Start, int End)> FindSpecialTokenRanges(string text)
    {
        var ranges = new List<(int Start, int End)>();

        foreach (Match match in Regex.Matches(text, EmailPattern))
            ranges.Add((match.Index, match.Index + match.Length));

        foreach (Match match in Regex.Matches(text, DatePattern))
            ranges.Add((match.Index, match.Index + match.Length));

        foreach (Match match in Regex.Matches(text, PhonePattern))
            ranges.Add((match.Index, match.Index + match.Length));

        return ranges;
    }

    private static List<(int Start, int End, string Value)> FindSpecialTokensWithValues(string text)
    {
        var tokens = new List<(int Start, int End, string Value)>();

        foreach (Match match in Regex.Matches(text, EmailPattern))
            tokens.Add((match.Index, match.Index + match.Length, match.Value));

        foreach (Match match in Regex.Matches(text, DatePattern))
            tokens.Add((match.Index, match.Index + match.Length, match.Value));

        foreach (Match match in Regex.Matches(text, PhonePattern))
            tokens.Add((match.Index, match.Index + match.Length, match.Value));

        return tokens;
    }

    private static Sentence ParseSentence(string sentenceText)
    {
        Sentence sentence = new Sentence();
        var specialTokenRanges = FindSpecialTokensWithValues(sentenceText);

        // Шаг 2: Обрабатываем текст посимвольно, пропуская специальные токены
        int currentPos = 0;

        while (currentPos < sentenceText.Length)
        {
            // Проверяем, находимся ли мы в начале специального токена
            var specialToken = specialTokenRanges.FirstOrDefault(r => r.Start == currentPos);

            if (specialToken != default)
            {
                // Добавляем специальный токен как Word
                sentence.AddToken(new Word(specialToken.Value));
                currentPos = specialToken.End;
            }
            else
            {
                // Проверяем, не находимся ли мы внутри специального токена
                bool insideSpecialToken = specialTokenRanges.Any(r => currentPos >= r.Start && currentPos < r.End);

                if (insideSpecialToken)
                {
                    // Пропускаем символ, так как он часть специального токена
                    currentPos++;
                }
                else
                {
                    char currentChar = sentenceText[currentPos];

                    // Проверяем, является ли символ знаком препинания
                    if (PunctuationMarks.Contains(currentChar))
                    {
                        sentence.AddToken(new Punctuation(currentChar.ToString()));
                        currentPos++;
                    }
                    // Проверяем, является ли символ буквой или частью слова
                    else if (char.IsLetter(currentChar) || char.GetUnicodeCategory(currentChar) == System.Globalization.UnicodeCategory.NonSpacingMark)
                    {
                        // Собираем слово (последовательность букв)
                        int wordStart = currentPos;
                        while (currentPos < sentenceText.Length &&
                               !specialTokenRanges.Any(r => currentPos >= r.Start && currentPos < r.End) &&
                               (char.IsLetter(sentenceText[currentPos]) ||
                                char.GetUnicodeCategory(sentenceText[currentPos]) == System.Globalization.UnicodeCategory.NonSpacingMark))
                        {
                            currentPos++;
                        }
                        string word = sentenceText.Substring(wordStart, currentPos - wordStart);
                        sentence.AddToken(new Word(word));
                    }
                    // Проверяем, является ли символ цифрой
                    else if (char.IsDigit(currentChar))
                    {
                        // Собираем число
                        int numberStart = currentPos;
                        while (currentPos < sentenceText.Length &&
                               !specialTokenRanges.Any(r => currentPos >= r.Start && currentPos < r.End) &&
                               char.IsDigit(sentenceText[currentPos]))
                        {
                            currentPos++;
                        }
                        string number = sentenceText.Substring(numberStart, currentPos - numberStart);
                        sentence.AddToken(new Word(number));
                    }
                    else
                    {
                        // Пропускаем пробелы и другие символы
                        currentPos++;
                    }
                }
            }
        }

        return sentence;
    }
}