using Godot.Collections;

public struct SUserData
{
    public int Score { get; set; }
    public Array<EFurniture> UnlockedFurnitures { get; set; }
    public Dictionary<string, string> PosterImages { get; set; }
    public bool DontShowTutorial { get; set; }

    public SUserData()
    {
        Score = 0;
        UnlockedFurnitures = new Array<EFurniture>();
        PosterImages = new Dictionary<string, string>();
        DontShowTutorial = false;
    }

    public SUserData(int score, Array<EFurniture> unlockedFurnitures, Dictionary<string, string> posterImages, bool dontShowTutorial)
    {
        Score = score;
        UnlockedFurnitures = unlockedFurnitures;
        PosterImages = posterImages;
        DontShowTutorial = dontShowTutorial;
    }
}
