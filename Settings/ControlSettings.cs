using Godot;
using Godot.Collections;

/* Singleton class that autoload from Godot. Path during runtime: "/root/ControlSettings" */
public partial class ControlSettings : Node
{
    public struct _cfg
    {
        public InputEventKey MoveForward;
    }

    public _cfg Get()
    {
        _cfg cfg = new _cfg();
        cfg.MoveForward = _config.GetValue(DEFAULT_SECTION_NAME, "MoveForward").As<InputEventKey>();

        return cfg;
    }

    private const string CONTROLS_FILE_PATH = "user://controls.cfg";
    private const string DEFAULT_SECTION_NAME = "controls-default";

    private bool _bIsDirty = false;
    private Timer _applyTimer = new Timer();
    private double _applyWaitTime = 5.0;
    private ConfigFile _config = new ConfigFile();

    public override void _Ready()
    {
        // Create empty config file if not exists
        if (FileAccess.FileExists(CONTROLS_FILE_PATH))
        {
            _config.Load(CONTROLS_FILE_PATH);
        }

        AddChild(_applyTimer);
        _applyTimer.OneShot = true;
        _applyTimer.Timeout += Apply;

        TreeExiting += ApplyOnExit;
    }

    public void Set(string actionName, InputEventKey eventKey)
    {
        _config.SetValue(DEFAULT_SECTION_NAME, actionName, eventKey);
        MakeDirty();
    }

    private void MakeDirty()
    {
        if (_bIsDirty)
        {
            _applyTimer.Start(_applyWaitTime);
            return;
        }

        _applyTimer.Start(_applyWaitTime);
        _bIsDirty = true;
    }

    private void Apply()
    {
        _config.Save(CONTROLS_FILE_PATH);
        _bIsDirty = false;
    }

    public void ApplyOnExit()
    {
        // Make sure to apply on exit if dirty
        if (_bIsDirty)
        {
            Apply();
        }
    }
}