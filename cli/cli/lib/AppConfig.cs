namespace cli.lib;

using System.Text.Json;

class AppConfig
{
    public string Name { get; set; }
    public int Age { get; set; }
    public static AppConfig? Read(String[] args)
    {
        // var translation = new Translator("en", "error");

        if (args.Length == 0)
        {
            Logging.Error("No config file provided.");
            return null;
        }
        // AppConfig config = JsonSerializer.Deserialize<AppConfig>(fs)!;
        Logging.Info("Reading config...");
        return new AppConfig();
    }
}
