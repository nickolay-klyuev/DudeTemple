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

public partial class TempleState : Node3D
{
	[Export]
	private WorldEnvironment _templeEnvironment;

	private CachedEnv _cachedEnv;

	public bool bIsBlurred { get; private set; } = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (_templeEnvironment == null)
		{
			_templeEnvironment = GetNode<WorldEnvironment>("WorldEnvironment");
		}

		#if DEBUG
		CheckHelper.Check(this, _templeEnvironment);
		#endif

		_cachedEnv.CacheEnv(_templeEnvironment.Environment);

		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public Node3D GetDude()
	{
		return GetNode<Node3D>("Dude");
	}

	public UserDataHolder GetUserDataHolder()
	{
		return GetNode<UserDataHolder>("UserDataHolder");
	}

	public FurnitureManager GetFurnitureManager()
	{
		return GetNode<FurnitureManager>("FurnitureManager");
	}

	public void BlurTemple()
	{
		_templeEnvironment.Environment.GlowEnabled = true;
		_templeEnvironment.Environment.GlowBloom = 1.0f;
		_templeEnvironment.Environment.GlowBlendMode = Environment.GlowBlendModeEnum.Replace;
		
		bIsBlurred = true;
	}

	public void UnblurTemple()
	{
		if (bIsBlurred)
		{
			_templeEnvironment.Environment.GlowEnabled = _cachedEnv.bGlowEnabledCache;
			_templeEnvironment.Environment.GlowBloom = _cachedEnv.GlowBloomCache;
			_templeEnvironment.Environment.GlowBlendMode = _cachedEnv.GlowBlendModeCache;

			bIsBlurred = false;
		}
	}
}
