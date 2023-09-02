using Godot;
using Godot.Collections;
using System;

public partial class UIManager : Control
{
	public enum EMenuType
	{
		None,
		PauseMenu,
		ShopMenu
	}

	[ExportSubgroup("Full Screen UI")]
	[Export]
	private PauseMenuHandler _pauseMenu;

	[Export]
	private ShopUIConstructor _shopMenu;

	[ExportSubgroup("Gameplay UI")]
	[Export]
	private Label _interactLabel;

	private EMenuType _activeMenu = EMenuType.None;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		#if DEBUG
		CheckHelper.Check(this, _pauseMenu, _shopMenu, _interactLabel);
		#endif

		_pauseMenu.Close();
		_shopMenu.Close();

		// Subscribe for shop close button
		_shopMenu.GetNode<Button>("CloseButton").Pressed += CloseShopMenu;
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
			if (_activeMenu == EMenuType.None)
			{
				OpenPauseMenu();
			}
			else if (_activeMenu == EMenuType.ShopMenu)
			{
				CloseShopMenu();
			}
			else
			{
				ClosePauseMenu();
			}
		}
    }

	public void SetInteractLabelVisibility(bool bIsVisible)
	{
		_interactLabel.Visible = bIsVisible;
	}

	public void OpenShopMenu()
	{
		_shopMenu.Open();

		_activeMenu = EMenuType.ShopMenu;
	}

	private void CloseShopMenu()
	{
		_shopMenu.Close();

		_activeMenu = EMenuType.PauseMenu;
	}

	public void ClosePauseMenu()
	{
		_pauseMenu.Close();

		_activeMenu = EMenuType.None;
		GetTree().Paused = false;
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	private void OpenPauseMenu()
	{
		_pauseMenu.Open();

		_activeMenu = EMenuType.PauseMenu;
		GetTree().Paused = true;
		Input.MouseMode = Input.MouseModeEnum.Visible;
		SetInteractLabelVisibility(false);
	}
}
