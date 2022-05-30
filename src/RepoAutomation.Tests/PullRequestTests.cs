using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoAutomation.Core.APIAccess;
using RepoAutomation.Core.Models;
using RepoAutomation.Tests.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepoAutomation.Tests;

[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
[TestClass]
[TestCategory("IntegrationTests")]
public class PullRequestTests : BaseAPIAccessTests
{
    [TestMethod]
    public async Task GetPullRequestsTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repoName = "RepoAutomation"; //inception!!
        
        //Act
        List<PullRequest>? pullRequests = await GitHubAPIAccess.GetPullRequests(base.GitHubId, base.GitHubSecret,
            owner, repoName);

        //Assert
        Assert.IsNotNull(pullRequests);
        if (pullRequests != null && pullRequests.Count > 0)
        {
            Assert.IsTrue(string.IsNullOrEmpty(pullRequests[0].Title));
        }
    }

}