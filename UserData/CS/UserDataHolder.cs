using Godot;
using Godot.Collections;
using System;

public partial class UserDataHolder : Node
{
    [Signal]
    public delegate void ScoreChangedEventHandler(int newScore);

    [Signal]
    public delegate void BuildingDataLoadedEventHandler(Dictionary<int, EBuilding> builtBuildings);

    [Signal]
    public delegate void PosterImagesLoadedEventHandler(Dictionary<string, string> posterImages);

    [Export]
    private Timer _dirtyTimer;

    private SUserData _userData = new SUserData();

    private bool _bIsDirty = false;

    public override void _Ready()
    {
        #if DEBUG
        CheckHelperStatic.CheckNode(_dirtyTimer, this);
        #endif

        _dirtyTimer.Timeout += SaveDirtyData;
        TreeExiting += ForceSaveDataOnExit;

        _userData = SaveDataHelper.LoadData();

        #if DEBUG
        SaveDataHelper.LogUserData(_userData);
        #endif

        EmitSignal(SignalName.ScoreChanged, _userData.Score);
        EmitSignal(SignalName.BuildingDataLoaded, _userData.BuiltBuildings);
        EmitSignal(SignalName.PosterImagesLoaded, _userData.PosterImages);
    }

    public void AddScore(int amount)
    {
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

    public void AddUnlockBuilding(EBuilding building)
    {
        _userData.UnlockedBuildings.Add(building);

        MarkAsDirty();
    }

    public bool IsBuildingUnlocked(EBuilding building)
    {
        return _userData.UnlockedBuildings.Contains(building);
    }

    public void AddOrUpdateBuiltBuilding(int placeIndex, EBuilding building)
    {
        if (_userData.BuiltBuildings.ContainsKey(placeIndex))
        {
            _userData.BuiltBuildings[placeIndex] = building;
        }
        else
        {
            _userData.BuiltBuildings.Add(placeIndex, building);
        }

        MarkAsDirty();
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
