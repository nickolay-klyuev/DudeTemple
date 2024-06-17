using Godot;

public partial class PauseMenu : Control
{
	[Signal]
	public delegate void PauseMenuClosedEventHandler();

	[Signal]
	public delegate void PauseMenuOpenedEventHandler();
	
	[Export]
	private Control _content;

	[Export]
	private Control _shop;

	[Export]
	private Control _settings;

	public bool bIsOpened { get; private set; } = true;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (_content == null)
		{
			_content = GetNode<Control>("Content");
		}

		#if DEBUG
		CheckHelper.Check(this, _content, _shop, _settings);
		#endif

		HideContentChildren();
		Close();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Open()
	{
		if (bIsOpened)
		{
			return;
		}

		Visible = true;
		bIsOpened = true;

		EmitSignal(SignalName.PauseMenuOpened);
	}

	public void Close()
	{
		if (!bIsOpened)
		{
			return;
		}

		Visible = false;
		bIsOpened = false;

		EmitSignal(SignalName.PauseMenuClosed);
	}

	private void HideContentChildren()
	{
		foreach (Control child in _content.GetChildren())
		{
			child.Visible = false;
		}
	}

	public void ShowShop()
	{
		HideContentChildren();

		_shop.Visible = true;
	}

	public void ShowSettings()
	{
		HideContentChildren();

		_settings.Visible = true;
	}
}
