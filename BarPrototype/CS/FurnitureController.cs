using Godot;
using System;

public partial class FurnitureController : Node3D, IFurniture
{
	[Export]
	private CollisionShape3D _collision;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (_collision == null)
		{
			_collision = GetNode<CollisionShape3D>("StaticBody3D/CollisionShape3D");
		}

		// Collision can be null
		#if DEBUG
		//CheckHelper.Check(this, _collision);
		#endif
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void EnableFurniture()
	{
		if (_collision != null)
		{
			_collision.Disabled = false;
		}
		
		Visible = true;
	}

	public void DisableFurniture()
	{
		if (_collision != null)
		{
			_collision.Disabled = true;	
		}
		
		Visible = false;
	}
}
