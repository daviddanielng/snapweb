using System.Text.Json;
namespace cli.lib;

public class Utils
{
    public static string ReadFile(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"File not found: {path}");
        var content = File.ReadAllText(path);
        return content;

    }
    public static async Task<bool> CreateDirIfNotExists(string path)
    {
        try
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Logging.InfoVerbose($"Created directory: {path}");
            }
            return true;
        }
        catch (Exception ex)
        {
            Logging.ErrorVerbose($"Exception occurred while creating directory {path}: {ex}", $"Failed to create directory {path}: {ex.Message}");
            return false;
        }
    }

}
