using cli.lib;
namespace cli.Tests;

public class UtilsTests
{
    [Fact]
    public void Test_ReadFile_Success()
    {
        var tempFilePath = Path.GetTempFileName();
        try
        {
            File.WriteAllText(tempFilePath, "test content");
            var content = Utils.ReadFile(tempFilePath);
            Assert.Equal("test content", content);
        }
        finally
        {
            File.Delete(tempFilePath);
        }
    }
    [Fact]
    public void Test_ReadFile_FileNotFound()
    {
        var nonExistentPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Assert.Throws<FileNotFoundException>(() => Utils.ReadFile(nonExistentPath));
    }
}
