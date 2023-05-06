using Godot;
using System;

public partial class PauseMenuHandler : Control
{
	private bool _bOpened;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetOpened(false);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _UnhandledInput(InputEvent @event)
    {
		// Open/Close pause menu by action
        if (Input.IsActionJustPressed("Return"))
		{
			SetOpened(!_bOpened);
		}
    }

	private void SetOpened(bool bOpened)
	{
		Visible = bOpened;
		_bOpened = bOpened;
		GetTree().Paused = bOpened;

		// Show/Hide cursor for menu
		if (bOpened)
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}
		else
		{
			Input.MouseMode = Input.MouseModeEnum.Captured;
		}
	}

	public void OnResumeButtonPressed()
	{
		SetOpened(false);
	}

	public void OnExitButtonPressed()
	{
		GetTree().Quit();
	}
}
