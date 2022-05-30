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
        //if (branchProtectionPolicy != null)
        //{
        //    Assert.IsNotNull(branchProtectionPolicy.required_status_checks);
        //    Assert.AreEqual(1, branchProtectionPolicy.required_status_checks?.checks?.Length);
        //    Assert.AreEqual("versionAndTest", branchProtectionPolicy.required_status_checks?.checks?[0].context);
        //    Assert.IsTrue(branchProtectionPolicy.required_status_checks?.strict);
        //    Assert.IsTrue(branchProtectionPolicy.enforce_admins?.enabled);
        //    Assert.IsTrue(branchProtectionPolicy.required_conversation_resolution?.enabled);
        //}
    }
    
}