using cli.lib;
using Microsoft.Playwright;

namespace cli.browser;

public class Chromium(AppConfig config, bool headless = true) : IBaseBrowser
{
    public IBrowser? Browser { get; set; }
    public IPlaywright? MPlaywright { get; set; }

    public AppConfig Config => config;
    public bool Headless { get; set; } = headless;

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
            Logging.InfoVerbose($"Launching Chromium browser at executable path: {executablePath}");

            MPlaywright = await Playwright.CreateAsync();
            Browser = await MPlaywright.Chromium.LaunchAsync(new()
            {
                ExecutablePath = executablePath,
                Headless = Headless
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
    public async Task<IPage?> NewPage()
    {
        if (Browser == null)
        {
            Logging.Error("Browser instance is not initialized.");
            return null;
        }
        try
        {
            var page = await Browser.NewPageAsync();
            return page;
        }
        catch (Exception ex)
        {
            Logging.ErrorVerbose($"Failed to create new page in Chromium browser: {ex}", $"Failed to create new page in Chromium browser: {ex.Message}");
            return null;
        }
    }

    public async Task Dispose()
    {
        Logging.InfoVerbose("Closing Chromium browser...");
        if (Browser != null)
        {
            await Browser.CloseAsync();
        }
        MPlaywright?.Dispose();
        Logging.InfoVerbose("Chromium browser closed.");
    }
}
