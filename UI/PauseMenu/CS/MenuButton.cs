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

	[Export] private AudioStreamPlayer _hoverPlayer;
	[Export] private AudioStreamPlayer _pressedPlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animPlayer ??= GetNode<AnimationPlayer>("ButtonAnimPlayer");
		_hoverPlayer ??= GetNode<AudioStreamPlayer>("HoverStreamPlayer");
		_pressedPlayer ??= GetNode<AudioStreamPlayer>("PressedStreamPlayer");

#if DEBUG
		CheckHelper.Check(this, _animPlayer, _hoverPlayer, _pressedPlayer);
#endif

		_animPlayer ??= new AnimationPlayer();

		SetPivotToCenter();

		MouseEntered += PlayHoverAnimAndSound;
		MouseExited += PlayHoverAnimBack;
		Pressed += PlayPressedSound;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void PlayHoverAnimAndSound()
	{
		_animPlayer.Play(_hoverAnimName);
		_hoverPlayer.Play();
	}

	private void PlayPressedSound()
	{
		_pressedPlayer.Play();
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