using Godot;
using Godot.Collections;
using System;

public partial class PinsGroupHandler : Node3D
{
	[Signal]
	public delegate void ScoreCalculatedEventHandler(int score);

	[Export]
	private Node3D _pinsHolder;

	[Export]
	private Timer _scoreCountdownTimer;

	private Array<Transform3D> _initPinsTransforms = new Array<Transform3D>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		#if DEBUG
		CheckHelper.CheckNodes(new Array<Node>(){ _pinsHolder, _scoreCountdownTimer }, this);
		#endif

		foreach (Node3D pin in _pinsHolder.GetChildren())
		{
			_initPinsTransforms.Add(pin.Transform);
		}

		_scoreCountdownTimer.Timeout += CountScore;
		_scoreCountdownTimer.Timeout += ResetPins;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void CountScore()
	{
		// TODO: Count score like that can give mistakes but rare. Maybe change it in the future.
		int hitPins = 0;
		foreach (Node3D pin in _pinsHolder.GetChildren())
		{
			float angleRad = 0.1745f; // 10 degrees
			
			if (Mathf.Abs(pin.Rotation.X) > angleRad || Mathf.Abs(pin.Rotation.Z) > angleRad)
			{
				hitPins++;
			}
		}

		EmitSignal(SignalName.ScoreCalculated, hitPins);
	}

	private void ResetPins()
	{
		int index = 0;
		foreach (RigidBody3D pin in _pinsHolder.GetChildren())
		{
			pin.Freeze = true;
			pin.Transform = _initPinsTransforms[index];
			index++;
		}
	}

	public void OnBallTriggerAreaBodyEntered(Node3D body)
	{
		_scoreCountdownTimer.Start();

		foreach (RigidBody3D pin in _pinsHolder.GetChildren())
		{
			pin.Freeze = false;
		}
	}
}
