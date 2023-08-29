using GitHubActionsDotNet.Helpers;
using GitHubActionsDotNet.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoAutomation.Tests.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace RepoAutomation.Tests;

[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
[TestClass]
[TestCategory("IntegrationTests")]
public class DependabotTests
{

    [TestMethod]
    public void CreateEmptyDependabotConfigurationTest()
    {
        //Arrange
        string workingDirectory = System.Environment.CurrentDirectory;
        string? projectDirectory = Directory.GetParent(workingDirectory)?.Parent?.Parent?.Parent?.Parent?.FullName;
        projectDirectory += "\\dependabotSamples";
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            projectDirectory = projectDirectory?.Replace("\\", "/");
        }

        //Act
        List<string> files = FileSearch.GetFilesForDirectory(projectDirectory);
        string yaml = DependabotSerialization.Serialize(projectDirectory, files);


        //Assert
        string expected = @"version: 2
updates:
- package-ecosystem: nuget
  directory: /dotnet/
- package-ecosystem: github-actions
  directory: /";
        Assert.AreEqual(expected, Utility.TrimNewLines(yaml));
    }

    [TestMethod]
    public void CreateFullDependabotConfigurationTest()
    {
        //Arrange
        string workingDirectory = System.Environment.CurrentDirectory;
        string? projectDirectory = Directory.GetParent(workingDirectory)?.Parent?.Parent?.Parent?.Parent?.FullName;
        projectDirectory += "\\dependabotSamples";
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            projectDirectory = projectDirectory?.Replace("\\", "/");
        }

        //Act
        List<string> files = FileSearch.GetFilesForDirectory(projectDirectory);
        string yaml = DependabotSerialization.Serialize(projectDirectory, files,
            "daily",
            "06:00",
            "America/New_York",
            new() { "samsmithnz" },
            20,
            true,
            "core",
            new string[] { "*" },
            new string[] { "minor", "patch" });

        //Assert
        string expected = @"version: 2
updates:
- package-ecosystem: nuget
  directory: /dotnet/
  schedule:
    interval: daily
    time: ""06:00""
    timezone: America/New_York
  assignees:
  - samsmithnz
  open-pull-requests-limit: 20
  groups:
    core:
      patterns: [""*""]
      update-types: [""minor"", ""patch""]
- package-ecosystem: github-actions
  directory: /
  schedule:
    interval: daily
    time: ""06:00""
    timezone: America/New_York
  assignees:
  - samsmithnz
  open-pull-requests-limit: 20
  groups:
    actions:
      patterns: [""*""]
      update-types: [""minor"", ""patch""]";
        Assert.AreEqual(expected, Utility.TrimNewLines(yaml));
    }

}
