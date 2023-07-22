using System;
using System.Collections.Generic;

public static class BuildingDataHelper
{
    private static List<SBuildingData> _buildingData = new List<SBuildingData>()
    {
        new SBuildingData(EBuilding.Bar, "Bar", 1, "res://BarPrototype/Scenes/NewBar.tscn"),
        new SBuildingData(EBuilding.Toilet, "Toilet", 100, "res://BarPrototype/Scenes/BarPrototype.tscn"),
        new SBuildingData(EBuilding.Lights, "Lights", 150, "res://BarPrototype/Scenes/BarPrototype.tscn"),
        new SBuildingData(EBuilding.Test_1, "Test_1", 5, "res://BarPrototype/Scenes/BarPrototype.tscn"),
        new SBuildingData(EBuilding.Test_2, "Test_2", 10, "res://BarPrototype/Scenes/BarPrototype.tscn"),
        new SBuildingData(EBuilding.Test_3, "Test_3", 15, "res://BarPrototype/Scenes/BarPrototype.tscn"),
        new SBuildingData(EBuilding.Test_4, "Test_4", 2, "res://BarPrototype/Scenes/BarPrototype.tscn"),
        new SBuildingData(EBuilding.Test_5, "Test_5", 4, "res://BarPrototype/Scenes/BarPrototype.tscn"),
        new SBuildingData(EBuilding.Test_6, "Test_6", 8, "res://BarPrototype/Scenes/BarPrototype.tscn"),
        new SBuildingData(EBuilding.Test_7, "Test_7", 9, "res://BarPrototype/Scenes/BarPrototype.tscn"),
        new SBuildingData(EBuilding.Test_8, "Test_8", 50, "res://BarPrototype/Scenes/BarPrototype.tscn"),
        new SBuildingData(EBuilding.Test_9, "Test_9", 100, "res://BarPrototype/Scenes/BarPrototype.tscn"),
        new SBuildingData(EBuilding.Test_10, "Test_10", 18, "res://BarPrototype/Scenes/BarPrototype.tscn")
    };

    private static Dictionary<int, List<SBuildingData>> _buildingDataPlaceMap = new Dictionary<int, List<SBuildingData>>()
    {
        { 0, new List<SBuildingData>(){ _buildingData[0], _buildingData[1], _buildingData[2], _buildingData[3]} },
        { 1, new List<SBuildingData>(){ _buildingData[4], _buildingData[5]} },
        { 2, new List<SBuildingData>(){ _buildingData[6], _buildingData[7], _buildingData[8]} },
        { 3, new List<SBuildingData>(){ _buildingData[9], _buildingData[10], _buildingData[11], _buildingData[12]} }
    };

    private static Dictionary<EBuilding, SBuildingData> _buildingDataMap = new Dictionary<EBuilding, SBuildingData>()
    {
        { EBuilding.Bar, _buildingData[0] },
        { EBuilding.Toilet, _buildingData[1] },
        { EBuilding.Lights, _buildingData[2] },
        { EBuilding.Test_1, _buildingData[3] },
        { EBuilding.Test_2, _buildingData[4] },
        { EBuilding.Test_3, _buildingData[5] },
        { EBuilding.Test_4, _buildingData[6] },
        { EBuilding.Test_5, _buildingData[7] },
        { EBuilding.Test_6, _buildingData[8] },
        { EBuilding.Test_7, _buildingData[9] },
        { EBuilding.Test_8, _buildingData[10] },
        { EBuilding.Test_9, _buildingData[11] },
        { EBuilding.Test_10, _buildingData[12] }
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
