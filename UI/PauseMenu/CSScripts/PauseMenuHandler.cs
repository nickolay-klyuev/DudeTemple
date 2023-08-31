using Godot;
using System;

public partial class PauseMenuHandler : Control, IMenuInteract
{
	[Signal]
	public delegate void CloseMenuRequestEventHandler();

	[Signal]
	public delegate void OpenShopRequestEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Open()
	{
		Visible = true;
	}

	public void Close()
	{
		Visible = false;
	}

	public void OnResumeButtonPressed()
	{
		EmitSignal(SignalName.CloseMenuRequest);
	}

	public void OnShopButtonPressed()
	{
		EmitSignal(SignalName.OpenShopRequest);
	}

	public void OnExitButtonPressed()
	{
		GetTree().Quit();
	}
}
