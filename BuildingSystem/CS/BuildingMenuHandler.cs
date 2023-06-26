using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class BuildingMenuHandler : Control, IMenuInteract
{
	[Signal]
	public delegate void BuildPressedEventHandler(int placeIndex, string scenePath);

	[Export]
	Label BuildingLabel;

	[Export]
	Label CostLabel;

	private int _placeIndex;
	public int BuildingPlaceIndex
	{ 
		set
		{
			_placeIndex = value;
			BuildingDatas = BuildingDataMapStatic.GetBuildingData(_placeIndex);
		}
	}

	private List<SBuildingData> _datas;
	private List<SBuildingData> BuildingDatas
	{
		set
		{
			_datas = value;

			if (_activePage >= _datas.Count)
			{
				_activePage = _datas.Count - 1;
			}
		}
	}

	private int _activePage = 0;

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
		UpdatePage();
		
		Visible = true;
	}

	public void Close()
	{
		Visible = false;
	}

	public void NextPage()
	{
		_activePage++;
		if (_activePage >= _datas.Count)
		{
			_activePage = 0;
		}

		UpdatePage();
	}

	public void PreviousPage()
	{
		_activePage--;
		if (_activePage < 0)
		{
			_activePage = _datas.Count - 1;
		}

		UpdatePage();
	}

	private void UpdatePage()
	{
		BuildingLabel.Text = _datas[_activePage].Label;
		CostLabel.Text = _datas[_activePage].Cost.ToString();
	}

	public void OnBuildButtonPressed()
	{
		EmitSignal(SignalName.BuildPressed, _placeIndex, _datas[_activePage].ScenePath);
	}
}
