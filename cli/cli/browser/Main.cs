using cli.lib;
using Microsoft.Playwright;

namespace cli.browser;

public interface IBaseBrowser
{
    // Logging Logging { get; }
    AppConfig Config { get; }
    IBrowser? Browser { get; set; }

    IPlaywright? MPlaywright { get; set; }
    bool Headless { get; set; }
    public virtual async Task Start()
    {
    }

}
