using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

public static class SaveDataHelper
{
    public static bool IsSaving { get; private set; } = false;

    private const string DATA_FILE_PATH = "user://dude-data.var";
    private const string FILE_HELPER = "wjetGsdgSFDksphas9SAFD";

    public static async void SaveDataAsync(SUserData userData)
    {
        IsSaving = true;
        await Task.Run(() => StorageData(userData));
    }

    private static void StorageData(SUserData userData)
    {
        using FileAccess saveFile = FileAccess.OpenEncryptedWithPass(DATA_FILE_PATH, FileAccess.ModeFlags.Write, FILE_HELPER);

        Variant scoreVar = userData.Score;
        Variant unlockedFurnituresVar = userData.UnlockedFurnitures;
        Variant posterImages = userData.PosterImages;

        saveFile.StoreVar(scoreVar);
        saveFile.StoreVar(unlockedFurnituresVar);
        saveFile.StoreVar(posterImages);
        saveFile.Close();

        IsSaving = false;
    }

    public static SUserData LoadData()
    {
        if (FileAccess.FileExists(DATA_FILE_PATH))
        {
            using FileAccess saveFile = FileAccess.OpenEncryptedWithPass(DATA_FILE_PATH, FileAccess.ModeFlags.Read, FILE_HELPER);

            // TODO: Test and fix any errors when there is no save data. 
            int loadedScore = (int)saveFile.GetVar();
            Array<EFurniture> unlockedFurnitures = (Array<EFurniture>)saveFile.GetVar();
            Dictionary<string, string> posterImages = (Dictionary<string, string>)saveFile.GetVar();

            saveFile.Close();

            return new SUserData(loadedScore, unlockedFurnitures, posterImages);
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
        GD.Print($"UnlockedFurnitures: {userData.UnlockedFurnitures};");
        GD.Print($"PosterImages: {userData.PosterImages};");
    } 
    #endif
}
