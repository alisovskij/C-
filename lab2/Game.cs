using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public enum GameState
{
    Start,
    End
}

public class Game
{
    public static string InputFile { get; set; } = "ChaseData.txt";
    public static string OutputFile { get; set; } = "PursuitLog.txt";

    private readonly int _boardSize;
    private readonly Player _cat;
    private readonly Player _mouse;
    private GameState _currentState;
    private readonly List<string> _printLog = new List<string>();

    public Game(int boardSize)
    {
        _boardSize = boardSize;
        _cat = new Player("Cat");
        _mouse = new Player("Mouse");
        _currentState = GameState.Start;
    }

    public void Run()
    {
        var commands = File.ReadAllLines(InputFile).Skip(1);

        foreach (var line in commands)
        {
            if (_currentState == GameState.End) break;
            
            var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var command = parts[0][0];

            switch (command)
            {
                case 'C':
                case 'M':
                    int steps = int.Parse(parts[1]);
                    DoMoveCommand(command, steps);
                    break;
                case 'P':
                    DoPrintCommand();
                    break;
            }
        }
        
        if(_currentState != GameState.End)
        {
            _currentState = GameState.End;
        }

        WriteOutput();
    }

    private void DoMoveCommand(char player, int steps)
    {
        if (player == 'C')
        {
            _cat.Move(steps, _boardSize);
        }
        else
        {
            _mouse.Move(steps, _boardSize);
        }

        CheckForCatch();
    }
    
    private void CheckForCatch()
    {
        if (_cat.CurrentState == State.Playing && _mouse.CurrentState == State.Playing && _cat.Location == _mouse.Location)
        {
            _currentState = GameState.End;
            _cat.SetFinalState(State.Winner);
            _mouse.SetFinalState(State.Loser);
        }
    }

    private void DoPrintCommand()
    {
        string catPos = _cat.CurrentState == State.NotInGame ? "??" : _cat.Location.ToString();
        string mousePos = _mouse.CurrentState == State.NotInGame ? "??" : _mouse.Location.ToString();
        string distance = GetDistance();

        _printLog.Add($"{catPos,3} {mousePos,5} {distance,9}");    }

    private string GetDistance()
    {
        if (_cat.CurrentState != State.Playing || _mouse.CurrentState != State.Playing)
        {
            return "";
        }

        int directDist = Math.Abs(_cat.Location - _mouse.Location);
        int wrapDist = _boardSize - directDist;
        return Math.Min(directDist, wrapDist).ToString();
    }

    private void WriteOutput()
{
    using (StreamWriter writer = new StreamWriter(OutputFile))
    {
        writer.WriteLine("Cat and Mouse");
        writer.WriteLine();
        writer.WriteLine("Cat Mouse  Distance");
        writer.WriteLine("-------------------");

        foreach (var logEntry in _printLog)
        {
            writer.WriteLine(logEntry);
        }

        writer.WriteLine("-------------------");

        writer.WriteLine();
        writer.WriteLine();

        writer.WriteLine($"{"Distance traveled:",-21}{"Mouse",-9}{"Cat"}");
        writer.WriteLine($"{string.Empty,21}{_mouse.DistanceTraveled,5}    {_cat.DistanceTraveled,3}");
        writer.WriteLine();

        if (_mouse.CurrentState == State.Loser)
        {
            writer.WriteLine($"Mouse caught at: {_mouse.Location,2}");
        }
        else
        {
            writer.WriteLine("Mouse evaded Cat");
        }
    }
}
}