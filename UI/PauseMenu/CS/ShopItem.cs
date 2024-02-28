using Godot;
using System;

[Tool]
public partial class ShopItem : Panel
{
	private EFurniture _type;

	[Export]
	public EFurniture ItemType
	{
		set
		{
			_type = value;

			UpdateInEditor();
		}
		get
		{
			return _type;
		}
	}

	private Control _buyContainer;
	private UserDataHolder _userData;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (Engine.IsEditorHint())
		{
			return;
		}

		_buyContainer = GetNode<Control>("Container/BuyContainer");
		_userData = GetTree().Root.GetNode<TempleState>("Temple").GetUserDataHolder();

		#if DEBUG
		CheckHelper.Check(this, _buyContainer, _userData);
		#endif

		_userData.UserDataLoaded += UpdateInGame;
		_userData.NewFurnitureUnlocked += OnFurnitureUnlocked;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void UpdateInEditor()
	{
		GetNode<Label>("Container/Name").Text = BuildingDataHelper.GetFurnitureName(_type);
		GetNode<Label>("Container/Cost").Text = BuildingDataHelper.GetFurnitureCost(_type).ToString();
		GetNode<TextureRect>("Container/Icon").Texture = ResourceLoader.Load<Texture2D>(BuildingDataHelper.GetFurnitureIconPath(_type));
	}

	private void UpdateInGame()
	{
		if (Engine.IsEditorHint())
		{
			return;
		}

		foreach (Control child in _buyContainer.GetChildren())
		{
			child.Visible = false;
		}

		if (_userData.IsFurnitureUnlocked(_type))
		{
			_buyContainer.GetNode<Control>("UnlockedLabel").Visible = true;
		}
		else if (_userData.IsFurnitureCanBeUnlocked(_type))
		{
			_buyContainer.GetNode<Control>("BuyButton").Visible = true;
		}
		else
		{
			_buyContainer.GetNode<Control>("UnavailableLabel").Visible = true;
		}
	}

	private void OnFurnitureUnlocked(int furniture)
	{
		if ((int)_type == furniture || BuildingDataHelper.GetFurnitureParent(_type) == (EFurniture)furniture)
		{
			UpdateInGame();
		}
	}

	private void BuyItem()
	{
		if (Engine.IsEditorHint())
		{
			return;
		}

		GetTree().Root.GetNode<TempleState>("Temple").GetFurnitureManager().BuyFurniture((int)_type);
	}
}
