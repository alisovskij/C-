using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

public class TextParser
{
    // Знаки препинания
    private static readonly string PunctuationMarks = ".!?,;:()\"'«»—-";

    // Читаем текст и разбиваем на токены с помощью regex
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

        // Разбиваем на предложения по знакам окончания предложения
        var sentencePattern = @"[^.!?]+[.!?]+";
        var sentenceMatches = Regex.Matches(content, sentencePattern);

        foreach (Match sentenceMatch in sentenceMatches)
        {
            string sentenceText = sentenceMatch.Value.Trim();
            if (string.IsNullOrWhiteSpace(sentenceText)) continue;

            Sentence sentence = ParseSentence(sentenceText);
            if (sentence.Tokens.Count > 0)
            {
                text.AddSentence(sentence);
            }
        }

        return text;
    }

    private static Sentence ParseSentence(string sentenceText)
    {
        Sentence sentence = new Sentence();

        // Разбиваем предложение на токены (слова и знаки препинания)
        // Паттерн: последовательность букв (слово) или знак препинания
        string pattern = @"[\p{L}\p{M}]+|[" + Regex.Escape(PunctuationMarks) + "]";
        var matches = Regex.Matches(sentenceText, pattern);

        foreach (Match match in matches)
        {
            string token = match.Value;

            if (IsPunctuation(token))
            {
                sentence.AddToken(new Punctuation(token));
            }
            else if (!string.IsNullOrWhiteSpace(token))
            {
                sentence.AddToken(new Word(token));
            }
        }

        return sentence;
    }

    private static bool IsPunctuation(string token)
    {
        return token.Length == 1 && PunctuationMarks.Contains(token);
    }

    // Метод для обратной совместимости
    public static string[] ParseTextToSentences(string filePath)
    {
        Text text = ParseFile(filePath);
        return text.Sentences.Select(s => s.ToString()).ToArray();
    }
}