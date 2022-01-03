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
    public async Task CheckFileExistsTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repository = "RepoAutomation"; //inception!!
        string? file = "dependabot";
        string? extension = "yml";
        string? path = null;

        //Act
        SearchResult? searchResult = await DependabotAutomation.CheckForDependabotFile(base.GitHubId, base.GitHubSecret, 
            owner, repository, file,extension, path);

        //Assert
        Assert.IsNotNull(searchResult);
    }
}