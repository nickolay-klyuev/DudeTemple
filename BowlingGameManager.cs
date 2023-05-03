using Godot;
using Godot.Collections;
using System;

public partial class BowlingGameManager : Node3D
{
	[ExportGroup("Resources")]
	[Export]
	PackedScene BowlingBallScene;

	[Export]
	Node3D SpawnBallLocation;

	[Export]
	PackedScene BowlingPinsScene;

	[Export]
	Node3D SpawnPinsLocation;

	[ExportSubgroup("UI")]
	[Export]
	VBoxContainer PowerMeterUI;

	[Export]
	TextureRect EndLineView;

	[ExportGroup("Settings")]
	[Export]
	private float _moveBallSensitivity = 1.0f;

	[Export]
	private float _changeThrowPowerTime = 0.1f;

	[Export]
	private float _baseThrowPower = 10.0f;


	private RigidBody3D _bowlingBall;
	private Node3D _bowlingPins;

	private bool _bIsBowlingLaneClean = true;
	private Vector3 _cachedBallPosition;

	private Array<ColorRect> _powerMeterParts = new Array<ColorRect>(); // Populate in _Ready()

	private int _currentThrowPower = 0;
	private int _maxThrowPower = 0; // Init in _Ready()
	private double _cachedPowerDeltaTime = 0.0f;

	private bool _bIsBallLanched = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetBowlingUIVisibility(false);

		const string powerMeterScaleName = "PowerMeterScale";

		VBoxContainer powerMeterScale = PowerMeterUI.GetNode<VBoxContainer>(powerMeterScaleName);
		if (powerMeterScale != null)
		{
			foreach (ColorRect scalePart in powerMeterScale.GetChildren())
			{
				_powerMeterParts.Add(scalePart);
				_powerMeterParts.Reverse();
			}
		}
		else
		{
			GD.PrintErr($"{Name}: Couldn't find {powerMeterScaleName} node! Please check if it exists on Scene!");
		}

		_maxThrowPower = _powerMeterParts.Count;
		UpdatePowerMeterScale();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		PowerChangeProcess(delta);
	}

	public override void _UnhandledInput(InputEvent @event)
    {
		if (_bowlingBall == null)
		{
			return;
		}

		// TODO: This code has an error, fix it later!!!
		// Move bowling ball before launching it
		if (@event is InputEventMouseMotion mouseMotion && !_bIsBallLanched)
		{
			if ((SpawnBallLocation.Position - _bowlingBall.Position).Length() < 0.5f) // Restriction to forbid player to move ball too far
			{
				_cachedBallPosition = _bowlingBall.Position;

				float moveModifier = 0.01f * _moveBallSensitivity;

				_bowlingBall.MoveAndCollide(new Vector3(-mouseMotion.Relative.Y, 0.0f, mouseMotion.Relative.X) * moveModifier);
			}
			else
			{
				// TODO: This makes ball glitching a bit. Need to be remade. But for now it's fine.
				_bowlingBall.Position = _cachedBallPosition;
			}
		}

		// Launch bowling ball into the pins
        if (Input.IsActionJustPressed("MainAction"))
		{
			if (!_bIsBallLanched)
			{
				_bowlingBall.Freeze = false;
				_bowlingBall.ApplyImpulse(new Vector3(_baseThrowPower * _currentThrowPower, 0.0f, 0.0f));
				//_bowlingBall.ApplyTorqueImpulse(new Vector3(500.0f, 0.0f, 0.0f));
				_bIsBallLanched = true;
				PowerMeterUI.Visible = false;
			}
		}
    }

	private void SpawnBall()
	{
		_bowlingBall = BowlingBallScene.Instantiate<RigidBody3D>();

		AddChild(_bowlingBall);

		_bowlingBall.Position = SpawnBallLocation.Position;

		_bIsBallLanched = false;
	}

	private void SpawnPins()
	{
		_bowlingPins = BowlingPinsScene.Instantiate<Node3D>();

		AddChild(_bowlingPins);

		_bowlingPins.Position = SpawnPinsLocation.Position;
	}

	private void PowerChangeProcess(double deltaTime)
	{
		if (_bIsBallLanched)
		{
			return;
		}

		_cachedPowerDeltaTime += deltaTime;

		if (_cachedPowerDeltaTime >= _changeThrowPowerTime)
		{
			_cachedPowerDeltaTime = 0;

			if (_currentThrowPower == _maxThrowPower)
			{
				_currentThrowPower = 0;
			}
			else
			{
				_currentThrowPower++;
			}

			UpdatePowerMeterScale();
		}
	}

	private void UpdatePowerMeterScale()
	{
		if (_currentThrowPower == 0)
		{
			foreach (ColorRect scalePart in _powerMeterParts)
			{
				scalePart.Visible = false;
			}
		}
		else
		{
			_powerMeterParts[_currentThrowPower - 1].Visible = true;
		}
	}

	public void OnGameModeChanged(TempleState.GameMode gameMode)
	{
		if (gameMode == TempleState.GameMode.RollingBalls)
		{
			_bIsBowlingLaneClean = false;

			SetBowlingUIVisibility(true);

			SpawnBall();
			SpawnPins();
		}
		else if (!_bIsBowlingLaneClean)
		{
			_bIsBowlingLaneClean = true;

			SetBowlingUIVisibility(false);

			_bowlingBall.QueueFree();
			_bowlingPins.QueueFree();
		}
	}

	private void SetBowlingUIVisibility(bool bIsVisible)
	{
		PowerMeterUI.Visible = bIsVisible;
		EndLineView.Visible = bIsVisible;
	}
}
