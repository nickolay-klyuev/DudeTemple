using Godot;
using System;

public partial class PosterHandler : Decal
{
	[Export]
	private FileDialog _posterFileDialog;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		#if DEBUG
		CheckHelperStatic.CheckNode(_posterFileDialog, this);
		#endif

		_posterFileDialog.FileSelected += OnPosterImageSelected;

		// TODO: It's a bit bulky. Maybe find out better way to change MouseMode when FileDialog is closed.
		_posterFileDialog.Canceled += OnPosterFileDialogClosed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OpenFileDialog()
	{
		_posterFileDialog.Popup();
		Input.MouseMode = Input.MouseModeEnum.Visible;
	}

	private void OnPosterImageSelected(string path)
	{
		#if DEBUG
		if (!FileAccess.FileExists(path))
		{
			GD.PrintErr($"{Name}: Couldn't load a poster picture. File: {path}, doesn't exist!!!");
			return;
		}
		#endif
		
		Image poster = new Image();
		poster.Load(path);

		ImageTexture posterTexture = ImageTexture.CreateFromImage(poster);
		Vector2 posterSize = posterTexture.GetSize();

		// Resize decal to look good with custom image.
		Size = new Vector3(Size.X, Size.Y, Size.X * posterSize.Y / posterSize.X);

		TextureAlbedo = posterTexture;

		// TODO: It's a bit bulky. Maybe find out better way to change MouseMode when FileDialog is closed.
		OnPosterFileDialogClosed();
	}

	private void OnPosterFileDialogClosed()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}
}
