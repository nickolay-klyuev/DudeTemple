using Godot;
using Godot.Collections;
using System;

public partial class Builder : Node
{
    [Export]
    private ScoreDataHolder _scoreHolder;

    [Export]
    [ExportGroup("Building Places")]
    private Array<NodePath> _buildingPlacePathes;

    private Array<bool> _freeBuildingPlaces = new Array<bool>();

    // Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        if (OS.IsDebugBuild())
        {
            CheckHelperStatic.CheckNode(_scoreHolder, this);
        }

        for (int index = 0; index < _buildingPlacePathes.Count; index++)
        {
            _freeBuildingPlaces.Add(true);
        }
	}

    public void Build(int placeIndex, string scenePath)
    {
        GD.Print("BUILD");
        if (_freeBuildingPlaces[placeIndex]) // TODO: building must replace building if it's busy
        {
            PackedScene buildingPacked = GD.Load<PackedScene>(scenePath);
            Node3D building = buildingPacked.Instantiate<Node3D>();

            GetNode(_buildingPlacePathes[placeIndex]).AddChild(building);

            _freeBuildingPlaces[placeIndex] = false;
        }
    }

    public void Unlock(EBuilding building, int cost)
    {
        #if DEBUG
        if (BuildingDataMapStatic.IsBuildingUnlocked(building))
        {
            GD.PrintErr($"{Name}: Building is already unlocked!!!!!!");
            return;
        }
        #endif

        if (_scoreHolder.RemoveScore(cost))
        {
            BuildingDataMapStatic.UnlockBuilding(building);
        }
    }
}
