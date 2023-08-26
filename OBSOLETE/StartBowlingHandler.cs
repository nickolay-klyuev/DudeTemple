using Godot;
using System;

public partial class StartBowlingHandler : Area3D, IInteractable
{
	[Signal]
	public delegate void StartBowlingTriggeredEventHandler(int lineIndex);

	[Export]
	private int _lineIndex = 0;

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
		EmitSignal(SignalName.StartBowlingTriggered, _lineIndex);
	}
}
