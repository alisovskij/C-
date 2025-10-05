using System;
using System.IO;
using System.Linq;
using System.Text;

public class Program
{
    public static void Main(string[] args)
    {
        string filename = "input.txt";
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine($"Читаем файл '{filename}' и разбиваем на предложения...\n");

        try
        {
            string[] listOfSentences = TextParser.ParseTextToSentences(filename);

            if (listOfSentences.Any()) 
            {
                Console.WriteLine("Результат разбиения:");
                for (int i = 0; i < listOfSentences.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {listOfSentences[i]}");
                }
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"Ошибка: Файл не найден по пути '{filename}'. Убедитесь, что он находится в папке с программой.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла непредвиденная ошибка: {ex.Message}");
        }
    }
}