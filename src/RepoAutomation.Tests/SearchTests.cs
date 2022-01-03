using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoAutomation.APIAccess;
using RepoAutomation.Helpers;
using RepoAutomation.Models;
using RepoAutomation.Tests.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RepoAutomation.Tests;

[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
[TestClass]
[TestCategory("IntegrationTests")]
public class SearchTests : BaseAPIAccessTests
{

    [TestMethod]
    public async Task CheckDependabotFileExistsTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repository = "RepoAutomation"; //inception!!
        string? file = "dependabot.yml";
        string? extension = "yml";
        string? path = ".github";

        //Act
        GitHubFile[]? searchResult = await DependabotAutomation.CheckForDependabotFile(base.GitHubId, base.GitHubSecret,
            owner, repository, path);

        //Assert
        Assert.IsNotNull(searchResult);
        Assert.IsTrue(searchResult.Length > 0);
    }

    [TestMethod]
    public async Task CheckActionFilesExistTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repository = "RepoAutomation"; //inception!!
        //string? file = null;
        //string? extension = "yml";
        string? path = ".github/workflows";

        //Act
        GitHubFile[]? searchResult = await DependabotAutomation.CheckForDependabotFile(base.GitHubId, base.GitHubSecret,
            owner, repository, path);

        //Assert
        Assert.IsNotNull(searchResult);
        Assert.IsTrue(searchResult.Length > 0);
    }

}