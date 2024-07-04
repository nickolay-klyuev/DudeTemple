using Godot;
using System;

public partial class LoadingIcon : Control
{
	[Export] private TextureRect _texture;

	private bool _bIsActive = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_texture ??= GetChild<TextureRect>(0);

#if DEBUG
		CheckHelper.Check(this, _texture);
#endif

		_texture.Modulate = new Color(1.0f, 1.0f, 1.0f, 0.0f);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_bIsActive)
		{
			_texture.Rotation += (float)delta;
		}
	}

	public void Activate()
	{
		if (_bIsActive)
		{
			return;
		}
		
		_bIsActive = true;

		Tween tween = CreateTween();
		tween.TweenProperty(_texture, "modulate", new Color(1.0f, 1.0f, 1.0f, 0.8f), 0.5);
	}

	public void Deactivate()
	{
		if (!_bIsActive)
		{
			return;
		}
		
		Tween tween = CreateTween();
		tween.TweenProperty(_texture, "modulate", new Color(1.0f, 1.0f, 1.0f, 0.0f), 0.5);

		SceneTreeTimer awaitTimer = GetTree().CreateTimer(0.5);
		awaitTimer.Timeout += () => { _bIsActive = false; };
	}
}
