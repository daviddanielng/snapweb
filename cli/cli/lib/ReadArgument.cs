namespace cli.lib;

public class ReadArgument
{
    public string ConfigFilePath { get; set; } = "";
    public bool Verbose = false;

    public bool Read(string[] args)
    {
        Logging.Info("Reading arguments .");
        if (args.Length == 0)
        {
            Logging.Error("No arguments provided.");
            Help();
            return false;
        }
        Dictionary<string, bool> wasLast = [];
        wasLast.Add("config", false);
        wasLast.Add("verbose", false);
        foreach (var arg in args)
        {
            if (wasLast["config"] && arg.StartsWith("--"))
            {
                Logging.Error("Expected file path after --config");
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
    private static void Help()
    {
        Logging.Info ("Usage: snapweb --config <config file path> [--verbose]\n " +
               "Options:\n" +
               "  --config <config file path>   Path to the configuration file.\n" +
               "  --verbose                     Enable verbose logging.");
    }
}
