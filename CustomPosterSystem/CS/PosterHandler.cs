using Godot;
using Godot.Collections;
using System;

public partial class PosterHandler : Decal
{
	[Export]
	private FileDialog _posterFileDialog;

	[Export]
	[ExportGroup("SaveMeta")]
	private string _posterId;

	[Export]
	private UserDataHolder _dataHolder;

	private Vector3 _initDecalSize = Vector3.Zero;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
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
		Vector2 posterSize = posterTexture.GetSize();

		// TODO: A bit of weird hack. Try to change it in the future.
		if (_initDecalSize == Vector3.Zero)
		{
			_initDecalSize = Size;
		}

		// Resize decal to look good with custom image.
		if (posterSize.Y > posterSize.X)
		{
			Size = new Vector3(_initDecalSize.X, _initDecalSize.Y, _initDecalSize.Z * posterSize.Y / posterSize.X);
		}
		else
		{
			Size = new Vector3(_initDecalSize.X * posterSize.X / posterSize.Y, _initDecalSize.Y, _initDecalSize.Z);
		}

		TextureAlbedo = posterTexture;
	}
}
