using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class BuildingMenuHandler : Control, IMenuInteract
{
	[Signal]
	public delegate void BuildPressedEventHandler(int placeIndex, EFurniture building);

	[Signal]
	public delegate void UnlockPressedEventHandler(EFurniture building, int cost);

	[Export]
	private UserDataHolder _userDataHolder;

	[ExportGroup("SubUI")]
	[Export]
	private Label _buildingLabel;

	[Export]
	private Label _costLabel;

	[Export]
	private Button _buildButton;

	[Export]
	private Button _unlockButton;

	private int _placeIndex;
	public int BuildingPlaceIndex
	{ 
		set
		{
			_placeIndex = value;
			BuildingDatas = BuildingDataHelper.GetBuildingDataForPlace(_placeIndex);
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
		#if DEBUG
		CheckHelper.CheckNodes(new Array<Node>(){_buildingLabel, _costLabel, _buildButton, _unlockButton, _userDataHolder}, this);
		#endif
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
		if (_userDataHolder.IsFurnitureUnlocked(_datas[_activePage].Building))
		{
			_buildButton.Visible = true;
			_unlockButton.Visible = false;
			_costLabel.Visible = false;

			_buildingLabel.Text = _datas[_activePage].Label;
		}
		else
		{
			_buildButton.Visible = false;
			_unlockButton.Visible = true;
			_costLabel.Visible = true;

			_buildingLabel.Text = _datas[_activePage].Label;
			_costLabel.Text = _datas[_activePage].Cost.ToString();
		}
	}

	public void OnBuildButtonPressed()
	{
		EmitSignal(SignalName.BuildPressed, _placeIndex, (int)_datas[_activePage].Building);
	}

	public void OnUnlockButtonPressed()
	{
		EmitSignal(SignalName.UnlockPressed, (int)_datas[_activePage].Building, _datas[_activePage].Cost);
		UpdatePage();
	}
}
