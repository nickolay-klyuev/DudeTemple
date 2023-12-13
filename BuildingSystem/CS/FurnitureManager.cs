using Godot;
using Godot.Collections;
using System;

public partial class FurnitureManager : Node3D
{
	[Export]
	private UserDataHolder _userData;

	[Export]
	private Node3D[] _furnitures = new Node3D[5];

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		#if DEBUG
		CheckHelper.Check(this, _furnitures);
		CheckHelper.Check(this, _userData);
		#endif
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void BuyFurniture(int index)
	{
		if (_userData.IsFurnitureUnlocked(index))
		{
			return;
		}

		int cost = BuildingDataHelper.GetFurnitureCost((EFurniture)index);
		if (_userData.RemoveScore(cost))
		{
			_userData.AddUnlockedFurniture((EFurniture)index);
			EnableFurniture(index);
		}
	}

	public void OnUnlockedFurnituresLoaded(Array<EFurniture> unlockedFurnitures)
	{
		foreach (IFurniture furniture in _furnitures)
		{
			furniture.DisableFurniture();
		}

		foreach (int furniture in unlockedFurnitures)
		{
			EnableFurniture(furniture);
		}
	}

	private void EnableFurniture(int index)
	{
		(_furnitures[index] as IFurniture).EnableFurniture();
	}

	private void EnableFurniture(EFurniture furniture)
	{
		EnableFurniture((int)furniture);
	}
}
