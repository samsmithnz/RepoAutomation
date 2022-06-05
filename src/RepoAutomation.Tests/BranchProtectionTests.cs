using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoAutomation.Core.APIAccess;
using RepoAutomation.Core.Models;
using RepoAutomation.Tests.Helpers;
using System;
using System.Threading.Tasks;

namespace RepoAutomation.Tests;

[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
[TestClass]
[TestCategory("IntegrationTests")]
public class BranchProtectionTests : BaseAPIAccessTests
{
    [TestMethod]
    public async Task GetBranchProtectionTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repoName = "RepoAutomationUnitTests"; 
        string branchName = "main";

        //Act
        BranchProtectionPolicy? branchProtectionPolicy = await GitHubAPIAccess.GetBranchProtectionPolicy(base.GitHubId, base.GitHubSecret,
            owner, repoName, branchName);

        //Assert
        Assert.IsNotNull(branchProtectionPolicy);
        if (branchProtectionPolicy != null)
        {
            Assert.IsNotNull(branchProtectionPolicy.required_status_checks);
            Assert.AreEqual(1, branchProtectionPolicy.required_status_checks?.checks?.Length);
            Assert.AreEqual("versionAndTest", branchProtectionPolicy.required_status_checks?.checks?[0].context);
            Assert.IsTrue(branchProtectionPolicy.required_status_checks?.strict);
            Assert.IsTrue(branchProtectionPolicy.enforce_admins?.enabled);
            Assert.IsTrue(branchProtectionPolicy.required_conversation_resolution?.enabled);
        }
    }

    [TestMethod]
    public async Task GetBranchProtectionPolicyThatDoesNotExistTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repoName = "RepoAutomationUnitTests"; 
        string branchName = "main2";

        //Act
        try
        {
            BranchProtectionPolicy? branchProtectionPolicy = await GitHubAPIAccess.GetBranchProtectionPolicy(base.GitHubId, base.GitHubSecret,
                owner, repoName, branchName);
        }
        catch (Exception ex)
        {
            //Assert
            Assert.AreEqual("Response status code does not indicate success: 404 (Not Found).", ex.Message);
        }
    }

    [TestMethod]
    public async Task UpdateBranchProtectionPolicyTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repoName = "RepoAutomationUnitTests";
        string branchName = "main";
        RequiredStatusCheckPut requiredStatusCheck = new()
        {
            strict = true,
            checks = new Check[1] { new Check() { context = "versionAndTest" } }
        };

        //Act
        bool result = await GitHubAPIAccess.UpdateBranchProtectionPolicy(base.GitHubId, base.GitHubSecret, owner, repoName,
            branchName, requiredStatusCheck);

        //Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task GetBranchProtectionWhereItDoesnotExistTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repoName = "CustomQueue";
        string branchName = "main";

        //Act
        BranchProtectionPolicy? branchProtectionPolicy = await GitHubAPIAccess.GetBranchProtectionPolicy(base.GitHubId, base.GitHubSecret,
            owner, repoName, branchName);

        //Assert
        Assert.IsNull(branchProtectionPolicy);
    }
}