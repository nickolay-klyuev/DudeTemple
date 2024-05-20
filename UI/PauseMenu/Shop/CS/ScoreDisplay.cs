using Godot;
using System;

public partial class ScoreDisplay : Label
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		UserDataHolder userData = GetNode<UserDataHolder>("/root/Temple/UserDataHolder");

#if DEBUG
		CheckHelper.Check(this, userData);
#endif
		
		userData.ScoreChanged += UpdateScoreLabel;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void UpdateScoreLabel(int newScore)
	{
		Text = $"Score: {newScore}";
	}
}
