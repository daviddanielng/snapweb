namespace cli.lib;

using System.Text.Json;

class AppConfig
{
    public string Name { get; set; }
    public int Age { get; set; }
    public static AppConfig? Read(String[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("No config file provided.");
            return null;
        }
        // AppConfig config = JsonSerializer.Deserialize<AppConfig>(fs)!;
        Console.WriteLine("Reading config...");
        return new AppConfig();
    }
}
