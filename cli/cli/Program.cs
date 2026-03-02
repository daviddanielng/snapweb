using cli.lib;
using Microsoft.Playwright;
internal class Program
{
    private static async Task Main(string[] args)
    {
        // Translator translator = new("en", "main");
        // Console.WriteLine(translator.T("hello"));
        var config = new ReadArgument().Read(args);
        if (!config)
        {

            return;
        }
        // Console.WriteLine(args);
        // var argument = args[0];

        // using var playwright = await Playwright.CreateAsync();
        // await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        // {
        //     ExecutablePath = "/usr/bin/google-chrome"
        // });
        // var page = await browser.NewPageAsync();
        // await page.GotoAsync("https://google.com");
        // await page.ScreenshotAsync(new()
        // {
        //     Path = "/home/daniel/Documents/Projects/snapweb/temp/screenshot.png"
        // });
    }
}
