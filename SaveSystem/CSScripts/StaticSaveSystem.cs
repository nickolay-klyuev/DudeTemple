using Godot;
using System;
using System.Threading.Tasks;

public static class StaticSaveSystem
{
    public static async void SaveScoreAsync(int score)
    {
        await Task.Run(() => StorageScore(score));
    }

    private static void StorageScore(int score)
    {
        using FileAccess saveFile = FileAccess.Open("user://dude-data.dt", FileAccess.ModeFlags.Write);
        saveFile.Store32((uint)score);
    }

    public static int LoadScore()
    {
        if (FileAccess.FileExists("user://dude-data.dt"))
        {
            using FileAccess saveFile = FileAccess.Open("user://dude-data.dt", FileAccess.ModeFlags.Read);
            return (int)saveFile.Get32();
        }
        else
        {
            return 0;
        }
    }
}
