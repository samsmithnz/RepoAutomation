using GitHubActionsDotNet.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoAutomation.Tests.Helpers;
using System.Collections.Generic;
using GitHubActionsDotNet.Serialization;

namespace RepoAutomation.Tests;

[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
[TestClass]
[TestCategory("IntegrationTests")]
public class DependabotGenerationTests
{

    [TestMethod]
    public void CreateEmptyDependabotConfigurationTest()
    {
        //Arrange
        string workingDirectory = System.Environment.CurrentDirectory;

        //Act
        List<string> files = FileSearch.GetFilesForDirectory(workingDirectory);
        string yaml = DependabotSerialization.Serialize(workingDirectory, files);


        //Assert
        string expected = @"version: 2
updates:
- package-ecosystem: nuget
  directory: /RepoAutomation/src/RepoAutomation.Tests/
- package-ecosystem: nuget
  directory: /RepoAutomation/src/RepoAutomation.Web/
- package-ecosystem: nuget
  directory: /RepoAutomation/src/RepoAutomation/
- package-ecosystem: nuget
  directory: /RepoTestProject/src/RepoTestProject.Tests/
- package-ecosystem: nuget
  directory: /RepoTestProject/src/RepoTestProject.Web/
- package-ecosystem: github-actions
  directory: /";
        Assert.AreEqual(expected, Utility.TrimNewLines(yaml));
    }

    [TestMethod]
    public void CreateFullDependabotConfigurationTest()
    {
        //Arrange
        string workingDirectory = System.Environment.CurrentDirectory;

        //Act
        List<string> files = FileSearch.GetFilesForDirectory(workingDirectory);
        string yaml = DependabotSerialization.Serialize(workingDirectory, files,
            "daily",
            "06:00",
            "America/New_York",
            new() { "samsmithnz" },
            20,
            true);

        //Assert
        string expected = @"version: 2
updates:
- package-ecosystem: nuget
  directory: /RepoAutomation/src/RepoAutomation.Tests/
  schedule:
    interval: daily
    time: 06:00
    timezone: America/New_York
  assignees:
  - samsmithnz
  open-pull-requests-limit: 20
- package-ecosystem: nuget
  directory: /RepoAutomation/src/RepoAutomation.Web/
  schedule:
    interval: daily
    time: 06:00
    timezone: America/New_York
  assignees:
  - samsmithnz
  open-pull-requests-limit: 20
- package-ecosystem: nuget
  directory: /RepoAutomation/src/RepoAutomation/
  schedule:
    interval: daily
    time: 06:00
    timezone: America/New_York
  assignees:
  - samsmithnz
  open-pull-requests-limit: 20
- package-ecosystem: nuget
  directory: /RepoTestProject/src/RepoTestProject.Tests/
  schedule:
    interval: daily
    time: 06:00
    timezone: America/New_York
  assignees:
  - samsmithnz
  open-pull-requests-limit: 20
- package-ecosystem: nuget
  directory: /RepoTestProject/src/RepoTestProject.Web/
  schedule:
    interval: daily
    time: 06:00
    timezone: America/New_York
  assignees:
  - samsmithnz
  open-pull-requests-limit: 20
- package-ecosystem: github-actions
  directory: /
  schedule:
    interval: daily
    time: 06:00
    timezone: America/New_York
  assignees:
  - samsmithnz
  open-pull-requests-limit: 20";
        Assert.AreEqual(expected, Utility.TrimNewLines(yaml));
    }

}
