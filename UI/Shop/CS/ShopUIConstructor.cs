using Godot;
using System;

public partial class ShopUIConstructor : Control, IMenuInteract
{
	[Signal]
	public delegate void BuyFurnitureRequestEventHandler(EFurniture furniture);

	private EFurniture[] _furnitures;

	private const string SELF_OPENING_BLOCK_PATH = "res://UI/Scenes/SelfOpeningBlock.tscn";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
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
		Control environment = GetNode<Control>("TabContainer/Environment");

		#if DEBUG
		CheckHelper.Check(this, environment);
		#endif

		int index = 0;
		foreach(ShopEnvironmentCategoryHandler child in environment.GetChildren())
		{
			SBuildingData data = BuildingDataHelper.GetBuildingData(_furnitures[index]);
			child.Id = (int)_furnitures[index];
			child.SetTitle(data.Label);
			child.SetDescription(data.Description);
			child.OnBuyButtonPressedId += MakeBuyFurnitureRequest;
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
		CheckHelper.CheckNode(environmentsHolder, this);

		for (int index = 0; index < _furnitures.Length; index++)
		{
			Node blockInstance = selfOpeningBlock.Instantiate();

			environmentsHolder.AddChild(blockInstance);

			blockInstance.Owner = GetTree().EditedSceneRoot;
		}
	}
}
