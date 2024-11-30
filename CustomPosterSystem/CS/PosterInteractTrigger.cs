using Godot;
using System;

public partial class PosterInteractTrigger : Area3D, IInteractable
{
	[Signal]
	public delegate void PosterInteractTriggeredEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Interact()
	{
		EmitSignal(SignalName.PosterInteractTriggered);
	}

	public bool CanInteract()
	{
		return true;
	}

	public string GetInteractLabel()
	{
		return "set new poster image";
	}
}
