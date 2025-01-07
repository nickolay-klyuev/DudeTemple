using Godot;
using Godot.Collections;
using System;
using System.Drawing;

public partial class PosterHandler : Decal
{
	[Export]
	private FileDialog _posterFileDialog;

	[Export]
	[ExportGroup("SaveMeta")]
	private string _posterId;

	[Export]
	private UserDataHolder _dataHolder;

	private Vector3 _posterBaseSize = new Vector3(2.0f, 1.0f, 2.0f);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_posterBaseSize.Y = Size.Y;

#if DEBUG
		CheckHelper.CheckNode(_posterFileDialog, this);

		if (String.IsNullOrEmpty(_posterId))
		{
			GD.PrintErr($"{Name}: Poster id is missing!!! You need to add poster id to save poster in user file!!!");
		}

		CheckHelper.CheckNode(_dataHolder, this);
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

		SetNewPosterImage(path);

		_dataHolder.AddOrUpdatePosterImage(_posterId, path);

		// TODO: It's a bit bulky. Maybe find out better way to change MouseMode when FileDialog is closed.
		OnPosterFileDialogClosed();
	}

	private void OnPosterFileDialogClosed()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	private void OnPosterImagesLoaded(Dictionary<string, string> posterImages)
	{
		SetNewPosterImage(posterImages[_posterId]);
	}

	private void SetNewPosterImage(string imagePath)
	{
		Image poster = new Image();
		poster.Load(imagePath);

		ImageTexture posterTexture = ImageTexture.CreateFromImage(poster);
		Vector2 textureSize = posterTexture.GetSize();

		// Resize decal to look good with custom image.
		if (textureSize.Y > textureSize.X)
		{
			Size = new Vector3(_posterBaseSize.X, _posterBaseSize.Y, _posterBaseSize.Z * textureSize.Y / textureSize.X);
		}
		else
		{
			Size = new Vector3(_posterBaseSize.X * textureSize.X / textureSize.Y, _posterBaseSize.Y, _posterBaseSize.Z);
		}

		TextureAlbedo = posterTexture;
	}
}