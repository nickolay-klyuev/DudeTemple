using Godot;
using System;

public partial class MainMenuUI : Control
{
	[Export] private Button _startBtn;
	[Export] private Button _quitBtn;
	
	private const string MAIN_SCENE_PATH = "res://Temple.tscn";
	
	private bool _bIsLoading = false;

	private LoadingIcon _loadingIcon;

	public override void _Ready()
	{
		_loadingIcon = GetNode<LoadingIcon>("/root/LoadingIcon");
		
#if DEBUG
		CheckHelper.Check(this, _startBtn, _quitBtn, _loadingIcon);
#endif
	}

	public override void _Process(double delta)
	{
		if (_bIsLoading)
		{
			ResourceLoader.ThreadLoadStatus result = ResourceLoader.LoadThreadedGetStatus(MAIN_SCENE_PATH);

			switch (result)
			{
				case ResourceLoader.ThreadLoadStatus.InvalidResource:
				case ResourceLoader.ThreadLoadStatus.Failed:
					GD.PrintErr($"{MAIN_SCENE_PATH}: couldn't be loaded because of: {result}");
					_bIsLoading = false;
					break;
				case ResourceLoader.ThreadLoadStatus.Loaded:
					_bIsLoading = false;
					ChangeSceneOnLoad();
					break;
				case ResourceLoader.ThreadLoadStatus.InProgress:
				default:
					break;
			}
		}
	}

	private void ChangeSceneOnLoad()
	{
		if (ResourceLoader.LoadThreadedGet(MAIN_SCENE_PATH) is PackedScene loadedScene)
		{
			_loadingIcon.Deactivate();
			
			GetTree().ChangeSceneToPacked(loadedScene);
		}
	}

	private void Start()
	{
		_startBtn.Disabled = true;
		
		ResourceLoader.LoadThreadedRequest(MAIN_SCENE_PATH);
		_bIsLoading = true;
		_loadingIcon.Activate();
	}

	private void Quit()
	{
		_quitBtn.Disabled = true;
		
		GetTree().Quit();
	}
}
