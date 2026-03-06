using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using NJsonSchema;

namespace cli.lib;


public class UrlConfig
{
    [Required, RegularExpression(@"^(https?://)?([\w-]+(\.[\w-]+)+)(/[\w-./?%&=]*)?$", ErrorMessage = "Invalid URL format")]
    public string Url { get; set; } = null!;
    public bool Pause { get; set; } = false;
    public bool IncludeFullPage { get; set; } = false;
    public string? Caption { get; set; }
    public bool Validate()
    {



        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(this);

        if (!Validator.TryValidateObject(this, validationContext, validationResults, true))
        {
            foreach (var validationResult in validationResults)
            {
                Logging.Error($"URL validation error: {validationResult.ErrorMessage} for URL {this.Url}");
            }
            return false;
        }


        return true;
    }
}
public class ScreenshotConfig
{
    [Required, Range(1, 100, ErrorMessage = "Quality must be between 1 and 100")]

    public int Quality { get; set; } = 100;
    [Required, RegularExpression(@"^(?i)(png|jpeg)$", ErrorMessage = "Format must be either 'png' or 'jpeg'")]
    public string Format { get; set; } = "png";
    public bool Validate()
    {
        if (Quality == 100 && Format.Equals("jpeg", StringComparison.OrdinalIgnoreCase))
        {
            Logging.Warning("Quality will not be in effect when using JPEG format.");
        }


        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(this);

        if (!Validator.TryValidateObject(this, validationContext, validationResults, true))
        {
            foreach (var validationResult in validationResults)
            {
                Logging.Error($"Screenshot validation error: {validationResult.ErrorMessage} for format {this.Format} with quality {this.Quality}");
            }
            return false;
        }


        return true;
    }
}
public class BrowserConfig
{
    [Required, RegularExpression(@"^(?i)(chromium|firefox|webkit)$", ErrorMessage = "Browser name must be either 'chromium', 'firefox', or 'webkit'")]

    public string Name { get; set; } = null!;
    [Required, MinLength(3, ErrorMessage = "Executable path cannot be empty")]
    public string ExecutablePath { get; set; } = null!;
    public bool Validate()
    {
        if (Name.Equals("webkit", StringComparison.OrdinalIgnoreCase))

        {
            Logging.Warning("Browser 'webkit' has not been fully tested and may not work correctly. Consider using 'chromium' or 'firefox' instead.");
        }
        var browserValidationResults = new List<ValidationResult>();
        var browserValidationContext = new ValidationContext(this);

        if (!Validator.TryValidateObject(this, browserValidationContext, browserValidationResults, true))
        {
            foreach (var validationResult in browserValidationResults)
            {
                Logging.Error($"Browser validation error: {validationResult.ErrorMessage} for browser {this.Name}");
            }
            return false;
        }
        if (!File.Exists(this.ExecutablePath))
        {
            Logging.Error($"Executable path not found: {this.ExecutablePath} for browser {this.Name}");
            return false;
        }

        return true;
    }
}
public class ScreenConfig
{
    [Required, Range(1, int.MaxValue, ErrorMessage = "Screen width must be greater than 0")]
    public int Width { get; set; }
    [Required, Range(1, int.MaxValue, ErrorMessage = "Screen height must be greater than 0")]
    public int Height { get; set; }

    public bool Validate()
    {

        var browserValidationResults = new List<ValidationResult>();
        var browserValidationContext = new ValidationContext(this);

        if (!Validator.TryValidateObject(this, browserValidationContext, browserValidationResults, true))
        {
            foreach (var validationResult in browserValidationResults)
            {
                Logging.Error($"Screen validation error: {validationResult.ErrorMessage} for screen {this.Width}x{this.Height}");
            }
            return false;
        }


        return true;
    }
}
public class AppConfig
{
    public readonly string AppVersion = "0.1.0";
    public readonly string MinConfigVersion = "0.1.0";
    public readonly string MaxConfigVersion = "0.1.0";

