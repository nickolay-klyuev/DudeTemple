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

    public void Build(int placeIndex, int cost, string scenePath)
    {
        if (_freeBuildingPlaces[placeIndex] && _scoreHolder.RemoveScore(cost))
        {
            PackedScene buildingPacked = GD.Load<PackedScene>(scenePath);
            Node3D building = buildingPacked.Instantiate<Node3D>();

            GetNode(_buildingPlacePathes[placeIndex]).AddChild(building);

            _freeBuildingPlaces[placeIndex] = false;
        }
    }
}
