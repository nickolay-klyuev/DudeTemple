using Godot;
using Godot.Collections;
using System;

[Tool]
public partial class ShopUITool : Control
{
	[ExportCategory("UI Constructer")]
	[Export]
	public Array<string> Environments
	{
		get => _environments;
		set
		{
			_environments = value;
			ConstructShopUIEditorOnly();
		}
	}

	private Array<string> _environments;

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

	private void SetupShopUIInfoGameplayOnly()
	{
		// Setup UI only during gameplay
		if (Engine.IsEditorHint())
		{
			return;
		}

		int index = 0;
		foreach(ShopEnvironmentCategoryHandler child in GetNode("TabContainer/Environment").GetChildren())
		{
			child.SetTitle(_environments[index]);
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
		CheckHelperStatic.CheckScene(selfOpeningBlock, this);

		Node environmentsHolder = GetNode("TabContainer/Environment");
		CheckHelperStatic.CheckNode(environmentsHolder, this);

		ClearShopUI(environmentsHolder);

		foreach (string environmentName in _environments)
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
