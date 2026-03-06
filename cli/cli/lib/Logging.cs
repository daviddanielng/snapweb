namespace cli.lib;

public enum OutputFormat
{
    Text,
    Json,
    Xml
}
public class Logging
{
    public static bool Verbose { get; set; } = false;
    public static OutputFormat Output { get; set; }
    public static void Info(string message)
    {
        Dictionary<string, string> data = [];
        data.Add("Info", message);
        Say(data);
    }
    public static void Raw(Dictionary<string, string> message)
    {
        Say(message);
    }
    /// <summary>
    /// Logs an informational message, using a verbose variant when verbose mode is enabled.
    /// </summary>
    /// <param name="verboseMessage">
    /// The message to log when <c>Verbose</c> is <see langword="true"/>.
    /// </param>
    /// <param name="message">
    /// An optional non-verbose fallback message to log when <c>Verbose</c> is <see langword="false"/>.
    /// If <see langword="null"/>, no message is logged in non-verbose mode.
    /// </param>
    public static void InfoVerbose(string verboseMessage, string? message = null)
    {
        Dictionary<string, string> data = [];
        if (Verbose)
        {

            data.Add("Info", verboseMessage);
            Say(data);
        }
        else if (message != null)
        {
            data.Add("Info", message);
            Say(data);
        }
    }
    public static void ErrorVerbose(string verboseMessage, string? message = null)
    {
        Dictionary<string, string> data = [];

        if (Verbose)
        {
            data.Add("Error", verboseMessage);
            Say(data);
        }
        else if (message != null)
        {
            data.Add("Error", message);
            Say(data);
        }
    }
    public static void Error(string message)
    {
        Dictionary<string, string> data = [];
        data.Add("Error", message);
        Say(data);
    }
    public static void Warning(string message)
    {
        Dictionary<string, string> data = [];
        data.Add("Warning", message);
        Say(data);
    }
    private static void Say(Dictionary<string, string> message)
    {
        switch (Output)
        {
            case OutputFormat.Text:
                Console.WriteLine(string.Join(", ", message.Select(kv => $"{kv.Key}: {kv.Value}")));
                break;
            case OutputFormat.Json:
                var json = System.Text.Json.JsonSerializer.Serialize(new { data = message });
                Console.WriteLine(json);
                break;
            case OutputFormat.Xml:
                var xml = new System.Xml.Linq.XElement("data", message.Select(kv => new System.Xml.Linq.XElement(kv.Key, kv.Value))).ToString();
                Console.WriteLine(xml);
                break;
        }
    }
}

