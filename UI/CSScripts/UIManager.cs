using Godot;
using Godot.Collections;
using System;

public partial class UIManager : Control
{
	public enum EFullScreenMenuType
	{
		None,
		PauseMenu,
		BuildingMenu
	}

	[ExportSubgroup("Full Screen UI")]
	[Export]
	private PauseMenuHandler _pauseMenu;

	[Export]
	private BuildingMenuHandler _buildingMenu;

	[ExportSubgroup("Gameplay UI")]
	[Export]
	private Label _interactLabel;

	private EFullScreenMenuType _activeMenu = EFullScreenMenuType.None;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_pauseMenu.Close();
		_buildingMenu.Close();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _UnhandledInput(InputEvent @event)
    {
		// Open/Close pause menu by action
        if (@event is InputEventKey && Input.IsActionJustPressed("Return"))
		{
			if (_activeMenu == EFullScreenMenuType.None)
			{
				OpenMenu(EFullScreenMenuType.PauseMenu);
			}
			else
			{
				CloseCurrent();
			}
		}
    }

	public void CloseCurrent()
	{
		switch(_activeMenu)
		{
			case EFullScreenMenuType.PauseMenu:
				_pauseMenu.Close();
				break;
			case EFullScreenMenuType.BuildingMenu:
				_buildingMenu.Close();
				break;
		}

		_activeMenu = EFullScreenMenuType.None;
		GetTree().Paused = false;
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public void OpenMenu(EFullScreenMenuType menu)
	{
		switch(menu)
		{
			case EFullScreenMenuType.PauseMenu:
				_pauseMenu.Open();
				break;
			case EFullScreenMenuType.BuildingMenu:
				_buildingMenu.Open();
				break;
		}

		_activeMenu = menu;
		GetTree().Paused = true;
		Input.MouseMode = Input.MouseModeEnum.Visible;
		SetInteractLabelVisibility(false);
	}

	public void OpenBuildingMenu(int buildingPlaceIndex)
	{
		_buildingMenu.BuildingPlaceIndex = buildingPlaceIndex;
		OpenMenu(EFullScreenMenuType.BuildingMenu);
	}

	public void SetInteractLabelVisibility(bool bIsVisible)
	{
		_interactLabel.Visible = bIsVisible;
	}
}
