using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

public static class SaveDataHelper
{
    private const string DATA_FILE_PATH = "user://dude-data.var";
    private const string FILE_HELPER = "wjetGsdgSFDksphas9SAFD";

    public static async void SaveDataAsync(SUserData userData)
    {
        await Task.Run(() => StorageData(userData));
    }

    private static void StorageData(SUserData userData)
    {
        using FileAccess saveFile = FileAccess.OpenEncryptedWithPass(DATA_FILE_PATH, FileAccess.ModeFlags.Write, FILE_HELPER);

        Variant scoreVar = userData.Score;
        Variant unlockedBuildingsVar = userData.UnlockedBuildings;
        Variant builtBuildingsVar = userData.BuiltBuildings;
        Variant posterImages = userData.PosterImages;

        saveFile.StoreVar(scoreVar);
        saveFile.StoreVar(unlockedBuildingsVar);
        saveFile.StoreVar(builtBuildingsVar);
        saveFile.StoreVar(posterImages);
        saveFile.Close();
    }

    public static SUserData LoadData()
    {
        if (FileAccess.FileExists(DATA_FILE_PATH))
        {
            using FileAccess saveFile = FileAccess.OpenEncryptedWithPass(DATA_FILE_PATH, FileAccess.ModeFlags.Read, FILE_HELPER);

            // TODO: Test and fix any errors when there is no save data. 
            int loadedScore = (int)saveFile.GetVar();
            Array<EBuilding> unlockedBuildings = (Array<EBuilding>)saveFile.GetVar();
            Dictionary<int, EBuilding> builtBuildings = (Dictionary<int, EBuilding>)saveFile.GetVar();
            Dictionary<string, string> posterImages = (Dictionary<string, string>)saveFile.GetVar();

            saveFile.Close();

            return new SUserData(loadedScore, unlockedBuildings, builtBuildings, posterImages);
        }
        else
        {
            return new SUserData();
        }
    }

    #if DEBUG
    public static void LogUserData(SUserData userData)
    {
        GD.Print("### User Data ###");
        GD.Print($"Score: {userData.Score};");
        GD.Print($"UnlockedBuildings: {userData.UnlockedBuildings};");
        GD.Print($"BuiltBuildings: {userData.BuiltBuildings};");
        GD.Print($"PosterImages: {userData.PosterImages};");
    } 
    #endif
}
