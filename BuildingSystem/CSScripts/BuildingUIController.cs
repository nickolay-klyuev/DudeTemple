using Godot;
using Godot.Collections;
using System;

public partial class BuildingUIController : Control, IMenuInteract
{
	[Export]
	Label BuildingLabel;

	[Export]
	Label CostLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CheckHelperStatic.CheckNodes(new Array<Node>(){BuildingLabel, CostLabel}, this);
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

	public void SetData(string buildingLabel, int cost)
	{
		BuildingLabel.Text = buildingLabel;
		CostLabel.Text = cost.ToString();
	}
}
