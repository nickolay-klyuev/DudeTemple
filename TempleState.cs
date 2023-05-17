using Godot;
using System;

public partial class TempleState : Node3D
{
	[Signal]
	public delegate void GameModeChangedEventHandler(GameMode currentGameMode);

	[Signal]
	public delegate void ActiveBowlingLineChangedEventHandler(int activeLineIndex);

	public enum GameMode
	{
		Walking,
		RollingBalls
	}

	private GameMode _currentGameMode;
	
	private int _activeLineIndex;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;

		_currentGameMode = GameMode.Walking;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		// TODO: Don't need to use another one action to close bowling. Need to make good input controller.
		if (_currentGameMode == GameMode.RollingBalls && Input.IsActionJustPressed("CloseBowling"))
		{
			ChangeGameMode(GameMode.Walking);
		}
	}

	private void ChangeGameMode(GameMode newGameMode)
	{
		_currentGameMode = newGameMode;
		EmitSignal(SignalName.GameModeChanged, (int)_currentGameMode);
	}

	private void SetActiveLine(int lineIndex)
	{
		_activeLineIndex = lineIndex;
		EmitSignal(SignalName.ActiveBowlingLineChanged, _activeLineIndex);
	}

	public void StartBowling(int lineIndex)
	{
		SetActiveLine(lineIndex);
		ChangeGameMode(GameMode.RollingBalls);
	}

	public void OnScoreCounted(int score)
	{
		ChangeGameMode(GameMode.Walking);
	}
}
