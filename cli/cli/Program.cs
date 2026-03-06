using cli.lib;

internal class Program
{
    private static async Task Main(string[] args)
    {
        if (args.Contains("--schema"))
        {
            File.WriteAllText("appconfig.schema.json", AppConfig.GenerateJsonSchema());
            Console.WriteLine("JSON schema for AppConfig has been generated at appconfig.schema.json");
            return;
        }
        var arguments = new ReadArgument(); ;
        if (!arguments.Read(args))
        {

            return;
        }
        Logging.Verbose = arguments.Verbose;
        Logging.Output = arguments.Output;
        var config = new AppConfig().ReadJson(arguments.ConfigFilePath);
        if (config == null)
        {
            return;
        }

        Logging.InfoVerbose(config.SettingsToWords());

        await cli.Start.Run(config);

    }
}
