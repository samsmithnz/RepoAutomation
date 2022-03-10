using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoAutomation.Core.Helpers;
using RepoAutomation.Tests.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepoAutomation.Tests;

[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
[TestClass]
[TestCategory("IntegrationTests")]
public class SearchForFilesTests : BaseAPIAccessTests
{

    [TestMethod]
    public async Task CheckDependabotFileExistsTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repository = "RepoAutomation"; //inception!!
        string? file = "dependabot.yml";
        string? extension = null;
        string? path = ".github";

        //Act
        List<string>? searchResult = await GitHubFileSearch.SearchForFiles(base.GitHubId, base.GitHubSecret,
            owner, repository, file, extension, path);

        //Assert
        Assert.IsNotNull(searchResult);
        Assert.IsTrue(searchResult.Count > 0);
        Assert.AreEqual(1, searchResult.Count);
        Assert.AreEqual("dependabot.yml", searchResult[0]);
    }

    [TestMethod]
    public async Task CheckActionFilesExistTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repository = "RepoAutomation"; //inception!!
        string? file = null;
        string? extension = "yml";
        string? path = ".github/workflows";

        //Act
        List<string>? searchResult = await GitHubFileSearch.SearchForFiles(base.GitHubId, base.GitHubSecret,
            owner, repository, file, extension, path);

        //Assert
        Assert.IsNotNull(searchResult);
        Assert.IsTrue(searchResult.Count > 0);
        Assert.AreEqual(1, searchResult.Count);
        Assert.AreEqual("dotnet.yml", searchResult[0]);
    }

    [TestMethod]
    public async Task CheckGitVersionFilesExistTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repository = "RepoAutomation"; //inception!!
        string? file = "GitVersion.yml";
        string? extension = null;
        string? path = "";

        //Act
        List<string>? searchResult = await GitHubFileSearch.SearchForFiles(base.GitHubId, base.GitHubSecret,
            owner, repository, file, extension, path);

        //Assert
        Assert.IsNotNull(searchResult);
        Assert.IsTrue(searchResult.Count > 0);
        Assert.AreEqual(1, searchResult.Count);
        Assert.AreEqual("GitVersion.yml", searchResult[0]);
    }

    [TestMethod]
    public async Task GetAllWorkflowsTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repository = "RepoAutomation"; //inception!!
        string? file = null;
        string? extension = null;
        string? path = ".github/workflows";

        //Act
        List<string>? searchResult = await GitHubFileSearch.SearchForFiles(base.GitHubId, base.GitHubSecret,
            owner, repository, file, extension, path);

        //Assert
        Assert.IsNotNull(searchResult);
        Assert.IsTrue(searchResult.Count > 0);
        Assert.AreEqual(1, searchResult.Count);
        Assert.AreEqual("dotnet.yml", searchResult[0]);
    }

}