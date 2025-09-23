using System.Text;

namespace Lab1;

public static class CommandHandler
{
    public static void ExecuteCommand(StreamWriter output, string commandLine, List<GeneticData> allData, ref int commandCounter)
    {
        var cleanLine = commandLine.Trim();
        var parts = cleanLine.Split('\t');
        var command = parts[0].Trim().ToLower();

        output.WriteLine();
        output.WriteLine(new string('-', 40));
        output.WriteLine($"{commandCounter:D3}\t{cleanLine}");

        switch (command)
        {
            case "search" when parts.Length > 1:
                PerformSearch(output, parts[1], allData);
                break;
            case "diff" when parts.Length > 2:
                CalculateDifference(output, parts[1], parts[2], allData);
                break;
            case "mode" when parts.Length > 1:
                FindMostCommonAminoAcid(output, parts[1], allData);
                break;
        }

        commandCounter++;
    }

    private static void PerformSearch(StreamWriter output, string rleSequence, List<GeneticData> allData)
    {
        var sequence = DecodeRLE(rleSequence);
        var matches = allData
            .Where(data => data.amino_acids.Contains(sequence))
            .Select(data => $"{data.organism}\t{data.protein}")
            .ToList();

        if (matches.Any())
        {
            matches.ForEach(m => output.WriteLine(m));
        }
        else
        {
            output.WriteLine("NOT FOUND");
        }
    }

    private static void CalculateDifference(StreamWriter output, string firstProteinName, string secondProteinName, List<GeneticData> allData)
    {
        var data1 = allData.FirstOrDefault(d => d.protein == firstProteinName);
        var data2 = allData.FirstOrDefault(d => d.protein == secondProteinName);
        
        bool isData1Missing = string.IsNullOrEmpty(data1.protein);
        bool isData2Missing = string.IsNullOrEmpty(data2.protein);

        if (isData1Missing || isData2Missing)
        {
            var missing = new List<string>();
            if (isData1Missing) missing.Add(firstProteinName);
            if (isData2Missing) missing.Add(secondProteinName);
            output.WriteLine($"amino-acids difference: MISSING: {string.Join(" ", missing)}");
            return;
        }

        int diff = GetSequenceDifference(data1.amino_acids, data2.amino_acids);
        output.WriteLine($"amino-acids difference: {diff}");
    }

    private static void FindMostCommonAminoAcid(StreamWriter output, string proteinName, List<GeneticData> allData)
    {
        var dataItem = allData.FirstOrDefault(d => d.protein == proteinName);

        if (string.IsNullOrEmpty(dataItem.protein))
        {
            output.WriteLine($"amino-acid occurs: MISSING: {proteinName}");
            return;
        }

        if (string.IsNullOrEmpty(dataItem.amino_acids))
        {
            output.WriteLine($"amino-acid occurs: ? 0");
            return;
        }

        var mostCommon = dataItem.amino_acids
            .GroupBy(c => c)
            .Select(group => new { AminoAcid = group.Key, Count = group.Count() })
            .OrderByDescending(x => x.Count)
            .ThenBy(x => x.AminoAcid)
            .First();

        output.WriteLine($"amino-acid occurs: {mostCommon.AminoAcid} {mostCommon.Count}");
    }

    public static List<GeneticData> LoadGeneticData(string path)
    {
        return File.ReadLines(path)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(line => line.Split('\t'))
            .Where(parts => parts.Length >= 3)
            .Select(parts => new GeneticData
            {
                protein = parts[0],
                organism = parts[1],
                amino_acids = DecodeRLE(parts[2])
            })
            .ToList();
    }
    
    
    private static string DecodeRLE(string compressed)
    {
        if (string.IsNullOrEmpty(compressed)) return "";
        var result = new StringBuilder();
        for (int i = 0; i < compressed.Length; i++)
        {
            if (char.IsDigit(compressed[i]) && i + 1 < compressed.Length)
            {
                int count = compressed[i] - '0';
                result.Append(compressed[i + 1], count);
                i++;
            }
            else
            {
                result.Append(compressed[i]);
            }
        }
        return result.ToString();
    }

    private static int GetSequenceDifference(string seq1, string seq2)
    {
        int diffCount = 0;
        int longest = Math.Max(seq1.Length, seq2.Length);
        for (int i = 0; i < longest; i++)
        {
            char char1 = i < seq1.Length ? seq1[i] : default;
            char char2 = i < seq2.Length ? seq2[i] : default;
            if (char1 != char2) diffCount++;
        }
        return diffCount;
    }
}