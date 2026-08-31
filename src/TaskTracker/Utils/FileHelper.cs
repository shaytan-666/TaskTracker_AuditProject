namespace TaskTracker.Utils;

public static class FileHelper
{
    public static string GetDataPath() => Path.Combine("data", "tasks.json");
}
