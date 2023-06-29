using System;
using System.Collections.Generic;

public static class BuildingDataMapStatic
{
    private static Dictionary<int, List<SBuildingData>> _buildingData = new Dictionary<int, List<SBuildingData>>()
    {
        { 0, new List<SBuildingData>(){ new SBuildingData(EBuilding.Bar, "Bar", 1, "res://BarPrototype/Scenes/BarPrototype.tscn"),
                                        new SBuildingData(EBuilding.Toilet, "Toilet", 100, ""),  
                                        new SBuildingData(EBuilding.Lights, "Lights", 150, ""), 
                                        new SBuildingData(EBuilding.Test, "Test", 5, "")}}
    };

    private static Dictionary<EBuilding, bool> _unlockedBuildings = new Dictionary<EBuilding, bool>()
    {
        { EBuilding.Bar, false },
        { EBuilding.Toilet, false },
        { EBuilding.Lights, false },
        { EBuilding.Test, false }
    };

    public static void UnlockBuilding(EBuilding building)
    {
        _unlockedBuildings[building] = true;
    }

    public static bool IsBuildingUnlocked(EBuilding building)
    {
        return _unlockedBuildings[building];
    }

    public static List<SBuildingData> GetBuildingData()
    {
        return _buildingData[0];
    }

    public static List<SBuildingData> GetBuildingData(int index)
    {
        return _buildingData[index];
    }
}
