using System.Text.Json;

namespace cli.lib;


public interface ITranslator
{
    string T(string key);
}


public class Translator : ITranslator
{
    private readonly Dictionary<string, string> _translations;

    public Translator(string language, string file)
    {
        var path = Path.Combine("translations", language, $"{file}.json");
        try
        {
            var content = Utils.ReadFile(path);
            _translations = JsonSerializer.Deserialize<Dictionary<string, string>>(content)!
                       ?? [];
        }
        catch (FileNotFoundException ex)
        {
            Logging.Error(ex.Message);
            _translations = new Dictionary<string, string>();
        }

    }

    public string T(string key)
    {
        return _translations.TryGetValue(key, out var value)
            ? value
            : key; // fallback to key if missing
    }
    public static bool TranslationLanguageExists(string language)
    {
        var path = Path.Combine("translations", language);
        return Directory.Exists(path);
    }
    public static List<string> GetAvailableLanguages()
    {
        var path = "translations";
        if (!Directory.Exists(path))
            return [];
        return [.. Directory.GetDirectories(path).Select(dir => Path.GetFileName(dir))];
    }

}
