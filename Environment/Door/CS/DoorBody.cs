using Godot;
using System;

public partial class DoorBody : StaticBody3D, IInteractable
{
	[Signal] public delegate void OnInteractEventHandler();

	private Door _door;

	public override void _Ready()
	{
		_door = GetParent<Door>();

#if DEBUG
		CheckHelper.Check(this, _door);
#endif
	}

	public void Interact()
	{
		EmitSignal(SignalName.OnInteract);
	}

	public bool CanInteract()
	{
		return _door.IsEnabled();
	}

	public string GetInteractLabel()
	{
		if (_door.IsClosed())
		{
			return "open the door";
		}

		return "close the door";
	}
}
