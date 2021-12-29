using GitHubActionsDotNet.Models;
using GitHubActionsDotNet.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoAutomation.Helpers;
using RepoAutomation.Tests.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RepoAutomation.Tests;

[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
[TestClass]
[TestCategory("IntegrationTests")]
public class ActionGenerationTests
{

    [TestMethod]
    public void CreateEmptyAction()
    {
        //Arrange
        string projectName = "TestProject";
        bool includeTestProject = false;
        bool includeClassLibraryProject = false;
        bool includeWebProject = false;

        //Act
        string yaml = GitHubActionsAutomation.CreateActionYaml(projectName,
            includeTestProject,
            includeClassLibraryProject,
            includeWebProject);

        //Assert
        string expected = @"";
        Assert.AreEqual(expected, Utility.TrimNewLines(yaml));
    }

}
