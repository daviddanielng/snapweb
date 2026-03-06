using cli.browser;
using cli.lib;

namespace cli;


class UrlInfo
{
    public string Url { get; set; } = null!;
    public bool Pause { get; set; } = false;
    public bool IncludeFullPage { get; set; } = false;
    public string? Caption { get; set; }

    public string Domain
    {
        get
        {
            try
            {
                var uri = new Uri(Url);
                return uri.Host;
            }
            catch (UriFormatException)
            {
                Logging.Error($"Invalid URL format: {Url}");
                return "invalid_url";
            }
        }
    }
    public string Path
    {
        get
        {
            try
            {
                var uri = new Uri(Url);
                var path = uri.AbsolutePath;
                if (string.IsNullOrEmpty(path) || path == "/")
                {
                    return "root";
                }
                return path.Trim('/');
            }
            catch (UriFormatException)
            {
                Logging.Error($"Invalid URL format: {Url}");
                return "invalid_url";
            }
        }
    }
}


public class Start
{
    public static async Task Run(AppConfig config)
    {

        var outDir = Path.Combine(config.OutputDir, DateTime.Now.ToString("Run_yyyy-MM-dd_HH-mm-ss"));
        if (!await Utils.CreateDirIfNotExists(outDir))
        {
            return;
        }
        if (config.AnyHasPause)
        {
            Logging.Warning("One or more URLs are configured to pause before processing. This will not open the browser in headless mode.");
        }

        foreach (var browserConfig in config.Browsers)
        {
            switch (browserConfig.Name)
            {
                case "chromium":

                    var newDir = Path.Combine(outDir, "chromium");
                    if (await Utils.CreateDirIfNotExists(newDir))
                    {
                        var chromium = await new Chromium(config, !config.AnyHasPause).Start();
                        if (chromium.Browser == null)
                        {
                            Logging.Error("Failed to start Chromium browser.");
                            return;
                        }
                        await Roll(config, newDir, chromium.Browser);
                        await chromium.Dispose();
                    }
                    else
                    {
                        Logging.Error($"Failed to create output directory for Chromium: {newDir}");
                    }

                    break;
                case "firefox":
                    throw new NotImplementedException("Firefox support is not implemented yet.");

                case "webkit":
                    throw new NotImplementedException("WebKit support is not implemented yet.");

                default:
                    Logging.Error($"Unsupported browser specified in config: {browserConfig.Name}");
                    break;
            }
        }
        Logging.Info("Processing completed.");
        Logging.Raw(new Dictionary<string, string>
        {
            {"Type","Done"},
            { "Output", outDir }
        });


    }
    private static async Task<Dictionary<string, List<UrlInfo>>?> Roll(AppConfig config, string outDir, Microsoft.Playwright.IBrowser Browser)
    {
        var data = new Dictionary<string, List<UrlInfo>>();
        var page = await Browser.NewPageAsync();

        foreach (var screenConfig in config.SaveScreenSizes)
        {
            foreach (var url in config.Urls)
            {
                var info = new UrlInfo
                {
                    Url = url.Url,
                    Pause = url.Pause,
                    IncludeFullPage = url.FullPage,
                    Caption = url.Caption
                };
                var path = await GenerateScreenShotFileName(info, outDir, screenConfig.Width, screenConfig.Height);
                if (path == null)
                {
                    Logging.Error($"Failed to generate file path for URL: {url.Url} at screen size {screenConfig.Width}x{screenConfig.Height}");
                    return null;
                }
                Logging.InfoVerbose($"Processing URL: {url.Url} at screen size {screenConfig.Width}x{screenConfig.Height}");
                await page.SetViewportSizeAsync(screenConfig.Width, screenConfig.Height);
                try
                {
                    await page.GotoAsync(url.Url, new Microsoft.Playwright.PageGotoOptions
                    {
                        Timeout = config.BrowserTimeout,
                        WaitUntil = Microsoft.Playwright.WaitUntilState.NetworkIdle
                    });
                    if (url.Pause)
                    {
                        Logging.InfoVerbose($"Pausing before processing URL: {url.Url}");
                        Console.WriteLine($"Press any key to continue processing URL: {url.Url}...");
                        Console.ReadKey();
                    }
                    await page.ScreenshotAsync(new Microsoft.Playwright.PageScreenshotOptions
                    {
                        Path = path + config.ScreenshotOptions.FileExtension,
                        FullPage = url.FullPage
                    });
                }
                catch (Exception ex)
                {
                    Logging.ErrorVerbose($"Failed to navigate to URL: {url.Url}. Exception: {ex}", $"Failed to navigate to URL: {url.Url}. Exception: {ex.Message}");
                    continue;
                }

            }
        }


        return data;
    }

    private static async Task<string?> GenerateScreenShotFileName(UrlInfo info, string outDir, int width, int height)
    {
        if (!await Utils.CreateDirIfNotExists(Path.Combine(outDir, info.Domain)))
        {
            return null;
        }
        var dirPath = Path.Combine(outDir, info.Domain, info.Path.Replace('/', ':'));
        if (!await Utils.CreateDirIfNotExists(dirPath))
        {
            return null;
        }
        var fileName = $"{width}x{height}";
        var filePath = Path.Combine(dirPath, fileName);


        return filePath;
    }


}
