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
        string yaml = GitHubActionsDotNet.Serialization.DependabotSerialization.Serialize(workingDirectory, files);


        //Assert
        string expected = @"";
        Assert.AreEqual(expected, Utility.TrimNewLines(yaml));
    }

}
