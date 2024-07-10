using Godot;
using System;

public partial class BowlingBall : RigidBody3D, IGrabbable
{
	[Export]
	private MeshInstance3D _ballMesh;
	
	private Transform3D _initialTransform;
	private StandardMaterial3D _ballMaterial;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
#if DEBUG
		CheckHelper.Check(this, _ballMesh);
#endif
		
		_initialTransform = Transform;

		_ballMaterial = _ballMesh.GetActiveMaterial(0) as StandardMaterial3D;
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
    
    public void OverrideLocation(Vector3 loc)
    {
	    GlobalPosition = loc;
	    _initialTransform = Transform;
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
