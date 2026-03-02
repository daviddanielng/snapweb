namespace cli.lib;

public class ReadArgument
{
    public string ConfigFilePath { get; set; } = "";
    public bool Verbose = false;
    public string Language { get; set; } = "en";

    public bool Read(String[] args)
    {
        if (args.Length == 0)
        {
            Help();
            return false;
        }
        Dictionary<string, bool> wasLast = [];
        wasLast.Add("config", false);
        wasLast.Add("verbose", false);
        wasLast.Add("language", false);
        foreach (var arg in args)
        {
            if (wasLast["config"] && arg.StartsWith("--"))
            {
                Logging.Error("Expected file path after --config");
                Help();
                return false;
            }
            if (wasLast["language"] && arg.StartsWith("--"))
            {
                Logging.Error("Expected language after --lang");
                Help();
                return false;
            }
            if (wasLast["config"])
            {
                wasLast["config"] = false;
                ConfigFilePath = arg;
                if (!ValidateConfigFilePath())
                {
                    return false;
                }


                continue;
            }

            if (wasLast["language"])
            {
                var availableLanguages = Translator.GetAvailableLanguages();
                if (!availableLanguages.Contains(arg))
                {
                    Logging.Error($"Language '{arg}' is not supported. Available languages: {string.Join(", ", availableLanguages)}");
                    return false;
                }
                wasLast["language"] = false;
                Language = arg;
                continue;
            }
            switch (arg)
            {
                case "--config":
                    if (!string.IsNullOrEmpty(ConfigFilePath))
                    {
                        Logging.Error("Unexpected argument --config after file path");
                        return false;
                    }
                    wasLast["config"] = true;
                    break;
                case "--verbose":
                    if (wasLast["verbose"])
                    {
                        Logging.Error("Unexpected argument --verbose after --verbose");
                        return false;

                    }
                    wasLast["verbose"] = true;
                    Verbose = true;
                    break;
                case "--lang":
                    if (wasLast["language"])
                    {
                        Logging.Error("Unexpected argument --lang after language");
                        return false;
                    }
                    wasLast["language"] = true;
                    break;
                default:
                    Logging.Error($"Unexpected argument {arg}");
                    return false;

            }
        }
        if (wasLast["config"])
        {
            Logging.Error("Expected file path after --config");
            return false;
        }
        if (Verbose)
        {
            Logging.Info($"Config file path: {ConfigFilePath}");
        }
        if (!wasLast["config"] && string.IsNullOrEmpty(ConfigFilePath))
        {
            Logging.Error("Expected --config argument");
            return false;
        }
        return true;
    }
    public bool ValidateConfigFilePath()
    {
        if (!File.Exists(ConfigFilePath))
        {
            Logging.Error($"Config file not found at path: {ConfigFilePath}");
            return false;
        }
        return true;
    }
    private static string Help()
    {
        return "Usage: snapweb --config <config file path>  --lang <language> [--verbose]\n " +
               "Options:\n" +
               "  --config <config file path>   Path to the configuration file.\n" +
               "  --lang <language>             Specify the language.\n" +
               "  --verbose                     Enable verbose logging.";
    }
}