    public JsonSerializerOptions JsonOptions { get; set; } = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };
    [Required, RegularExpression(@"^\d+\.\d+\.\d+$", ErrorMessage = "Version must be in the format x.y.z")]
    public string Version { get; set; } = null!;
    public int BrowserTimeout { get; set; } = 30000;

    [Required, MinLength(1, ErrorMessage = "At least one browser configuration is required"), MaxLength(3, ErrorMessage = "No more than three browser configurations are allowed")]
    public List<BrowserConfig> Browsers { get; set; } = [];
    [Required, MinLength(1, ErrorMessage = "At least one screen configuration is required"), MaxLength(15, ErrorMessage = "No more than fifteen screen configurations are allowed")]
    public List<ScreenConfig> SaveScreenSizes { get; set; } = [];
    [Required, MinLength(1, ErrorMessage = "At least one URL configuration is required"), MaxLength(150, ErrorMessage = "No more than 150 URL configurations are allowed")]
    public List<UrlConfig> Urls { get; set; } = [];
    [Required, MinLength(1, ErrorMessage = "Output directory cannot be empty")]
    public string OutputDir { get; set; } = null!;

    public ScreenshotConfig ScreenshotOptions { get; set; } = new();
    public bool AnyHasPause => Urls.Any(url => url.Pause);
    public AppConfig? ReadJson(string path)
    {
        Logging.InfoVerbose($"Reading config at {path}", "Reading config...");
        try
        {
            if (!File.Exists(path))
            {
                Logging.Error($"Config file not found: {path}");
                return null;
            }

            AppConfig config = JsonSerializer.Deserialize<AppConfig>(File.ReadAllText(path), JsonOptions)!;
            if (!config.Validate())
            {

                return null;
            }


            if (!ValidateVersion(config.MinConfigVersion, config.MaxConfigVersion, config.Version))
            {

                return null;
            }
            Logging.InfoVerbose("Config loaded successfully");
            return config;
        }
        catch (Exception ex)
        {
            Logging.ErrorVerbose($"Exception loading config file: {ex}", $"Error loading config file: {ex.Message}");

            return null;
        }


    }
    private static bool ValidateVersion(string MinConfigVersion, string MaxConfigVersion, string ConfigVersion)
    {
        Version configVer = new(ConfigVersion);
        Version minConfigVer = new(MinConfigVersion);
        Version maxConfigVer = new(MaxConfigVersion);
        if (configVer < minConfigVer)
        {
            Logging.Error($"Config version {configVer} is less than minimum required version {minConfigVer}");
            return false;
        }
        if (configVer > maxConfigVer)
        {
            Logging.Error($"Config version {configVer} is greater than maximum allowed version {maxConfigVer}. Consider updating the application.");
            return false;
        }
        return true;
    }
    private bool Validate()
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(this);

        if (!Validator.TryValidateObject(this, validationContext, validationResults, true))
        {
            foreach (var validationResult in validationResults)
            {
                Logging.Error($"Validation error: {validationResult.ErrorMessage}");
            }
            return false;
        }
        if (!Directory.Exists(this.OutputDir))
        {
            Logging.Error($"Output directory not found: {this.OutputDir}");
            return false;
        }
        var browserNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var browser in this.Browsers)
        {
            if (!browserNames.Add(browser.Name))
            {
                Logging.Error($"Duplicate browser name found: {browser.Name}");
                return false;
            }
            if (!browser.Validate())
            {
                return false;
            }


        }
        var screenList = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var screen in this.SaveScreenSizes)
        {
            string screenKey = $"{screen.Width}x{screen.Height}";
            if (!screenList.Add(screenKey))
            {
                Logging.Error($"Duplicate screen configuration found: {screenKey}");
                return false;
            }
            if (!screen.Validate())
            {
                return false;
            }
        }
        if (!ScreenshotOptions.Validate())
        {
            return false;
        }
        return true;
    }
    public string SettingsToWords()
    {
        var imageCount = this.Browsers.Count * this.SaveScreenSizes.Count * this.Urls.Count;
        return $"Config version {Version} with {Browsers.Count} browsers and {SaveScreenSizes.Count} screen sizes. Output  directory: {OutputDir}. Screenshot format: {ScreenshotOptions.Format} with quality {ScreenshotOptions.Quality}. Browser timeout: {BrowserTimeout}ms. Total images to capture: {imageCount} from {Urls.Count} URLs.";
    }
    public static string GenerateJsonSchema()
    {
        var schema = JsonSchema.FromType<AppConfig>();
        return schema.ToJson();
    }
}
