using Godot;
using System;

public partial class BowlingBallController : RigidBody3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _UnhandledInput(InputEvent @event)
    {
        if (Input.IsActionJustPressed("MainAction"))
		{
			if (Freeze)
			{
				Freeze = false;
			}
			
			ApplyImpulse(new Vector3(50.0f, 0.0f, 0.0f));
		}
    }
}
