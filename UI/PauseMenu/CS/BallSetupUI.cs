using Godot;
using System;

public partial class BallSetupUI : Control
{
	//private BowlingBallSetup _ballSetup;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//_ballSetup = FindHelper.FindBowlingBallSetup(this);

		#if DEBUG
		//CheckHelper.Check(this, _ballSetup);
		#endif
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// Execute by signal from ColorPickerButton
	public void OnBallColorChanged(Color newColor)
	{
		//_ballSetup.BallColor = newColor;
	}

	public void OnGlowingStatusChanged(bool bIsEnabled)
	{
		//_ballSetup.IsGlowing = bIsEnabled;
	}

	public void OnGlowingStrengthChanged(float newStrength)
	{
		//_ballSetup.GlowingStrength = newStrength;
	}
}
