using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoAutomation.Tests.Helpers;

namespace RepoAutomation.Tests;

[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
[TestClass]
[TestCategory("IntegrationTests")]
public class CommandTests
{

    [TestMethod]
    public void DotNetHelpTest()
    {
        //Arrange
        string command = "dotnet";
        string arguments = "-h";

        //Act
        string result = CommandLine.RunCommand(command, arguments);

        //Assert
        string expected = @".NET SDK (6.0.101)
Usage: dotnet [runtime-options] [path-to-application] [arguments]

Execute a .NET application.";
        Assert.AreEqual(expected, Utility.TakeNLines(result, 4));
    }

    [TestMethod]
    public void GitHelpTest()
    {
        //Arrange
        string command = "git";
        string arguments = "help";

        //Act
        string result = CommandLine.RunCommand(command, arguments);

        //Assert
        string expected = @"usage: git [--version] [--help] [-C <path>] [-c <name>=<value>]
           [--exec-path[=<path>]] [--html-path] [--man-path] [--info-path]
           [-p | --paginate | -P | --no-pager] [--no-replace-objects] [--bare]
           [--git-dir=<path>] [--work-tree=<path>] [--namespace=<name>]
           [--super-prefix=<path>] [--config-env=<name>=<envvar>]
           <command> [<args>]";
        Assert.AreEqual(expected, Utility.TakeNLines(result,6));
    }

}
