using Godot;
using System;

public partial class ShopEnvironmentCategoryHandler : VBoxContainer
{
	[Signal]
	public delegate void OnBuyButtonPressedIdEventHandler(int id);

	public int Id { get; set; }

	private Control _categoryContentBlock;
	private Label _title;
	private Label _description;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_categoryContentBlock = GetNode<Control>("Content");
		_title = GetNode<Label>("Title/Label");
		_description = GetNode<Label>("Content/Description");
		
		#if DEBUG
		CheckHelper.CheckUIs(this, _categoryContentBlock, _title, _description);
		#endif

		ToggleContent(false);
		UnhoverModulate();

		MouseEntered += HoverModulate;
		MouseExited += UnhoverModulate;
		GuiInput += OnGuiInput;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnBuyButtonPressed()
	{
		EmitSignal(SignalName.OnBuyButtonPressedId, Id);
	}

	public void SetTitle(string title)
	{
		_title.Text = title;
	}

	public void SetDescription(string desc)
	{
		_description.Text = desc;
	}

	private void HoverModulate()
	{
		if (_categoryContentBlock.Visible)
		{
			return;
		}
		
		Modulate = new Color(1.0f, 1.0f, 1.0f, 1.0f);
	}

	private void UnhoverModulate()
	{
		if (_categoryContentBlock.Visible)
		{
			return;
		}

		Modulate = new Color(1.0f, 1.0f, 1.0f, 0.5f);
	}

	private void OnGuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton && @event.IsPressed())
		{
			ToggleContent();
		}
	}

	private void ToggleContent()
	{
		_categoryContentBlock.Visible = !_categoryContentBlock.Visible;

		if (_categoryContentBlock.Visible)
		{
			Modulate = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		}
	}

	private void ToggleContent(bool bIsOpened)
	{
		_categoryContentBlock.Visible = bIsOpened;

		if (bIsOpened)
		{
			Modulate = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		}
	}
}
