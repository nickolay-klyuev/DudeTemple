using Godot;
using System;

public partial class SettingsUI : Panel
{
	[Export] private HSlider _cameraSpeed;
	[Export] private PercentageDisplay _cameraSpeedPercent;

	[Export] private OptionButton _displayMode;

	[Export] private Button _applyButton;
	[Export] private Button _resetButton;

	[ExportCategory("Control Settings")]
	[Export] private Control _rebindKeyMessage;
	//[Export] private Container _controlsContainer;

	private DudeSettings _generalSettings;
	private ControlSettings _controlSettings;

	private string _actionToRebind = "";
	private Button _actionBindButton = null;
	private bool _bIsListeningForInput = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_cameraSpeed ??= GetNode<HSlider>("VBoxContainer/CameraSpeedContainer/CameraSpeed");
		_cameraSpeedPercent ??= GetNode<PercentageDisplay>("VBoxContainer/CameraSpeedContainer/PercentageDisplay");

		_displayMode ??= GetNode<OptionButton>("VBoxContainer/DisplayModeContainer/DisplayMode");

		_applyButton ??= GetNode<Button>("VBoxContainer/ApplyResetContainer/Apply");
		_resetButton ??= GetNode<Button>("VBoxContainer/ApplyResetContainer/Reset");
		
		_generalSettings = GetNode<DudeSettings>("/root/DudeSettings");
		_controlSettings = GetNode<ControlSettings>("/root/ControlSettings");

#if DEBUG
		CheckHelper.Check(this, _generalSettings, _controlSettings, _cameraSpeed, _cameraSpeedPercent,
			_displayMode, _applyButton, _resetButton, _rebindKeyMessage);
#endif

		_rebindKeyMessage.Visible = false;

		UpdateUI();
		
		_cameraSpeed.DragEnded += UpdateCameraSpeed;
		_displayMode.ItemSelected += UpdateDisplayMode;

		_applyButton.Pressed += ApplySettings;
		_resetButton.Pressed += ResetSettings;

		_generalSettings.Dirty += OnDirtySettings;
		_generalSettings.Clean += OnCleanSettings;

		CallDeferred(nameof(UpdateControlsUI));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void UpdateUI()
	{
		_cameraSpeed.Value = _generalSettings.GetCameraSpeed();
		_cameraSpeedPercent.Update(_cameraSpeed.Value);

		_displayMode.Selected = _displayMode.GetItemIndex((int)_generalSettings.GetDisplayMode());
	}

	private void UpdateCameraSpeed(bool bValueChanged)
	{
		if (bValueChanged)
		{
			_generalSettings.SetCameraSpeed(_cameraSpeed.Value);
		}
	}

	private void UpdateDisplayMode(long index)
	{
		_generalSettings.SetDisplayMode(_displayMode.GetItemId((int)index));
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
		_generalSettings.AcceptSettings();
	}

	private void ResetSettings()
	{
		_generalSettings.ResetSettings();
		UpdateUI();
	}

	// Control Settings
	public void OnRebindKeyButtonPressed(string actionName, Button buttonPressed)
	{
		_actionToRebind = actionName;
		_actionBindButton = buttonPressed;
		_rebindKeyMessage.Visible = true;
		_bIsListeningForInput = true;
	}

	public override void _Input(InputEvent @event)
	{
		if (!_bIsListeningForInput)
		{
			return;
		}

		if (@event is InputEventKey && @event.IsPressed())
		{
			InputEventKey eventKey = @event as InputEventKey;
			_controlSettings.Set(_actionToRebind, eventKey);
			_actionBindButton.Text = eventKey.AsText();
			_bIsListeningForInput = false;
			_rebindKeyMessage.Visible = false;
		}
	}

	private void UpdateControlsUI()
	{
		//ControlSettings._cfg settings = _controlSettings.Get();

		//Button button = _controlsContainer.GetNode<Button>("MoveForward/RebindKeyButton");
		//button.Text = settings.MoveForward.AsText();
	}
}
