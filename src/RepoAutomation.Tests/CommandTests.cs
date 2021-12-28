using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoAutomation.Tests.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;

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
        Assert.AreEqual(expected, Utility.TakeNLines(result, 6));
    }

    [TestMethod]
    public void CreateDotNetSolutionAndProjectsTest()
    {
        //Arrange
        string repoLocation = "https://github.com/samsmithnz/RepoAutomation";
        string projectName = "RepoTestProject";
        string workingDirectory = Environment.CurrentDirectory;

        //Act
        string log = ProjectAutomation.SetupProject(repoLocation, projectName, workingDirectory);
        //Cleanup
        Directory.Delete(workingDirectory + "/src", true);

        //Assert
        Assert.IsNotNull(log);
    }

    [TestMethod]
    public async Task RepoAutomationInceptionCommandLineTest()
    {
        //Arrange

        //Act
        string result = "";
        using (StringWriter sw = new())
        {
            Console.SetOut(sw);
            await Program.Main(new string[] { "" });
            result = sw.ToString();
        }

        //Assert
        Assert.IsNotNull(result);
        string expected = @"Running GitHub url: https://api.github.com/repos/samsmithnz/RepoAutomation
Hello world samsmithnz/RepoAutomation
";
        Assert.AreEqual(expected, result);
    }


    [TestMethod]
    public void RepoAutomationInceptionWithArgsCommandLineTest()
    {
        //Arrange
        string command = "RepoAutomation";
        string arguments = "-d " + Environment.CurrentDirectory;

        //Act
        string result = CommandLine.RunCommand(command, arguments);

        //Assert
        Assert.IsNotNull(result);
        string expected = @"Running GitHub url: https://api.github.com/repos/samsmithnz/RepoAutomation
Hello world samsmithnz/RepoAutomation
";
        Assert.AreEqual(expected, result);
    }
}
