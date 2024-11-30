using Godot;
using System;

public partial class Door : Node3D, IFurniture
{
	[Export] private AnimationPlayer _animPlayer;

	private bool _bIsEnabled = false;
	private bool _bIsClosed = true;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animPlayer ??= GetNode<AnimationPlayer>("AnimationPlayer");

#if DEBUG
		CheckHelper.Check(this, _animPlayer);
#endif
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void DoorInteraction()
	{
		if (!_bIsEnabled)
		{
			return;
		}
		
		if (_bIsClosed)
		{
			_bIsClosed = false;
			_animPlayer.Play("OpenDoor");
		}
		else
		{
			_bIsClosed = true;
			_animPlayer.Play("CloseDoor");
		}
	}

	public void EnableFurniture()
	{
		_bIsEnabled = true;
	}

	public void DisableFurniture()
	{
		_bIsEnabled = false;
	}

	public bool IsEnabled()
	{
		return _bIsEnabled;
	}

	public bool IsClosed()
	{
		return _bIsClosed;
	}
}
