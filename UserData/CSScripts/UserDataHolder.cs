using Godot;
using Godot.Collections;
using System;

public partial class UserDataHolder : Node
{
    [Signal]
    public delegate void ScoreChangedEventHandler(int newScore);

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
        TreeExiting += ForceSaveDirtyData;

        _userData = SaveDataHelper.LoadData();

        EmitSignal(SignalName.ScoreChanged, _userData.Score);
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

    public void UnlockBuilding(EBuilding building)
    {
        _userData.UnlockedBuildings.Add(building);

        MarkAsDirty();
    }

    public bool IsBuildingUnlocked(EBuilding building)
    {
        return _userData.UnlockedBuildings.Contains(building);
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

    private void ForceSaveDirtyData()
    {
        if (_bIsDirty)
        {
            _dirtyTimer.Stop();
            SaveDirtyData();
        }
    }
}
