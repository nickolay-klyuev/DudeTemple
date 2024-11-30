using Godot;
using System;

public partial class Tutorial : Control
{
	[Export]
	private CheckBox _dontShowCheckBox;

	[Export]
	private UserDataHolder _userDataHolder;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Visible = false;

#if DEBUG
		CheckHelper.Check(this, _dontShowCheckBox, _userDataHolder);
#endif

		_userDataHolder.UserDataLoaded += OnUserDataLoaded;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnUserDataLoaded()
	{
		bool dontShow = _userDataHolder.GetDontShowTutorial();

		if (dontShow)
		{
			CloseAndDestroy();
			return;
		}

		Visible = true;
		Input.MouseMode = Input.MouseModeEnum.Visible;
	}

	private void CloseAndDestroy()
	{
		if (_dontShowCheckBox.ButtonPressed)
		{
			_userDataHolder.SetDontShowTutorial(true);
		}

		Input.MouseMode = Input.MouseModeEnum.Captured;
		QueueFree();
	}
}
