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
        string repoName = "RepoAutomationUnitTests";

        //Act
        List<PullRequest>? pullRequests = await GitHubAPIAccess.GetPullRequests(base.GitHubId, base.GitHubSecret,
            owner, repoName);

        //Assert
        Assert.IsNotNull(pullRequests);
        if (pullRequests != null && pullRequests.Count > 0)
        {
            Assert.IsTrue(string.IsNullOrEmpty(pullRequests[0].Title) == false);
        }
    }

    [TestMethod]
    public async Task GetPullRequestNoReviewsTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repoName = "RepoAutomationUnitTests";
        string pullRequestNumber = "1";

        //Act
        List<PRReview>? pullRequestReviews = await GitHubAPIAccess.GetPullRequestReview(base.GitHubId, base.GitHubSecret,
            owner, repoName, pullRequestNumber);

        //Assert
        Assert.IsNotNull(pullRequestReviews);
        Assert.AreEqual(0, pullRequestReviews.Count);
    }

    [TestMethod]
    public async Task GetPullRequestMultipleReviewsTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repoName = "RepoAutomationUnitTests";
        string pullRequestNumber = "7";

        //Act
        List<PRReview>? pullRequestReviews = await GitHubAPIAccess.GetPullRequestReview(base.GitHubId, base.GitHubSecret,
            owner, repoName, pullRequestNumber);

        //Assert
        Assert.IsNotNull(pullRequestReviews);
        Assert.AreEqual(2, pullRequestReviews.Count);
        if (pullRequestReviews != null )
        {
            Assert.AreEqual("8d495f5ba3e16d70800328c081aeb6d408f4ac86", pullRequestReviews[0].commit_id);
            Assert.AreEqual("996002417", pullRequestReviews[0].id);
            Assert.AreEqual("APPROVED", pullRequestReviews[0].state);
            Assert.AreEqual("2022-06-05T12:31:44Z", pullRequestReviews[0].submitted_at);
        }
    }

}