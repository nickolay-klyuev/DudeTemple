using Godot;
using System;

public partial class TenPin : RigidBody3D
{
	[Export] private AudioStreamPlayer3D _hitPlayer;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_hitPlayer ??= GetNode<AudioStreamPlayer3D>("HitAudioPlayer");
		
#if DEBUG
		CheckHelper.Check(this, _hitPlayer);
#endif

		BodyEntered += PlayHitSoundIfPossible;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	void PlayHitSoundIfPossible(Node hitter)
	{
		if (hitter is RigidBody3D hitterBody) // hit ball or another pin
		{
			_hitPlayer.Play();
		}
		else if (LinearVelocity.Length() + AngularVelocity.Length() >= 10.0f) // hit floor
		{
			_hitPlayer.Play();
		}
	}
}
