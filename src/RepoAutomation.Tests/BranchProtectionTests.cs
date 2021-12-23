using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoAutomation.APIAccess;
using RepoAutomation.Models;
using RepoAutomation.Tests.Helpers;
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
        string repoName = "RepoAutomation"; //inception!!
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
            Assert.AreEqual("version", branchProtectionPolicy.required_status_checks?.checks?[0]);
        }
    }

    [TestMethod]
    public async Task GetBranchProtectionPolicyThatDoesNotExistTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repoName = "RepoAutomation"; //inception!!
        string branchName = "main2";

        //Act
        BranchProtectionPolicy? branchProtectionPolicy = await GitHubAPIAccess.GetBranchProtectionPolicy(base.GitHubId, base.GitHubSecret,
            owner, repoName, branchName);

        //Assert
        Assert.IsNull(branchProtectionPolicy);
    }

    [TestMethod]
    public async Task UpdateBranchProtectionPolicyTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repoName = "RepoAutomation";
        string branchName = "main";
        string[] contexts = new string[]
        {
            "version",
            "build (Linux_x64, linux-x64)",
            "build (Windows_x64, windows-x64)"
        };

        //Act
        bool result = await GitHubAPIAccess.UpdateBranchProtectionPolicy(base.GitHubId, base.GitHubSecret, owner, repoName, 
            branchName, contexts);

        //Assert
        Assert.IsFalse(result);
    }

    //[TestMethod]
    //public async Task CreateAndDeleteRepoTest()
    //{
    //    //Arrange
    //    string owner = "samsmithnz";
    //    string repoName = "NewRepoTest";

    //    //Act I: Creation
    //    await GitHubAPIAccess.CreateRepo(base.GitHubId, base.GitHubSecret, repoName,
    //        true, true, false, true);
    //    Repo? repo = await GitHubAPIAccess.GetRepo(base.GitHubId, base.GitHubSecret, owner, repoName);

    //    //Assert
    //    Assert.IsNotNull(repo);
    //    if (repo != null)
    //    {
    //        Assert.AreEqual(owner, repo.owner?.login);
    //        Assert.AreEqual(repoName, repo.name);
    //        Assert.AreEqual(owner + "/" + repoName, repo.full_name);
    //        Assert.AreEqual("true", repo.allow_auto_merge);
    //        Assert.AreEqual("true", repo.delete_branch_on_merge);
    //        Assert.AreEqual("true", repo.allow_merge_commit);
    //        Assert.AreEqual("false", repo.allow_rebase_merge);
    //        Assert.AreEqual("true", repo.allow_squash_merge);
    //        Assert.AreEqual("private", repo.visibility);
    //        Assert.AreEqual("main", repo.default_branch);
    //        Assert.IsNotNull(repo.RawJSON);
    //        Assert.IsNotNull(repo.id);
    //    }

    //    //Act II: End of days
    //    await GitHubAPIAccess.DeleteRepo(base.GitHubId, base.GitHubSecret, owner, repoName);
    //    repo = await GitHubAPIAccess.GetRepo(base.GitHubId, base.GitHubSecret, owner, repoName);

    //    Assert.IsNull(repo);
    //}
}