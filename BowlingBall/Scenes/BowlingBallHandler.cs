using Godot;
using System;

public partial class BowlingBallHandler : RigidBody3D, IGrabbable
{
	private Transform3D _initialTransform;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_initialTransform = Transform;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _PhysicsProcess(double delta)
    {
		// If the ball is falling in the abyss then reset it to original position. 
        if (GlobalPosition.Y <= -10.0f)
		{
			ResetBall();
		}
    }

	private void ResetBall()
	{
		Sleeping = true;
		Transform = _initialTransform;
	}

	// ##### IGrabbable interface #####
	public bool IsCanBeGrabbed()
	{
		return true;
	}

	public void Grab()
	{
		Freeze = true;
	}

	public void Release()
	{
		Freeze = false;
	}

	public void ThrowToDirection(Vector3 direction, float force)
	{
		Release();
		ApplyImpulse(direction.Normalized() * force);
	}
}
