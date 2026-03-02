namespace cli.Tests;

using cli.lib;
using Xunit;

public class ReadArgsTests
{
    [Fact]
    public void Test_WithTempFile()
    {
        var tempFilePath = Path.GetTempFileName();
        try
        {
            File.WriteAllText(tempFilePath, "test content");
            var arg = new ReadArgument();
            Assert.True(arg.Read(["--config", tempFilePath]));
        }
        finally
        {
            File.Delete(tempFilePath);
        }
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
        var tempFilePath = Path.GetTempFileName();
        try
        {
            File.WriteAllText(tempFilePath, "test content");
            var arg = new ReadArgument();
            Assert.True(arg.Read(["--config", tempFilePath]));
            Assert.Equal(tempFilePath, arg.ConfigFilePath);
        }
        finally
        {
            File.Delete(tempFilePath);
        }
    }
    [Fact]
    public void Test_VerboseFlagIsSet()
    {
        var tempFilePath = Path.GetTempFileName();
        try
        {
            File.WriteAllText(tempFilePath, "test content");
            var arg = new ReadArgument();
            Assert.True(arg.Read(["--verbose", "--config", tempFilePath]));
            Assert.True(arg.Verbose);
        }
        finally
        {
            File.Delete(tempFilePath);
        }
    }

    [Fact]
    public void Test_VerboseFlagIsNotSet()
    {
        var tempFilePath = Path.GetTempFileName();
        try
        {
            File.WriteAllText(tempFilePath, "test content");
            var arg = new ReadArgument();
            Assert.True(arg.Read(["--config", tempFilePath]));
            Assert.False(arg.Verbose, "Expected Verbose to be false when --verbose flag is not provided");
        }
        finally
        {
            File.Delete(tempFilePath);
        }
    }

    [Fact]
    public void Test_LanguageIsSet()
    {
        var tempFilePath = Path.GetTempFileName();
        try
        {
            File.WriteAllText(tempFilePath, "test content");
            var arg = new ReadArgument();
            Assert.True(arg.Read(["--lang", "fr", "--config", tempFilePath]));
            Assert.Equal("fr", arg.Language);
        }
        finally
        {
            File.Delete(tempFilePath);
        }

    }
    [Fact]
    public void Test_LanguageFlagIsNotSet()
    {
        var tempFilePath = Path.GetTempFileName();
        try
        {
            File.WriteAllText(tempFilePath, "test content");
            var arg = new ReadArgument();
            Assert.True(arg.Read(["--config", tempFilePath]));
            Assert.Equal("en", arg.Language);
        }
        finally
        {
            File.Delete(tempFilePath);
        }
    }
    [Fact]
    public void Test_LanguageDefaultsToEnglish()
    {
        var tempFilePath = Path.GetTempFileName();
        try
        {
            File.WriteAllText(tempFilePath, "test content");
            var arg = new ReadArgument();
            Assert.True(arg.Read(["--config", tempFilePath]));
            Assert.Equal("en", arg.Language);
        }
        finally
        {
            File.Delete(tempFilePath);
        }
    }
    [Fact]
    public void Test_LanguageIsInvalid()
    {
        var tempFilePath = Path.GetTempFileName();
        try
        {
            File.WriteAllText(tempFilePath, "test content");
            var arg = new ReadArgument();
            Assert.False(arg.Read(["--lang", "invalid-lang", "--config", tempFilePath]));
        }
        finally
        {
            File.Delete(tempFilePath);
        }
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
    ];

    [Theory]
    [MemberData(nameof(InvalidArgsTestCases))]
    public void Read_WithInvalidArgs_ReturnsFalse(CustomTypeWithString<string[]> testCase)
    {
        var arg = new ReadArgument();
        Assert.False(arg.Read(testCase.Value), testCase.Message);
    }
}
