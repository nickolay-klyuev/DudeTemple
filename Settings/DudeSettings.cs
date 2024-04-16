using Godot;
using Godot.Collections;

public partial class DudeSettings : Node
{
    private struct Data
    {
        public float CameraSpeed;
        public DisplayServer.WindowMode DisplayMode;
    }
    
    [Signal]
    public delegate void SettingsUpdatedEventHandler();

    private const string SETTINGS_FILE_PATH = "user://dude-settings.json";

    private Dictionary<string, Variant> _currentSettings;

    public float GetCameraSpeed()
    {
        return _currentSettings["CameraSpeed"].As<float>();
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
        
        _currentSettings = new Dictionary<string, Variant>()
        {
            { "CameraSpeed", 0.3f },
            { "DisplayMode", (int)DisplayServer.WindowMode.Fullscreen }
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
    }

    public void AcceptSettings()
    {
        using FileAccess settingsFile = FileAccess.Open(SETTINGS_FILE_PATH, FileAccess.ModeFlags.Write);

        string settingsJson = Json.Stringify(_currentSettings);

        settingsFile.StoreString(settingsJson);
        settingsFile.Close();

        EmitSignal(SignalName.SettingsUpdated);
    }
}