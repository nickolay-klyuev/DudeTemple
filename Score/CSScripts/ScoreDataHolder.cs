using Godot;
using System;

public partial class ScoreDataHolder : Node
{
    [Signal]
    public delegate void ScoreChangedEventHandler(int newScore);

    public int GlobalScore { private set; get; } = 0;

    public override void _Ready()
    {
        GlobalScore = SaveDataHelperStatic.LoadScore();
        EmitSignal(SignalName.ScoreChanged, GlobalScore);
    }

    public void AddScore(int amount)
    {
        GlobalScore += amount;

        SaveDataHelperStatic.SaveScoreAsync(GlobalScore);
        EmitSignal(SignalName.ScoreChanged, GlobalScore);
    }
}
