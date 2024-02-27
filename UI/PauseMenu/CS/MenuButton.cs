using Godot;
using System;

public partial class MenuButton : Button
{
	[ExportCategory("Animations")]
	
	[Export]
	private string _hoverAnimName = "HoverAnim";

	[Export]
	private string _unhoverAnimName = "UnhoverAnim";

	[Export]
	private AnimationPlayer _animPlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (_animPlayer == null)
		{
			_animPlayer = GetNode<AnimationPlayer>("ButtonAnimPlayer");
		}

		#if DEBUG
		CheckHelper.Check(this, _animPlayer);
		#endif

		if (_animPlayer == null)
		{
			_animPlayer = new AnimationPlayer();
		}

		SetPivotToCenter();

		MouseEntered += PlayHoverAnim;
		MouseExited += PlayHoverAnimBack;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void PlayHoverAnim()
	{
		_animPlayer.Play(_hoverAnimName);
	}

	private void PlayHoverAnimBack()
	{
		_animPlayer.Play(_unhoverAnimName);
	}

	private void SetPivotToCenter()
	{
		PivotOffset = new Vector2(Size.X / 2, Size.Y / 2);
	}
}