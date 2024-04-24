using Godot;
using System;

public partial class PercentageDisplay : Label
{
	public void Update(double value)
	{
		Text = $"{value:P0}";
	}

	public void Update(float value)
	{
		Text = $"{value:P0}";
	}
}
