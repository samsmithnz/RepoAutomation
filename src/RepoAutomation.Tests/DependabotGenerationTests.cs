using GitHubActionsDotNet.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoAutomation.Helpers;
using RepoAutomation.Tests.Helpers;
using System.Collections.Generic;

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
        string yaml = GitHubActionsDotNet.Serialization.DependabotSerialization.Serialize(workingDirectory, files);


        //Assert
        string expected = @"version: 2
updates:
- package-ecosystem: nuget
  directory: /RepoAutomation/src/RepoAutomation.Tests/
  schedule: {}
- package-ecosystem: nuget
  directory: /RepoAutomation/src/RepoAutomation.Web/
  schedule: {}
- package-ecosystem: nuget
  directory: /RepoAutomation/src/RepoAutomation/
  schedule: {}
- package-ecosystem: nuget
  directory: /RepoTestProject/src/RepoTestProject.Tests/
  schedule: {}
- package-ecosystem: nuget
  directory: /RepoTestProject/src/RepoTestProject.Web/
  schedule: {}
- package-ecosystem: github-actions
  directory: /
  schedule: {}
";
        Assert.AreEqual(expected, Utility.TrimNewLines(yaml));
    }

    [TestMethod]
    public void CreateFullDependabotConfigurationTest()
    {
        //Arrange
        string workingDirectory = System.Environment.CurrentDirectory;

        //Act
        List<string> files = FileSearch.GetFilesForDirectory(workingDirectory);
        string yaml = GitHubActionsDotNet.Serialization.DependabotSerialization.Serialize(workingDirectory, files);


        //Assert
        string expected = @"";
        Assert.AreEqual(expected, Utility.TrimNewLines(yaml));
    }

}
