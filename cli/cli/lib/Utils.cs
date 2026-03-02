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

}
