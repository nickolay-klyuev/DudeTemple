using System;
using System.Collections.Generic;

public static class BuildingDataHelper
{
    private static List<SBuildingData> _buildingData = new List<SBuildingData>()
    {
        new SBuildingData(EBuilding.Bar, "Bar", 1, "res://BarPrototype/Scenes/BarPrototype.tscn"),
        new SBuildingData(EBuilding.Toilet, "Toilet", 100, ""),
        new SBuildingData(EBuilding.Lights, "Lights", 150, ""),
        new SBuildingData(EBuilding.Test, "Test", 5, "")
    };

    private static Dictionary<int, List<SBuildingData>> _buildingDataPlaceMap = new Dictionary<int, List<SBuildingData>>()
    {
        { 0, new List<SBuildingData>(){ _buildingData[0], _buildingData[1], _buildingData[2], _buildingData[3]} }
    };

    private static Dictionary<EBuilding, SBuildingData> _buildingDataMap = new Dictionary<EBuilding, SBuildingData>()
    {
        { EBuilding.Bar, _buildingData[0] },
        { EBuilding.Toilet, _buildingData[1] },
        { EBuilding.Lights, _buildingData[2] },
        { EBuilding.Test, _buildingData[3] }
    };

    public static List<SBuildingData> GetBuildingDataForPlace()
    {
        return _buildingDataPlaceMap[0];
    }

    public static List<SBuildingData> GetBuildingDataForPlace(int placeIndex)
    {
        return _buildingDataPlaceMap[placeIndex];
    }

    public static SBuildingData GetBuildingData(EBuilding building)
    {
        return _buildingDataMap[building];
    }
}
