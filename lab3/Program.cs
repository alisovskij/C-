using System;
using System.IO;
using System.Linq;
using System.Text;

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("=== Лабораторная работа 3: Токенизация текста ===\n");

        try
        {
            // Парсинг текста
            string filename = "input.txt";
            Console.WriteLine($"1. Читаем и парсим файл '{filename}'...\n");

            Text text = TextParser.ParseFile(filename);

            Console.WriteLine($"Исходный текст ({text.Sentences.Count} предложений):");
            Console.WriteLine(text.ToStringNumbered());
            Console.WriteLine();

            // Задание 1: Сортировка по количеству слов
            Console.WriteLine("2. Предложения в порядке возрастания количества слов:");
            var sortedByWords = text.GetSentencesSortedByWordCount();
            for (int i = 0; i < sortedByWords.Count; i++)
            {
                Console.WriteLine($"   {i + 1}. [{sortedByWords[i].WordCount} слов] {sortedByWords[i]}");
            }
            Console.WriteLine();

            // Задание 2: Сортировка по длине
            Console.WriteLine("3. Предложения в порядке возрастания длины:");
            var sortedByLength = text.GetSentencesSortedByLength();
            for (int i = 0; i < sortedByLength.Count; i++)
            {
                Console.WriteLine($"   {i + 1}. [{sortedByLength[i].Length} символов] {sortedByLength[i]}");
            }
            Console.WriteLine();

            // Задание 3: Слова в вопросительных предложениях
            int wordLength = 4;
            Console.WriteLine($"4. Слова длиной {wordLength} в вопросительных предложениях:");
            var wordsInQuestions = text.FindWordsInQuestions(wordLength);
            if (wordsInQuestions.Any())
            {
                foreach (var word in wordsInQuestions)
                {
                    Console.WriteLine($"   - {word}");
                }
            }
            else
            {
                Console.WriteLine("   (не найдено)");
            }
            Console.WriteLine();

            // Задание 4: Удаление слов, начинающихся с согласной
            Text textCopy = TextParser.ParseFile(filename); // Создаем копию
            int lengthToRemove = 3;
            Console.WriteLine($"5. Удаляем слова длиной {lengthToRemove}, начинающиеся с согласной...");
            textCopy.RemoveConsonantWordsOfLength(lengthToRemove);
            Console.WriteLine("   Результат:");
            Console.WriteLine(textCopy.ToStringNumbered());
            Console.WriteLine();

            // Задание 5: Замена слов в предложении
            Text textCopy2 = TextParser.ParseFile(filename);
            int sentenceIndex = 0;
            int lengthToReplace = 5;
            string replacement = "XXXXX";
            Console.WriteLine($"6. Заменяем слова длиной {lengthToReplace} в предложении {sentenceIndex + 1} на '{replacement}':");
            textCopy2.ReplaceWordsInSentence(sentenceIndex, lengthToReplace, replacement);
            Console.WriteLine($"   {textCopy2.Sentences[sentenceIndex]}");
            Console.WriteLine();

            // Задание 6: Удаление стоп-слов
            string stopWordsFile = "stopwords_ru.txt";
            if (File.Exists(stopWordsFile))
            {
                Text textCopy3 = TextParser.ParseFile(filename);
                Console.WriteLine("7. Удаляем стоп-слова из текста...");
                textCopy3.RemoveStopWords(stopWordsFile);
                Console.WriteLine("   Результат:");
                Console.WriteLine(textCopy3.ToStringNumbered());
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine($"7. Файл стоп-слов '{stopWordsFile}' не найден, пропускаем...\n");
            }

            // Задание 7: Экспорт в XML
            string xmlFile = "output.xml";
            Console.WriteLine($"8. Экспортируем текст в XML файл '{xmlFile}'...");
            text.ExportToXml(xmlFile);
            Console.WriteLine("   Успешно экспортировано!");
            Console.WriteLine();

            Console.WriteLine("=== Все задания выполнены! ===");
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"Ошибка: Файл не найден - {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }
}