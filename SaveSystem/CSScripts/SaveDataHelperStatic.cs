using Godot;
using System;
using System.Threading.Tasks;

public static class SaveDataHelperStatic
{
    private const string DATA_PATH = "user://dude-data.var";
    private const string FILE_HELPER = "wjetGsdgSFDksphas9SAFD";

    public static async void SaveScoreAsync(int score)
    {
        await Task.Run(() => StorageScore(score));
    }

    private static void StorageScore(int score)
    {
        using FileAccess saveFile = FileAccess.OpenEncryptedWithPass(DATA_PATH, FileAccess.ModeFlags.Write, FILE_HELPER);

        Variant scoreVar = score;
        saveFile.StoreVar(scoreVar);
        saveFile.Close();
    }

    public static int LoadScore()
    {
        if (FileAccess.FileExists(DATA_PATH))
        {
            using FileAccess saveFile = FileAccess.OpenEncryptedWithPass(DATA_PATH, FileAccess.ModeFlags.Read, FILE_HELPER);

            int loadedScore = (int)saveFile.GetVar();

            saveFile.Close();

            return loadedScore;
        }
        else
        {
            return 0;
        }
    }
}
