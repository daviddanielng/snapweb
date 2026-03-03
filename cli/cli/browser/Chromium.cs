using cli.lib;
using Microsoft.Playwright;

namespace cli.browser;

public class Chromium(AppConfig config, Logging logging) : IBaseBrowser
{
    public IBrowser? Browser { get; set; }
    public IPlaywright? MPlaywright { get; set; }

    public AppConfig Config => config;
    public Logging Logging => logging;


    public async Task<Chromium> Start()
    {
        try
        {
            var executablePath = Config.Browsers.FirstOrDefault(b => b.Name.Equals("chromium", StringComparison.OrdinalIgnoreCase))?.ExecutablePath;
            if (string.IsNullOrEmpty(executablePath))
            {
                Logging.Error("Chromium executable path not found in configuration.");
                Environment.Exit(1);
            }
            Logging.InfoVerbose($"Launching Chromium browser with executable path: {executablePath}");

            MPlaywright = await Playwright.CreateAsync();
            Browser = await MPlaywright.Chromium.LaunchAsync(new()
            {
                ExecutablePath = executablePath
            });

            Logging.InfoVerbose("Chromium browser launched successfully.");
            return this;
        }
        catch (Exception ex)
        {
            Logging.ErrorVerbose($"Failed to launch Chromium browser: {ex}", $"Failed to launch Chromium browser: {ex.Message}");
            Environment.Exit(1);
            return this;
        }
    }
    public async Task<string?> TakeScreenshot(string url, int width, int height, bool fullPage = false)
    {
        if (Browser == null)
        {
            Logging.Error("Browser instance is not initialized.");
            return null;
        }
        try
        {
            Logging.InfoVerbose($"Taking screenshot for URL {url} at {width}x{height}. Full page: {fullPage}. Browser: chrome", $"Taking screenshot for URL {url} at {width}x{height}.");
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var outputPath = Path.Combine(Config.OutputDir, $"screenshot_{timestamp}_{width}x{height}.{Config.ScreenshotOptions.Format.ToLower()}");
            var page = await Browser.NewPageAsync(new()
            {
                ViewportSize = new ViewportSize
                {
                    Width = width,
                    Height = height
                }
            });
            await page.GotoAsync(url);
            await page.ScreenshotAsync(new()
            {
                Path = outputPath,
                FullPage = fullPage,
                Type = ScreenshotType.Png
            });
            await page.CloseAsync();
            return outputPath;
        }
        catch (Exception ex)
        {
            Logging.ErrorVerbose($"Failed to take screenshot for URL {url} at {width}x{height}. Exception details: {ex}", $"Failed to take screenshot for URL {url} at {width}x{height}: {ex.Message}");

            return null;
        }
    }

    public async Task Dispose()
    {
        if (Browser != null)
        {
            await Browser.CloseAsync();
        }
        MPlaywright?.Dispose();
    }
}
