using cli.lib;
using Microsoft.Playwright;

namespace cli.browser;

public interface IBaseBrowser
{
    Logging Logging { get; }
    AppConfig Config { get; }
    IBrowser? Browser { get; set; }
    IPlaywright? MPlaywright { get; set; }
    public virtual async Task Start()
    {
    }

    // public async Task Start()
    // {

    //     using var playwright = await Playwright.CreateAsync();
    //     await using var browser = await playwright.Chromium.LaunchAsync(new()
    //     {
    //         ExecutablePath = "/usr/bin/google-chrome"
    //     });
    //     var page = await browser.NewPageAsync();
    //     await page.GotoAsync("https://playwright.dev/dotnet");
    //     await page.ScreenshotAsync(new()
    //     {
    //         Path = "/home/daniel/Documents/Projects/snapweb/temp/screenshot_full.png",
    //         FullPage = true,
    //         Type = ScreenshotType.Png
    //     });
    //     await page.ScreenshotAsync(new()
    //     {
    //         Path = "/home/daniel/Documents/Projects/snapweb/temp/screenshot.png",
    //         Type = ScreenshotType.Png
    //     });
    //     await browser.CloseAsync();
    // }
}
