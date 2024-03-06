using Godot;
using System;

public partial class ThrowForce : ProgressBar
{
	[Export]
	private Color _beginColor;

	[Export]
	private Color _endColor;

	private StyleBoxFlat _fillStyleBoxOverride = new StyleBoxFlat();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AddThemeStyleboxOverride("fill", _fillStyleBoxOverride);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SetForcePercentage(float percent)
	{
		Value = percent * 100.0f;

		float newR = _beginColor.R + (_endColor.R - _beginColor.R) * percent;
		float newG = _beginColor.G + (_endColor.G - _beginColor.G) * percent;
		float newB = _beginColor.B + (_endColor.B - _beginColor.B) * percent;

		Color newFillColor = new Color(newR, newG, newB);

		_fillStyleBoxOverride.Set("bg_color", newFillColor);
	}
}
