namespace cli.cli.Tests;

using global::cli.lib;
using Xunit;

public class ReadArgsTests
{
    [Fact]
    public void AllCorrect()
    {
        var arg = new ReadArgument();
        Assert.True(arg.Read(["--file", "config.json"]), "Expected ReadArgument.Read to return true when a config file is provided.");

    }
    [Fact]
    public void AllWrong()
    {
        var arg = new ReadArgument();
        Assert.False(arg.Read([]), "Expected ReadArgument.Read to return false when no config file is provided.");
        Assert.False(arg.Read(["--file"]), "Expected ReadArgument.Read to return false when --file is provided without a file path.");
        Assert.False(arg.Read(["--file", "--verbose"]), "Expected ReadArgument.Read to return false when --file is provided without a file path and --verbose is provided.");

    }
}
