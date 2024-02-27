using Godot;
using System;

public partial class TempleController : Node
{
    [Signal]
	public delegate void TemplePausedEventHandler();

	[Signal]
	public delegate void TempleUnpausedEventHandler();
    
    #if DEBUG
    private UserDataHolder _userDataHolder;
    #endif //DEBUG

    [Export]
    private string _resumeBtnPath = "./PauseMenu/MenuContainer/ResumeMenuButton";

    [Export]
    private string _exitBtnPath = "./PauseMenu/MenuContainer/ExitMenuButton";

    public override void _Ready()
	{
        Button exitBtn = GetNode<Button>(_exitBtnPath);
        Button resumeBtn = GetNode<Button>(_resumeBtnPath);

        #if DEBUG
		_userDataHolder = GetNode<UserDataHolder>("../UserDataHolder");

        CheckHelper.Check(this, _userDataHolder, exitBtn, resumeBtn);
        #endif //DEBUG

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
		#endif //DEBUG

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

				EmitSignal(SignalName.TemplePaused);
			}
		}
	}

    private void UnpauseTemple()
    {
        GetTree().Paused = false;
        Input.MouseMode = Input.MouseModeEnum.Captured;

        EmitSignal(SignalName.TempleUnpaused);
    }

    private void QuitTemple()
    {
        GetTree().Quit();
    }
}