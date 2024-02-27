using System;
using System.Collections.Generic;
using System.Linq;

public static class BuildingDataHelper
{
    private static EFurniture[] _furnitureForUnlock = new EFurniture[5]
    {
        EFurniture.BarCounter, EFurniture.Couch, EFurniture.TVTable, EFurniture.TV, EFurniture.Console 
    };

    public static EFurniture[] GetFurnitureForUnlock()
    {
        return _furnitureForUnlock;
    }

    private static SBuildingData[] _buildingData = new SBuildingData[5]
    {
        new SBuildingData(EFurniture.BarCounter, EFurniture.BarCounter, "Bar counter", 100, "A simple bar counter."),
        new SBuildingData(EFurniture.Couch, EFurniture.Couch, "Couch", 50, "A simple couch for comfort."),
        new SBuildingData(EFurniture.TVTable, EFurniture.TVTable, "TV table", 75, "Table for holding a TV set and a console."),
        new SBuildingData(EFurniture.TVTable, EFurniture.TV, "TV set", 200, "TV set for watching cozy videos."),
        new SBuildingData(EFurniture.TVTable, EFurniture.Console, "Console", 250, "A simple retro console replica.")
    };

    // private static Dictionary<int, List<SBuildingData>> _buildingDataPlaceMap = new Dictionary<int, List<SBuildingData>>()
    // {
    //     { 0, new List<SBuildingData>(){ _buildingData[0], _buildingData[1], _buildingData[2], _buildingData[3]} },
    //     { 1, new List<SBuildingData>(){ _buildingData[4], _buildingData[5]} },
    //     { 2, new List<SBuildingData>(){ _buildingData[6], _buildingData[7], _buildingData[8]} },
    //     { 3, new List<SBuildingData>(){ _buildingData[9], _buildingData[10], _buildingData[11], _buildingData[12]} }
    // };

    // private static Dictionary<EFurniture, SBuildingData> _buildingDataMap = new Dictionary<EFurniture, SBuildingData>()
    // {
    //     { EFurniture.Bar, _buildingData[0] },
    //     { EFurniture.Toilet, _buildingData[1] },
    //     { EFurniture.Lights, _buildingData[2] },
    //     { EFurniture.Test_1, _buildingData[3] },
    //     { EFurniture.Test_2, _buildingData[4] },
    //     { EFurniture.Test_3, _buildingData[5] },
    //     { EFurniture.Test_4, _buildingData[6] },
    //     { EFurniture.Test_5, _buildingData[7] },
    //     { EFurniture.Test_6, _buildingData[8] },
    //     { EFurniture.Test_7, _buildingData[9] },
    //     { EFurniture.Test_8, _buildingData[10] },
    //     { EFurniture.Test_9, _buildingData[11] },
    //     { EFurniture.Test_10, _buildingData[12] }
    // };

    // public static List<SBuildingData> GetBuildingDataForPlace(int placeIndex)
    // {
    //     return _buildingDataPlaceMap[placeIndex];
    // }

    public static string GetFurnitureName(EFurniture furniture)
    {
        return _buildingData[(int)furniture].Label;
    }

    public static SBuildingData GetBuildingData(EFurniture furniture)
    {
        return _buildingData[(int)furniture];
    }

    public static int GetFurnitureCost(EFurniture furniture)
    {
        return _buildingData[(int)furniture].Cost;
    }

    public static EFurniture GetFurnitureParent(EFurniture furniture)
    {
        return _buildingData[(int)furniture].Parent;
    }
}
