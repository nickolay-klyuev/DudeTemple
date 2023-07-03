using Godot.Collections;

public struct SUserData
{
    public int Score { get; set; }
    public Array<EBuilding> UnlockedBuildings { get; set; }
    public Dictionary<int, EBuilding> BuiltBuildings { get; set; }

    public SUserData()
    {
        Score = 0;
        UnlockedBuildings = new Array<EBuilding>();
        BuiltBuildings = new Dictionary<int, EBuilding>();
    }

    public SUserData(int score, Array<EBuilding> unlockedBuildings, Dictionary<int, EBuilding> builtBuildings)
    {
        Score = score;
        UnlockedBuildings = unlockedBuildings;
        BuiltBuildings = builtBuildings;
    }

    public SUserData(int score, Array<EBuilding> unlockedBuildings, Dictionary<int, int> builtBuildings)
    {
        Dictionary<int, EBuilding> builtBuildingsCasted = new Dictionary<int, EBuilding>();
        for (int placeIndex = 0; placeIndex < builtBuildings.Count; placeIndex++)
        {
            builtBuildingsCasted.Add(placeIndex, (EBuilding)builtBuildings[placeIndex]);
        }

        Score = score;
        UnlockedBuildings = unlockedBuildings;
        BuiltBuildings = builtBuildingsCasted;
    }
}
