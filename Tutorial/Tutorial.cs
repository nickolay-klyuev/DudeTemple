using Godot;
using System;

public partial class Tutorial : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Visible;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void CloseAndDestroy()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
		QueueFree();
	}
}
