using Godot;
using System;

public partial class ScoreDataHolder : Node
{
    [Signal]
    public delegate void ScoreChangedEventHandler(int newScore);

    public int GlobalScore { private set; get; } = 0;

    public void AddScore(int amount)
    {
        GlobalScore += amount;

        EmitSignal(SignalName.ScoreChanged, GlobalScore);
    }
}
