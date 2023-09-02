using System;
using System.Collections.Generic;
using System.Linq;

public static class BuildingDataHelper
{
    private static EFurniture[] _furnitureForUnlock = { EFurniture.Bar, EFurniture.Toilet, EFurniture.Lights };

    public static EFurniture[] GetFurnitureForUnlock()
    {
        return _furnitureForUnlock;
    }

    private static List<SBuildingData> _buildingData = new List<SBuildingData>()
    {
        new SBuildingData(EFurniture.Bar, "Bar", 1, "res://BarPrototype/Scenes/NewBar.tscn", "This you can buy drinks or something else."),
        new SBuildingData(EFurniture.Toilet, "Toilet", 100, "res://BarPrototype/Scenes/BarPrototype.tscn", "You can use it when you need it."),
        new SBuildingData(EFurniture.Lights, "Lights", 150, "res://BarPrototype/Scenes/BarPrototype.tscn", "Add fancy and cozy light."),
        new SBuildingData(EFurniture.Test_1, "Test_1", 5, "res://BarPrototype/Scenes/BarPrototype.tscn"),
        new SBuildingData(EFurniture.Test_2, "Test_2", 10, "res://BarPrototype/Scenes/BarPrototype.tscn"),
        new SBuildingData(EFurniture.Test_3, "Test_3", 15, "res://BarPrototype/Scenes/BarPrototype.tscn"),
        new SBuildingData(EFurniture.Test_4, "Test_4", 2, "res://BarPrototype/Scenes/BarPrototype.tscn"),
        new SBuildingData(EFurniture.Test_5, "Test_5", 4, "res://BarPrototype/Scenes/BarPrototype.tscn"),
        new SBuildingData(EFurniture.Test_6, "Test_6", 8, "res://BarPrototype/Scenes/BarPrototype.tscn"),
        new SBuildingData(EFurniture.Test_7, "Test_7", 9, "res://BarPrototype/Scenes/BarPrototype.tscn"),
        new SBuildingData(EFurniture.Test_8, "Test_8", 50, "res://BarPrototype/Scenes/BarPrototype.tscn"),
        new SBuildingData(EFurniture.Test_9, "Test_9", 100, "res://BarPrototype/Scenes/BarPrototype.tscn"),
        new SBuildingData(EFurniture.Test_10, "Test_10", 18, "res://BarPrototype/Scenes/BarPrototype.tscn")
    };

    private static Dictionary<int, List<SBuildingData>> _buildingDataPlaceMap = new Dictionary<int, List<SBuildingData>>()
    {
        { 0, new List<SBuildingData>(){ _buildingData[0], _buildingData[1], _buildingData[2], _buildingData[3]} },
        { 1, new List<SBuildingData>(){ _buildingData[4], _buildingData[5]} },
        { 2, new List<SBuildingData>(){ _buildingData[6], _buildingData[7], _buildingData[8]} },
        { 3, new List<SBuildingData>(){ _buildingData[9], _buildingData[10], _buildingData[11], _buildingData[12]} }
    };

    private static Dictionary<EFurniture, SBuildingData> _buildingDataMap = new Dictionary<EFurniture, SBuildingData>()
    {
        { EFurniture.Bar, _buildingData[0] },
        { EFurniture.Toilet, _buildingData[1] },
        { EFurniture.Lights, _buildingData[2] },
        { EFurniture.Test_1, _buildingData[3] },
        { EFurniture.Test_2, _buildingData[4] },
        { EFurniture.Test_3, _buildingData[5] },
        { EFurniture.Test_4, _buildingData[6] },
        { EFurniture.Test_5, _buildingData[7] },
        { EFurniture.Test_6, _buildingData[8] },
        { EFurniture.Test_7, _buildingData[9] },
        { EFurniture.Test_8, _buildingData[10] },
        { EFurniture.Test_9, _buildingData[11] },
        { EFurniture.Test_10, _buildingData[12] }
    };

    public static List<SBuildingData> GetBuildingDataForPlace()
    {
        return _buildingDataPlaceMap[0];
    }

    public static List<SBuildingData> GetBuildingDataForPlace(int placeIndex)
    {
        return _buildingDataPlaceMap[placeIndex];
    }

    public static SBuildingData GetBuildingData(EFurniture building)
    {
        return _buildingDataMap[building];
    }
}
