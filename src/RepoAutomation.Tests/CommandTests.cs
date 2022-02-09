using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoAutomation.Core.Helpers;
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
        string result = RepoAutomation.Core.Helpers.CommandLine.RunCommand(command, arguments);

        //Assert
        string expected = @"Usage: dotnet [runtime-options] [path-to-application] [arguments]

Execute a .NET application.";
        Assert.AreEqual(expected, Utility.TakeNLines(result, 3));
    }

    [TestMethod]
    public void GitHelpTest()
    {
        //Arrange
        string command = "git";
        string arguments = "help";

        //Act
        string result = RepoAutomation.Core.Helpers.CommandLine.RunCommand(command, arguments);

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
    public void CloneProjectsTest()
    {
        //Arrange
        string repoLocation = "https://github.com/samsmithnz/RepoAutomation";
        string projectName = "RepoTestProject";
        string workingDirectory = Environment.CurrentDirectory;

        //Act
        string log = DotNetAutomation.CloneRepo(repoLocation, projectName, workingDirectory);

        //Assert
        Assert.IsNotNull(log);
    }

    [TestMethod]
    public void CreateDotNetSolutionAndProjectsTest()
    {
        //Arrange
        string projectName = "RepoTestProject";
        string workingDirectory = Environment.CurrentDirectory;
        string projectTypes = "mstest,mvc";

        //Act
        string log = DotNetAutomation.SetupDotnetProjects(projectName, workingDirectory,
            projectTypes);

        //Assert
        Assert.IsNotNull(log);

        //Cleanup
        if (Directory.Exists(workingDirectory + "/src") == true)
        {
            Directory.Delete(workingDirectory + "/src", true);
        }
    }

    [TestMethod]
    public async Task RepoAutomationInceptionCommandLineTest()
    {
        //Arrange
        string[] arguments = new string[] { "-o", "samsmithnz", "-r", "RepoAutomation" };

        //Act
        string result = "";
        using (StringWriter sw = new())
        {
            Console.SetOut(sw);
            await Program.Main(arguments);
            result = sw.ToString();
        }

        //Assert
        Assert.IsNotNull(result);
        string workingDirectory = Environment.CurrentDirectory;
        string expected = @"Cloning repo 'RepoAutomation' to " + workingDirectory;
        Assert.AreEqual(expected, Utility.TakeNLines(result, 1));
    }

    [TestMethod]
    public void RepoAutomationInceptionWithArgsCommandLineTest()
    {
        //Arrange
        string command = "RepoAutomation";
        string arguments = "-d " + Environment.CurrentDirectory +
            " -o samsmithnz -r RepoAutomation";

        //Act
        string result = RepoAutomation.Core.Helpers.CommandLine.RunCommand(command, arguments);

        //Assert
        Assert.IsNotNull(result);
        string workingDirectory = Environment.CurrentDirectory;
        string expected = @"Cloning repo 'RepoAutomation' to " + workingDirectory;
        Assert.AreEqual(expected, Utility.TakeNLines(result, 1));
    }
}
