using Godot;
using System;

public partial class BowlingBallHandler : RigidBody3D, IGrabbable
{
	[Export]
	private MeshInstance3D _ballMesh;

	private BowlingBallSetup _bowlingBallSetup;
	private Transform3D _initialTransform;
	private StandardMaterial3D _ballMaterial;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_bowlingBallSetup = FindHelper.FindBowlingBallSetup(this);

		#if DEBUG
		CheckHelper.Check(this, _ballMesh, _bowlingBallSetup);
		#endif

		_bowlingBallSetup.BallSetupIsDirty += OnBallSetupIsDirty;

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

	// #################### Visuals ################################
	private void OnBallSetupIsDirty(int property)
	{
		switch (property)
		{
			case 0:
				UpdateColor(_bowlingBallSetup.BallColor);
				return;
			case 1:
				_ballMaterial.EmissionEnabled = _bowlingBallSetup.IsGlowing;
				return;
			case 2:
				UpdateEmittionStrength(_bowlingBallSetup.GlowingStrength);
				return;
		}
	}

	private void UpdateColor(Color newColor)
	{
		_ballMaterial.AlbedoColor = newColor;
		_ballMaterial.Emission = newColor;
	}

	private void UpdateEmittionStrength(float newStrength)
	{
		if (newStrength < 0.0f)
		{
			_ballMaterial.EmissionEnergyMultiplier = 0.0f;
		}
		else
		{
			_ballMaterial.EmissionEnergyMultiplier = newStrength;
		}
	}
}
