using Godot;
using System;

public partial class BowlingBallHandler : RigidBody3D, IGrabbable
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
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
