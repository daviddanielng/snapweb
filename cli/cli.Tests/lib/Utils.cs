namespace cli.Tests.lib;

public class Utils
{
    public static void CreateAndUseTempFile(string content, Action<string> testAction)
    {

        {
            var tempFilePath = Path.GetTempFileName();
            try
            {
                File.WriteAllText(tempFilePath, content);
                testAction(tempFilePath);
            }
            finally
            {
                File.Delete(tempFilePath);
            }
        }
    }
}
