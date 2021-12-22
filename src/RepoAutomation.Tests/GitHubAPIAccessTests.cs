using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoAutomation.APIAccess;
using RepoAutomation.Models;
using System.Threading.Tasks;

namespace RepoAutomation.Tests;

[TestClass]
[TestCategory("IntegrationTests")]
public class GitHubAPIAccessTests : BaseAPIAccessTests
{
    [TestMethod]
    public async Task RepoGetTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repoName = "RepoAutomation"; //inception!!

        //Act
        Repo? repo = await GitHubAPIAccess.GetRepo(base.GitHubId, base.GitHubSecret, owner, repoName);

        //Assert
        Assert.IsNotNull(repo);
        if (repo != null)
        {
            Assert.AreEqual(owner, repo.owner?.login);
            Assert.AreEqual(repoName, repo.name);
            Assert.AreEqual(owner + "/" + repoName, repo.full_name);
        }
    }
}