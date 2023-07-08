using Godot.Collections;

public struct SUserData
{
    public int Score { get; set; }
    public Array<EBuilding> UnlockedBuildings { get; set; }
    public Dictionary<int, EBuilding> BuiltBuildings { get; set; }
    public Dictionary<string, string> PosterImages { get; set; }

    public SUserData()
    {
        Score = 0;
        UnlockedBuildings = new Array<EBuilding>();
        BuiltBuildings = new Dictionary<int, EBuilding>();
        PosterImages = new Dictionary<string, string>();
    }

    public SUserData(int score, Array<EBuilding> unlockedBuildings, Dictionary<int, EBuilding> builtBuildings, Dictionary<string, string> posterImages)
    {
        Score = score;
        UnlockedBuildings = unlockedBuildings;
        BuiltBuildings = builtBuildings;
        PosterImages = posterImages;
    }

    public SUserData(int score, Array<EBuilding> unlockedBuildings, Dictionary<int, int> builtBuildings, Dictionary<string, string> posterImages)
    {
        Dictionary<int, EBuilding> builtBuildingsCasted = new Dictionary<int, EBuilding>();
        foreach (var builtBuilding in builtBuildings)
        {
            builtBuildingsCasted.Add(builtBuilding.Key, (EBuilding)builtBuilding.Value);
        }

        Score = score;
        UnlockedBuildings = unlockedBuildings;
        BuiltBuildings = builtBuildingsCasted;
        PosterImages = posterImages;
    }
}
