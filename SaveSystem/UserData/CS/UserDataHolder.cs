using Godot;
using Godot.Collections;
using System;

public partial class UserDataHolder : Node
{
    [Signal]
    public delegate void UserDataLoadedEventHandler();

    [Signal]
    public delegate void ScoreChangedEventHandler(int newScore);

    [Signal]
    public delegate void UnlockedFurnituresLoadedEventHandler(Array<EFurniture> unlockedFurnitures);

    [Signal]
    public delegate void NewFurnitureUnlockedEventHandler(int NewUnlockedFurniture);

    [Signal]
    public delegate void PosterImagesLoadedEventHandler(Dictionary<string, string> posterImages);

    [Export]
    private Timer _dirtyTimer;

    private LoadingIcon _loading;

    private SUserData _userData = new SUserData();

    private bool _bIsDirty = false;
    private bool _bCheckingForSaveCompleted = false;

    public override void _Ready()
    {
        _loading = GetNode<LoadingIcon>("/root/LoadingIcon");
        
#if DEBUG
        CheckHelper.Check(this, _dirtyTimer, _loading);
#endif

        _dirtyTimer.Timeout += SaveDirtyData;
        TreeExiting += ForceSaveDataOnExit;

        _userData = SaveDataHelper.LoadData();

        #if DEBUG
        SaveDataHelper.LogUserData(_userData);
        #endif

        EmitSignal(SignalName.UserDataLoaded);
        EmitSignal(SignalName.ScoreChanged, _userData.Score);
        EmitSignal(SignalName.UnlockedFurnituresLoaded, _userData.UnlockedFurnitures);
        EmitSignal(SignalName.PosterImagesLoaded, _userData.PosterImages);
    }

    public override void _Process(double delta)
    {
        if (_bCheckingForSaveCompleted)
        {
            if (!SaveDataHelper.IsSaving)
            {
                _bCheckingForSaveCompleted = false;
                _loading.Deactivate();
            }
        }
    }

    public void AddScore(int amount)
    {
        if (amount == 0)
        {
            return;
        }

        _userData.Score += amount;

        EmitSignal(SignalName.ScoreChanged, _userData.Score);

        MarkAsDirty();
    }

    public bool RemoveScore(int amount)
    {
        if (_userData.Score - amount < 0)
        {
            return false;
        }
        
        _userData.Score -= amount;

        EmitSignal(SignalName.ScoreChanged, _userData.Score);

        MarkAsDirty();
        
        return true;
    }

    public void AddUnlockedFurniture(EFurniture furniture)
    {
        _userData.UnlockedFurnitures.Add(furniture);

        EmitSignal(SignalName.NewFurnitureUnlocked, (int)furniture);

        MarkAsDirty();
    }

    public bool IsFurnitureUnlocked(EFurniture furniture)
    {
        return _userData.UnlockedFurnitures.Contains(furniture);
    }

    public bool IsFurnitureCanBeUnlocked(EFurniture furniture)
    {
        return !IsFurnitureUnlocked(furniture) && (IsFurnitureUnlocked(BuildingDataHelper.GetFurnitureParent(furniture)) || BuildingDataHelper.GetFurnitureParent(furniture) == furniture);
    }

    public bool IsFurnitureUnlocked(int furniture)
    {
        return IsFurnitureUnlocked((EFurniture)furniture);
    }

    public void AddOrUpdatePosterImage(string posterId, string imagePath)
    {
        if (_userData.PosterImages.ContainsKey(posterId))
        {
            _userData.PosterImages[posterId] = imagePath;
        }
        else
        {
            _userData.PosterImages.Add(posterId, imagePath);
        }

        MarkAsDirty();
    }

    public void SetDontShowTutorial(bool bDontShow)
    {
        _userData.DontShowTutorial = bDontShow;

        MarkAsDirty();
    }

    public bool GetDontShowTutorial()
    {
        return _userData.DontShowTutorial;
    }

    private void MarkAsDirty()
    {
        // Mark data as dirty. It will wait for _dirtyTimer and then save all data to a file.
        if (!_bIsDirty)
        {
            _bIsDirty = true;

            _dirtyTimer.Start();
        }
    }

    private void SaveDirtyData()
    {
        SaveDataHelper.SaveDataAsync(_userData);
        _bIsDirty = false;
        
        _loading.Activate();

        SceneTreeTimer timer = GetTree().CreateTimer(0.5);
        timer.Timeout += () => { _bCheckingForSaveCompleted = true; };
    }

    private void ForceSaveDataOnExit()
    {
        if (_bIsDirty)
        {
            _dirtyTimer.Stop();
            SaveDirtyData();
        }
    }
}
