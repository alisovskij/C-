using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        Game.InputFile = "2.ChaseData.txt";
        Game.OutputFile = "PursuitLog.txt";

        int boardSize = int.Parse(File.ReadLines(Game.InputFile).First());

        Game game = new Game(boardSize);
        
        game.Run();
    }
}