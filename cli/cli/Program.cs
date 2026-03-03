using cli.browser;
using cli.lib;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var arguments = new ReadArgument(); ;
        if (!arguments.Read(args))
        {

            return;
        }
        var logging = new Logging();
        Logging.Verbose = arguments.Verbose;
        var config = new AppConfig().ReadJson(arguments.ConfigFilePath, arguments.Verbose);
        if (config == null)
        {
            return;
        }

        Logging.InfoVerbose(config.SettingsToWords());

        var chrome = await new Chromium(config, logging).Start();
        await chrome.TakeScreenshot("https://playwright.dev/dotnet", 1280, 720, true);
        // var capture = new Capture();
        // await capture.Start();


    }
}
