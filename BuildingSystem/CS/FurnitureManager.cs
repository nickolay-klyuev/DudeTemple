using Godot;
using System;

public partial class FurnitureManager : Node3D
{
	[Export]
	private Node3D[] _furnitures = new Node3D[3];

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		#if DEBUG
		CheckHelper.Check(this, _furnitures);
		#endif

		foreach (IFurniture furniture in _furnitures)
		{
			furniture.DisableFurniture();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void EnableFurniture(int index)
	{
		(_furnitures[index] as IFurniture).EnableFurniture();
	}
}
