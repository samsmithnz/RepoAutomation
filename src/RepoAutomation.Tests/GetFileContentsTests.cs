using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoAutomation.Core.Helpers;
using RepoAutomation.Core.Models;
using RepoAutomation.Tests.Helpers;
using System.Threading.Tasks;

namespace RepoAutomation.Tests;

[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
[TestClass]
[TestCategory("IntegrationTests")]
public class GetFileContentsTests : BaseAPIAccessTests
{

    [TestMethod]
    public async Task GetDependabotFileContentsTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repository = "RepoAutomationUnitTests";
        string? filePath = ".github/dependabot.yml";

        //Act
        GitHubFile? fileResult = await GitHubFiles.GetFileContents(base.GitHubId, base.GitHubSecret,
            owner, repository, filePath);

        //Assert
        Assert.IsNotNull(fileResult);
        Assert.AreEqual("dependabot.yml", fileResult.name);
        Assert.IsNotNull(fileResult.content);
        Assert.AreEqual("version: 2", fileResult.content.Substring(0, 10));
    }

    [TestMethod]
    public async Task GetActionsFileContentsTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repository = "RepoAutomationUnitTests";
        string? filePath = ".github/workflows/workflow.yml";

        //Act
        GitHubFile? fileResult = await GitHubFiles.GetFileContents(base.GitHubId, base.GitHubSecret,
            owner, repository, filePath);

        //Assert
        Assert.IsNotNull(fileResult);
        Assert.AreEqual("workflow.yml", fileResult.name);
        Assert.IsNotNull(fileResult.content);
        Assert.AreEqual("name: CI/CD", fileResult.content.Substring(0, 11));
    }

    [TestMethod]
    public async Task GetActionsFileWhereNoneExistsTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repository = "CustomQueue";
        string? filePath = ".github/workflows/workflow.yml";

        //Act
        GitHubFile? fileResult = await GitHubFiles.GetFileContents(base.GitHubId, base.GitHubSecret,
            owner, repository, filePath);

        //Assert
        Assert.IsNull(fileResult);
    }

}