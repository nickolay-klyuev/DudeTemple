using System;
using System.Collections.Generic;

public static class BuildingDataMapStatic
{
    private static Dictionary<int, List<SBuildingData>> _buildingData = new Dictionary<int, List<SBuildingData>>()
    {
        { 0, new List<SBuildingData>(){ new SBuildingData("Bar", 50, "res://BarPrototype/Scenes/BarPrototype.tscn"), new SBuildingData("Toilet", 100, ""),  
                                        new SBuildingData("Lights", 150, ""), new SBuildingData("Test", 5, "")}}
    };

    public static List<SBuildingData> GetBuildingData()
    {
        return _buildingData[0];
    }

    public static List<SBuildingData> GetBuildingData(int index)
    {
        return _buildingData[index];
    }
}
