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
        new SBuildingData(EFurniture.BarCounter, EFurniture.BarCounter, "Bar counter", 100, "A simple bar counter.", "res://UI/PauseMenu/ShopTextures/Bar.png"),
        new SBuildingData(EFurniture.Couch, EFurniture.Couch, "Couch", 50, "A simple couch for comfort.", "res://UI/PauseMenu/ShopTextures/Couch.png"),
        new SBuildingData(EFurniture.TVTable, EFurniture.TVTable, "TV table", 75, "Table for holding a TV set and a console.", "res://UI/PauseMenu/ShopTextures/Table.png"),
        new SBuildingData(EFurniture.TVTable, EFurniture.TV, "TV set", 200, "TV set for watching cozy videos.", "res://UI/PauseMenu/ShopTextures/TV.png"),
        new SBuildingData(EFurniture.TVTable, EFurniture.Console, "Console", 250, "A simple retro console replica.", "res://UI/PauseMenu/ShopTextures/Console.png")
    };

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

    public static string GetFurnitureIconPath(EFurniture furniture)
    {
        return _buildingData[(int)furniture].IconPath;
    }
}
