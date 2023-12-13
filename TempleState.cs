using Godot;
using System;

public partial class TempleState : Node3D
{
	[Export]
	private UserDataHolder _userDataHolder;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		#if DEBUG
		CheckHelper.Check(this, _userDataHolder);
		#endif

		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		// For testing
		if (Input.IsActionJustPressed("AddScore"))
		{
			_userDataHolder.AddScore(100);
		}
	}

	public Node3D GetDude()
	{
		return GetNode<Node3D>("Dude");
	}
}
