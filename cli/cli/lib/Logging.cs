public class Logging
{
    public static bool Verbose { get; set; } = false;
    public static void Info(string message)
    {
        Console.WriteLine($"[INFO] {message}");
    }
    public static void InfoVerbose(string verboseMessage, string? message = null)
    {
        if (Verbose)
        {
            Console.WriteLine($"[INFO] {verboseMessage}");
        }
        else if (message != null)
        {
            Console.WriteLine($"[INFO] {message}");
        }
    }
    public static void ErrorVerbose(string verboseMessage, string? message = null)
    {
        if (Verbose)
        {
            Console.WriteLine($"[ERROR] {verboseMessage}");
        }
        else if (message != null)
        {
            Console.WriteLine($"[ERROR] {message}");
        }
    }
    public static void Error(string message)
    {
        Console.WriteLine($"[ERROR] {message}");
    }
    public static void Warning(string message)
    {
        Console.WriteLine($"[WARNING] {message}");
    }
}

