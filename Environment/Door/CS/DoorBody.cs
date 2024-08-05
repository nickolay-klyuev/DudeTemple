using Godot;
using System;

public partial class DoorBody : StaticBody3D, IInteractable
{
	[Signal] public delegate void OnInteractEventHandler();

	public void Interact()
	{
		EmitSignal(SignalName.OnInteract);
	}
}
