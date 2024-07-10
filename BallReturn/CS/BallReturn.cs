using Godot;
using System;

public partial class BallReturn : Node3D
{
	[Export] private double _spawnDelay = 3.0;
	[Export] private int _ballsAmount = 6;
	[Export] private PackedScene _ballTemplate;
	
	private bool _bFlipFlop = true;
	private Vector3 _spawnPositionFlip = Vector3.Zero;
	private Vector3 _spawnPositionFlop = Vector3.Zero;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
#if DEBUG
		CheckHelper.CheckScene(_ballTemplate, this);
#endif

		_spawnPositionFlip = GetNode<Node3D>("SpawnPoint_1").GlobalPosition;
		_spawnPositionFlop = GetNode<Node3D>("SpawnPoint_2").GlobalPosition;
		
		SceneTreeTimer timer = GetTree().CreateTimer(_spawnDelay);
		timer.Timeout += () =>
		{
			SpawnBallRecursion();
		};
	}

	void SpawnBallRecursion(int index = 0)
	{
		BowlingBall ball = _ballTemplate.Instantiate<BowlingBall>();
		GetTree().Root.GetNode("Temple").AddChild(ball);

		ball.OverrideLocation(_bFlipFlop ? _spawnPositionFlip : _spawnPositionFlop);
		_bFlipFlop = !_bFlipFlop;

		if (++index < _ballsAmount)
		{
			SceneTreeTimer timer = GetTree().CreateTimer(_spawnDelay);
			timer.Timeout += () =>
			{
				SpawnBallRecursion(index);
			};
		}
	}
}
