using Godot;
using Godot.Collections;
using System;

public partial class ShopUIConstructor : Control, IMenuInteract
{
	[Signal]
	public delegate void BuyFurnitureRequestEventHandler(EFurniture furniture);

	[Export]
	private UserDataHolder _userData;

	private EFurniture[] _furnitures;

	private Control _environment;

	private const string SELF_OPENING_BLOCK_PATH = "res://UI/Scenes/SelfOpeningBlock.tscn";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		#if DEBUG
		CheckHelper.Check(this, _userData);
		#endif

		_userData.UnlockedFurnituresLoaded += OnUnlockedFurnituresLoaded;
		_userData.NewFurnitureUnlocked += OnNewFurnitureUnlocked;

		_furnitures = BuildingDataHelper.GetFurnitureForUnlock();

		ConstructShopUI();
		SetupShopUIInfo();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Open()
	{
		Visible = true;
	}

	public void Close()
	{
		Visible = false;
	}

	private void SetupShopUIInfo()
	{
		_environment = GetNode<Control>("TabContainer/Environment");

		#if DEBUG
		CheckHelper.Check(this, _environment);
		#endif

		int index = 0;
		foreach(ShopEnvironmentCategoryHandler child in _environment.GetChildren())
		{
			SBuildingData data = BuildingDataHelper.GetBuildingData(_furnitures[index]);
			child.Id = (int)_furnitures[index];
			child.SetTitle(data.Label);
			child.SetDescription(data.Description);
			child.SetCost(data.Cost);
			child.OnBuyButtonPressedId += MakeBuyFurnitureRequest;

			_userData.NewFurnitureUnlocked += DisableBuyButtonForFurniture;

			index++;
		}
	}

	private void MakeBuyFurnitureRequest(int id)
	{
		EmitSignal(SignalName.BuyFurnitureRequest, id);
	}

	private void ConstructShopUI()
	{
		PackedScene selfOpeningBlock = GD.Load<PackedScene>(SELF_OPENING_BLOCK_PATH);

		#if DEBUG
		CheckHelper.Check(this, selfOpeningBlock);
		#endif

		Node environmentsHolder = GetNode("TabContainer/Environment");

		#if DEBUG
		CheckHelper.CheckNode(environmentsHolder, this);
		#endif

		for (int index = 0; index < _furnitures.Length; index++)
		{
			Node blockInstance = selfOpeningBlock.Instantiate();

			environmentsHolder.AddChild(blockInstance);

			blockInstance.Owner = GetTree().EditedSceneRoot;
		}
	}

	private void OnNewFurnitureUnlocked(int furniture)
	{
		DisableBuyButtonForFurniture(furniture);

		foreach(ShopEnvironmentCategoryHandler category in _environment.GetChildren())
		{
			if (_userData.IsFurnitureCanBeUnlocked((EFurniture)category.Id))
			{
				category.EnableContent();
			}
		}
	}

	private void OnUnlockedFurnituresLoaded(Array<EFurniture> furnitures)
	{
		foreach (int furniture in furnitures)
		{
			DisableBuyButtonForFurniture(furniture);
		}

		DisableContentThatCantBeUnlocked();
	}

	private void DisableBuyButtonForFurniture(int furniture)
	{
		_environment.GetChild<ShopEnvironmentCategoryHandler>(furniture).DisableContent();
	}

	private void DisableContentThatCantBeUnlocked()
	{
		foreach(ShopEnvironmentCategoryHandler category in _environment.GetChildren())
		{
			if (!_userData.IsFurnitureCanBeUnlocked((EFurniture)category.Id))
			{
				category.DisableContent();
			}
		}
	}
}
