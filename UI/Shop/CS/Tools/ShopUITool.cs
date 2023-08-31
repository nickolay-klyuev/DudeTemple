using Godot;
using Godot.Collections;
using System;

[Tool]
public partial class ShopUITool : Control, IMenuInteract
{
	[ExportCategory("UI Constructer")]
	[Export]
	public Array<EFurniture> Environments
	{
		get => _environments;
		set
		{
			_environments = value;
			ConstructShopUIEditorOnly();
		}
	}

	private Array<EFurniture> _environments;

	private const string SELF_OPENING_BLOCK_PATH = "res://UI/Scenes/SelfOpeningBlock.tscn";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetupShopUIInfoGameplayOnly();
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

	private void SetupShopUIInfoGameplayOnly()
	{
		// Setup UI only during gameplay
		if (Engine.IsEditorHint())
		{
			return;
		}

		Control environment = GetNode<Control>("TabContainer/Environment");
		#if DEBUG
		CheckHelper.CheckUI(environment, this);
		#endif

		int index = 0;
		foreach(ShopEnvironmentCategoryHandler child in environment.GetChildren())
		{
			SBuildingData data = BuildingDataHelper.GetBuildingData(_environments[index]);
			child.SetTitle(data.Label);
			child.SetDescription(data.Description);
			index++;
		}
	}

	private void ConstructShopUIEditorOnly()
	{
		// Construct UI only in editor
		if (!Engine.IsEditorHint())
		{
			return;
		}

		PackedScene selfOpeningBlock = GD.Load<PackedScene>(SELF_OPENING_BLOCK_PATH);
		CheckHelper.CheckScene(selfOpeningBlock, this);

		Node environmentsHolder = GetNode("TabContainer/Environment");
		CheckHelper.CheckNode(environmentsHolder, this);

		ClearShopUI(environmentsHolder);

		for (int index = 0; index < _environments.Count; index++)
		{
			Node blockInstance = selfOpeningBlock.Instantiate();

			environmentsHolder.AddChild(blockInstance);

			blockInstance.Owner = GetTree().EditedSceneRoot;
		}
	}

	private void ClearShopUI(params Node[] holdersToClean)
	{
		foreach (Node holder in holdersToClean)
		{
			foreach (Node child in holder.GetChildren())
			{
				child.QueueFree();
			}
		}
	}
}
