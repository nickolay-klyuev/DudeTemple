using Godot;
using Godot.Collections;

/* Singleton class that autoload from Godot. Path during runtime: "/root/DudeSettings" */
public partial class DudeSettings : Node
{
	[Signal]
	public delegate void SettingsUpdatedEventHandler();

	[Signal]
	public delegate void DirtyEventHandler();

	[Signal]
	public delegate void CleanEventHandler();

	private const string SETTINGS_FILE_PATH = "user://dude-settings.json";

	private const string CAMERA_SPEED = "CameraSpeed";
	private const string DISPLAY_MODE = "DisplayMode";
	
	private Dictionary<string, Variant> _currentSettings = new Dictionary<string, Variant>();
	private Dictionary<string, Variant> _tempSettings = new Dictionary<string, Variant>();
	
	private bool _bIsDirty = false;

	public void SetCameraSpeed(double newSpeed)
	{
		_tempSettings[CAMERA_SPEED] = newSpeed;
		SetDirty();
	}

	public float GetCameraSpeed()
	{
		return _currentSettings[CAMERA_SPEED].As<float>();
	}

	public void SetDisplayMode(int mode)
	{
		_tempSettings[DISPLAY_MODE] = mode;
		SetDirty();
	}

	public DisplayServer.WindowMode GetDisplayMode()
	{
		return _currentSettings[DISPLAY_MODE].As<DisplayServer.WindowMode>();
	}

	private void SetDirty()
	{
		if (!_bIsDirty)
		{
			_bIsDirty = true;
			EmitSignal(SignalName.Dirty);
		}
	}

	private void SetClean()
	{
		if (_bIsDirty)
		{
			_bIsDirty = false;
			EmitSignal(SignalName.Clean);
		}
	}

// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Set Default settings
		_currentSettings[CAMERA_SPEED] = 0.3f;
		_currentSettings[DISPLAY_MODE] = (int)DisplayServer.WindowMode.Fullscreen;
		
		if (FileAccess.FileExists(SETTINGS_FILE_PATH))
		{
			LoadDudeSettings();
		}
	}

	private void LoadDudeSettings()
	{
		using FileAccess settingsFile = FileAccess.Open(SETTINGS_FILE_PATH, FileAccess.ModeFlags.Read);

		string jsonString = settingsFile.GetAsText();
		settingsFile.Close();

		Dictionary<string, Variant> loadedSettings = (Dictionary<string, Variant>)Json.ParseString(jsonString);

		if (loadedSettings == null)
		{
			#if DEBUG
			GD.PrintErr("Couldn't load DudeSettings!");
			#endif

			//LoadDefaultSettings();
			return;
		}

		foreach (var setting in loadedSettings)
		{
			_currentSettings[setting.Key] = setting.Value;	
		}
		
		DisplayServer.WindowSetMode(GetDisplayMode());
	}

	public void AcceptSettings()
	{
		/* Don't need to save or apply if not dirty */
		if (!_bIsDirty)
		{
			return;
		}

		foreach (var temp in _tempSettings)
		{
			_currentSettings[temp.Key] = temp.Value;
		}
		
		using FileAccess settingsFile = FileAccess.Open(SETTINGS_FILE_PATH, FileAccess.ModeFlags.Write);

		string settingsJson = Json.Stringify(_currentSettings);

		settingsFile.StoreString(settingsJson);
		settingsFile.Close();
		
		_tempSettings.Clear();
		SetClean();
		
		EmitSignal(SignalName.SettingsUpdated);
		
		DisplayServer.WindowSetMode(GetDisplayMode());
	}

	public void ResetSettings()
	{
		_tempSettings.Clear();
		SetClean();
	}
}
