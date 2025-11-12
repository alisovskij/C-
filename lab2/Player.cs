using System;

public enum State
{
    Playing,
    Winner,
    Loser,
    NotInGame,
    MouseEscaped
}

public class Player
{
    public string Name { get; }
    public int Location { get; private set; }
    public State CurrentState { get; private set; }
    public int DistanceTraveled { get; private set; }

    public Player(string name)
    {
        Name = name;
        Location = -1;
        CurrentState = State.NotInGame;
        DistanceTraveled = 0;
    }

    public void Move(int steps, int boardSize, bool is_mouse)
    {
        if (CurrentState == State.NotInGame)
        {
            Location = steps;
            CurrentState = State.Playing;
        }
        else
        {
            int newLocation = ((Location - 1 + steps) % boardSize + boardSize) % boardSize + 1;
            Location = newLocation;
            DistanceTraveled += Math.Abs(steps);
        }

        if (is_mouse && Location == 1)
        {
            CurrentState = State.MouseEscaped;
        }

    }

    public void SetFinalState(State finalState)
    {
        CurrentState = finalState;
    }
}