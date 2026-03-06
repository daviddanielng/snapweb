namespace cli.Tests;

using cli.lib;
using Xunit;

public class ReadArgsTests
{
    [Fact]
    public void Test_XmlOutputSet()
    {
        lib.Utils.CreateAndUseTempFile("test content", (tempFilePath) =>
        {
            var arg = new ReadArgument();
            Assert.True(arg.Read(["--output", "xml", "--config", tempFilePath]), $"Expected Read to return true for args: --output xml --config {tempFilePath}");
            Assert.Equal(OutputFormat.Xml, arg.Output);
        });

    }
    [Fact]
    public void Test_TextOutputSet()
    {
        lib.Utils.CreateAndUseTempFile("test content", (tempFilePath) =>
        {
            var arg = new ReadArgument();
            Assert.True(arg.Read(["--output", "text", "--config", tempFilePath]), $"Expected Read to return true for args: --output text --config {tempFilePath}");
            Assert.Equal(OutputFormat.Text, arg.Output);
        });

    }
    [Fact]
    public void Test_JsonOutputSet()
    {
        lib.Utils.CreateAndUseTempFile("test content", (tempFilePath) =>
        {
            var arg = new ReadArgument();
            Assert.True(arg.Read(["--output", "json", "--config", tempFilePath]), $"Expected Read to return true for args: --output json --config {tempFilePath}");
            Assert.Equal(OutputFormat.Json, arg.Output);
        });

    }

    [Fact]
    public void Test_ConfigValid()
    {
        lib.Utils.CreateAndUseTempFile("test content", (tempFilePath) =>
        {
            var arg = new ReadArgument();
            Assert.True(arg.Read(["--config", tempFilePath]), $"Expected Read to return true for args: --config {tempFilePath}");
            Assert.Equal(tempFilePath, arg.ConfigFilePath);
        });
    }

    [Fact]
    public void Test_MissingConfigPath()
    {
        var arg = new ReadArgument();
        Assert.False(arg.Read(["--config"]));
    }
    [Fact]
    public void Test_InvalidConfigPath()
    {
        var arg = new ReadArgument();
        Assert.False(arg.Read(["--config", "nonexistentfile.txt"]));
    }
    [Fact]
    public void Test_ConfigFilePathIsSet()
    {
        lib.Utils.CreateAndUseTempFile("test content", (tempFilePath) =>
        {
            var arg = new ReadArgument();
            Assert.True(arg.Read(["--config", tempFilePath]), $"Expected Read to return true for args: --config {tempFilePath}");
            Assert.Equal(tempFilePath, arg.ConfigFilePath);
        });
    }
    [Fact]
    public void Test_VerboseFlagIsSet()
    {
        lib.Utils.CreateAndUseTempFile("test content", (tempFilePath) =>
        {
            var arg = new ReadArgument();
            Assert.True(arg.Read(["--verbose", "--config", tempFilePath]));
            Assert.True(arg.Verbose);
        });
    }

    [Fact]
    public void Test_VerboseFlagIsNotSet()
    {
        lib.Utils.CreateAndUseTempFile("test content", (tempFilePath) =>
        {
            var arg = new ReadArgument();
            Assert.True(arg.Read(["--config", tempFilePath]));
            Assert.False(arg.Verbose);
        });
    }



    public static TheoryData<CustomTypeWithString<string[]>> InvalidArgsTestCases =>
    [
        new CustomTypeWithString<string[]>
        {
            Value = [  ],
            Message = "Expected Read to return false for empty args"
        },
        new CustomTypeWithString<string[]>
        {
            Value = [ "--config" ],
            Message = "Expected Read to return false for args: --config"
        },
        new CustomTypeWithString<string[]>
        {
            Value = [ "--verbose", "--config" ],
            Message = "Expected Read to return false for args: --verbose --config"
        },
        new CustomTypeWithString<string[]>
        {
            Value = [ "--unknown" ],
            Message = "Expected Read to return false for args: --unknown"
        },
        new CustomTypeWithString<string[]>
        {
            Value = [ "--config", "--verbose" ],
            Message = "Expected Read to return false for args: --config --verbose"
        },
        new CustomTypeWithString<string[]>
        {
            Value = [ "--verbose", "--verbose", "--config", "config.json" ],
            Message = "Expected Read to return false for args: --verbose --verbose --config config.json"
        },
        new CustomTypeWithString<string[]>
        {
            Value = [ "--lang" ],
            Message = "Expected Read to return false for args: --lang"
        },
        new CustomTypeWithString<string[]>
        {
            Value = [ "--lang", "--config", "config.json" ],
            Message = "Expected Read to return false for args: --lang --config config.json"
        },
        new CustomTypeWithString<string[]>
        {
            Value = [ "--config", "config.json", "--lang" ],
            Message = "Expected Read to return false for args: --config config.json --lang"
        },
        new CustomTypeWithString<string[]>
        {
            Value = [ "--output", "invalidformat" ],
            Message = "Expected Read to return false for args: --output invalidformat"
        },
        new CustomTypeWithString<string[]>        {
            Value = [ "--output" ],
            Message = "Expected Read to return false for args: --output"
        },
    ];

    [Theory]
    [MemberData(nameof(InvalidArgsTestCases))]
    public void Read_WithInvalidArgs_ReturnsFalse(CustomTypeWithString<string[]> testCase)
    {
        var arg = new ReadArgument();
        Assert.False(arg.Read(testCase.Value), testCase.Message);
    }
}
