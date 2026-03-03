namespace cli.Tests;

using cli.Tests.lib;
using Xunit;


public class AppConfigTests
{
    [Fact]
    public void Test_ReadJson_ValidConfig()
    {
        Utils.CreateAndUseTempFile(@"{
            ""version"": ""0.1.0"",
            ""browserTimeout"": 50000,
            ""browsers"": [
                {
                    ""name"": ""chromium"",
                    ""executablePath"": ""/usr/bin/chromium""
                }
            ]
        }", (tempFilePath) =>
        {
            var config = new cli.lib.AppConfig().ReadJson(tempFilePath);
            Assert.NotNull(config);
            Assert.Equal("0.1.0", config.Version);
            Assert.Equal(50000, config.BrowserTimeout);
            Assert.Single(config.Browsers);
        });

    }
}
