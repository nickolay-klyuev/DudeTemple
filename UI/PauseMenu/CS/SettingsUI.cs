using Godot;
using System;

public partial class SettingsUI : Panel
{
	[Export] private HSlider _cameraSpeed;
	[Export] private PercentageDisplay _cameraSpeedPercent;

	[Export] private OptionButton _displayMode;

	[Export] private Button _applyButton;
	[Export] private Button _resetButton;
	
	private DudeSettings _settings;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_cameraSpeed ??= GetNode<HSlider>("VBoxContainer/CameraSpeedContainer/CameraSpeed");
		_cameraSpeedPercent ??= GetNode<PercentageDisplay>("VBoxContainer/CameraSpeedContainer/PercentageDisplay");

		_displayMode ??= GetNode<OptionButton>("VBoxContainer/DisplayModeContainer/DisplayMode");

		_applyButton ??= GetNode<Button>("VBoxContainer/ApplyResetContainer/Apply");
		_resetButton ??= GetNode<Button>("VBoxContainer/ApplyResetContainer/Reset");
		
		_settings = GetNode<DudeSettings>("/root/DudeSettings");

#if DEBUG
		CheckHelper.Check(this, _settings, _cameraSpeed, _cameraSpeedPercent, _displayMode, _applyButton, _resetButton);
#endif

		UpdateUI();
		
		_cameraSpeed.DragEnded += UpdateCameraSpeed;
		_displayMode.ItemSelected += UpdateDisplayMode;

		_applyButton.Pressed += ApplySettings;
		_resetButton.Pressed += ResetSettings;

		_settings.Dirty += OnDirtySettings;
		_settings.Clean += OnCleanSettings;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void UpdateUI()
	{
		_cameraSpeed.Value = _settings.GetCameraSpeed();
		_cameraSpeedPercent.Update(_cameraSpeed.Value);

		_displayMode.Selected = _displayMode.GetItemIndex((int)_settings.GetDisplayMode());
	}

	private void UpdateCameraSpeed(bool bValueChanged)
	{
		if (bValueChanged)
		{
			_settings.SetCameraSpeed(_cameraSpeed.Value);
		}
	}

	private void UpdateDisplayMode(long index)
	{
		_settings.SetDisplayMode(_displayMode.GetItemId((int)index));
	}

	private void OnDirtySettings()
	{
		_applyButton.Disabled = false;
		_resetButton.Disabled = false;
	}

	private void OnCleanSettings()
	{
		_applyButton.Disabled = true;
		_resetButton.Disabled = true;
	}
	
	private void ApplySettings()
	{
		_settings.AcceptSettings();
	}

	private void ResetSettings()
	{
		_settings.ResetSettings();
		UpdateUI();
	}
}
