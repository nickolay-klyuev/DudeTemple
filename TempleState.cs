using Godot;

public partial class TempleState : Node3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public Node3D GetDude()
	{
		return GetNode<Node3D>("Dude");
	}

	public UserDataHolder GetUserDataHolder()
	{
		return GetNode<UserDataHolder>("UserDataHolder");
	}

	public FurnitureManager GetFurnitureManager()
	{
		return GetNode<FurnitureManager>("FurnitureManager");
	}
}