using Godot;

struct CachedEnv
{
	public bool bGlowEnabledCache { get; private set; }
	public float GlowBloomCache { get; private set; }
	public Environment.GlowBlendModeEnum GlowBlendModeCache { get; private set; }

	public void CacheEnv(Environment envToCache)
	{
		bGlowEnabledCache = envToCache.GlowEnabled;
		GlowBloomCache = envToCache.GlowBloom;
		GlowBlendModeCache = envToCache.GlowBlendMode;
	}
}

public partial class TempleController : Node
{
    [Signal]
	public delegate void TemplePausedEventHandler();

	[Signal]
	public delegate void TempleUnpausedEventHandler();
    
#if DEBUG
    private UserDataHolder _userDataHolder;
#endif

    [Export]
    private string _resumeBtnPath = "./PauseMenu/MenuContainer/ResumeMenuButton";

    [Export]
    private string _exitBtnPath = "./PauseMenu/MenuContainer/ExitMenuButton";

    [Export]
	private WorldEnvironment _templeEnvironment;

	private CachedEnv _cachedEnv;

	public bool bIsBlurred { get; private set; } = false;

    public override void _Ready()
	{
        Button exitBtn = GetNode<Button>(_exitBtnPath);
        Button resumeBtn = GetNode<Button>(_resumeBtnPath);

        _templeEnvironment ??= GetNode<WorldEnvironment>("../WorldEnvironment");

#if DEBUG
		_userDataHolder = GetNode<UserDataHolder>("../UserDataHolder");

        CheckHelper.Check(this, _userDataHolder, exitBtn, resumeBtn, _templeEnvironment);
#endif

        _cachedEnv.CacheEnv(_templeEnvironment.Environment);

        exitBtn.Pressed += QuitTemple;
        resumeBtn.Pressed += UnpauseTemple;
	}
    

    public override void _UnhandledInput(InputEvent @event)
	{
#if DEBUG
		// For testing
		if (@event is InputEventKey && Input.IsPhysicalKeyPressed(Key.P))
		{
			_userDataHolder.AddScore(100);
		}
#endif

		// Pause/unpause temple by player
        if (@event is InputEventKey && Input.IsActionJustPressed("Return"))
		{
			if (GetTree().Paused)
			{
				UnpauseTemple();
			}
			else
			{
				GetTree().Paused = true;
				Input.MouseMode = Input.MouseModeEnum.Visible;

                BlurTemple();

				EmitSignal(SignalName.TemplePaused);
			}
		}
	}

    private void UnpauseTemple()
    {
        GetTree().Paused = false;
        Input.MouseMode = Input.MouseModeEnum.Captured;
        
        UnblurTemple();

        EmitSignal(SignalName.TempleUnpaused);
    }

    private void BlurTemple()
	{
		_templeEnvironment.Environment.GlowEnabled = true;
		_templeEnvironment.Environment.GlowBloom = 1.0f;
		_templeEnvironment.Environment.GlowBlendMode = Environment.GlowBlendModeEnum.Replace;
		
		bIsBlurred = true;
	}

	private void UnblurTemple()
	{
		if (bIsBlurred)
		{
			_templeEnvironment.Environment.GlowEnabled = _cachedEnv.bGlowEnabledCache;
			_templeEnvironment.Environment.GlowBloom = _cachedEnv.GlowBloomCache;
			_templeEnvironment.Environment.GlowBlendMode = _cachedEnv.GlowBlendModeCache;

			bIsBlurred = false;
		}
	}

    private void QuitTemple()
    {
        GetTree().Quit();
    }
}