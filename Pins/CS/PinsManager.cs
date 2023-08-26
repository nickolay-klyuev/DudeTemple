using Godot;
using Godot.Collections;
using System;

public partial class PinsManager : Node3D
{
	[Export]
	private PackedScene _pinsScene;

	[Export]
	private UserDataHolder _userData;

	private Array<Vector3> _pinsSpawnPoints = new Array<Vector3>();
	private Array<Node3D> _pinsInstances = new Array<Node3D>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		#if DEBUG
		CheckHelper.CheckScene(_pinsScene, this);
		CheckHelper.CheckNode(_userData, this);
		#endif

		foreach (Node3D point in GetChildren())
		{
			_pinsSpawnPoints.Add(point.GlobalPosition);
		}

		SpawnPins();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void SpawnPins()
	{
		foreach (Vector3 point in _pinsSpawnPoints)
		{
			Node3D pins = _pinsScene.Instantiate<Node3D>();

			// Add as child to Temple node
			AddChild(pins);

			PinsGroupHandler ballTriggerArea = pins as PinsGroupHandler;
			ballTriggerArea.ScoreCalculated += OnScoreCalculated;

			pins.GlobalPosition = point;
			_pinsInstances.Add(pins);
		}
	}

	private void OnScoreCalculated(int score)
	{
		_userData.AddScore(score);
	}
}
