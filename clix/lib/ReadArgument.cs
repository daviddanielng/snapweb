namespace cli.lib;

public class ReadArgument
{
    public string ConfigFilePath { get; set; }
    public bool Verbose = false;

    public bool Read(String[] args)
    {
        Dictionary<string, bool> wasLast = [];
        wasLast.Add("config", false);
        if (args.Length == 0)
        {
            Logging.Error("No config file provided.");
            Help();
            return false;
        }
        foreach (var arg in args)
        {
            if (wasLast["config"] && arg.StartsWith("--"))
            {
                Logging.Error("Expected file path after --file");
                return false;
            }
            if (wasLast["config"])
            {
                wasLast["config"] = false;
                continue;
            }

            switch (arg)
            {
                case "--file":

                    wasLast["file"] = true;
                    break;
                case "--verbose":
                    wasLast["verbose"] = true;
                    break;

            }
        }


        return true;
    }
    private static string Help()
    {
        return "Usage: snapweb <config-file-path>";
    }
}
