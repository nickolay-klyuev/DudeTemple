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
	
	private Dictionary<string, Variant> _currentSettings;
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
		if (FileAccess.FileExists(SETTINGS_FILE_PATH))
		{
			LoadDudeSettings();
		}
		else
		{
			LoadDefaultSettings();
			AcceptSettings();
		}
	}

	private void LoadDefaultSettings()
	{
		#if DEBUG
		GD.Print("Loading default settings.");
		#endif
		
		_tempSettings = new Dictionary<string, Variant>()
		{
			{ CAMERA_SPEED, 0.3f },
			{ DISPLAY_MODE, (int)DisplayServer.WindowMode.Fullscreen }
		};
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

			LoadDefaultSettings();
			return;
		}

		_currentSettings = loadedSettings;
		
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
			if (_currentSettings.ContainsKey(temp.Key))
			{
				_currentSettings[temp.Key] = temp.Value;
				continue;
			}
			
			GD.PrintErr("Something wrong in _tempSettings!");
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
