using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoAutomation.Core.APIAccess;
using RepoAutomation.Core.Models;
using RepoAutomation.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RepoAutomation.Tests;

[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
[TestClass]
[TestCategory("IntegrationTests")]
public class RepoTests : BaseAPIAccessTests
{
    [TestMethod]
    public async Task GetRepoTest()
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
            Assert.AreEqual(true, repo.allow_auto_merge);
            Assert.AreEqual(true, repo.delete_branch_on_merge);
            Assert.AreEqual(true, repo.allow_merge_commit);
            Assert.AreEqual(false, repo.allow_rebase_merge);
            Assert.AreEqual(true, repo.allow_squash_merge);
            Assert.AreEqual("public", repo.visibility);
            Assert.AreEqual("main", repo.default_branch);
            Assert.IsNotNull(repo.RawJSON);
            Assert.IsNotNull(repo.id);
        }
    }

    [TestMethod]
    public async Task GetRepoThatDoesNotExistTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repoName = "RepoAutomation2"; //Doesn't exist

        //Act
        try
        {
            Repo? repo = await GitHubAPIAccess.GetRepo(base.GitHubId, base.GitHubSecret, owner, repoName);
        }
        catch (Exception ex)
        {
            //Assert
            Assert.AreEqual("Response status code does not indicate success: 404 (Not Found).", ex.Message);
        }
    }

    [TestMethod]
    public async Task DeleteRepoThatDoesNotExistTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repoName = "RepoAutomationToDelete"; //Doesn't exist

        //Act
        try
        {
            bool result = await GitHubAPIAccess.DeleteRepo(base.GitHubId, base.GitHubSecret, owner, repoName);
        }
        catch (Exception ex)
        {
            //Assert
            Assert.AreEqual("Response status code does not indicate success: 404 (Not Found).", ex.Message);
        }
    }

    [TestMethod]
    public async Task GetReposTest()
    {
        //Arrange
        string owner = "samsmithnz";

        //Act
        List<Repo>? repos = await GitHubAPIAccess.GetRepos(base.GitHubId, base.GitHubSecret,
              owner);

        //Assert
        Assert.IsNotNull(repos);
        Assert.IsTrue(repos.Count >= 30);
        //bool foundPublicRepo = false;
        //bool foundPrivateRepo = false;
        //foreach (Repo repo in repos)
        //{
        //    if (repo.visibility == "public")
        //    {
        //        foundPublicRepo = true;
        //    }
        //    else if (repo.visibility == "private")
        //    {
        //        foundPrivateRepo = true;
        //    }
        //}
        //Assert.IsTrue(foundPublicRepo);
        //Assert.IsTrue(foundPrivateRepo);
    }

    [TestMethod]
    public async Task CreateAndDeleteRepoTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repoName = "NewRepoTest";
        Repo? repo;

        try
        {
            //Act 0: Check if the test half failed earlier
            repo = await GitHubAPIAccess.GetRepo(base.GitHubId, base.GitHubSecret, owner, repoName);
            if (repo == null)
            {
                //Act I: Creation
                await GitHubAPIAccess.CreateRepo(base.GitHubId, base.GitHubSecret, repoName,
                    true, true, false, true);
                repo = await GitHubAPIAccess.GetRepo(base.GitHubId, base.GitHubSecret, owner, repoName);

                //Assert
                Assert.IsNotNull(repo);
                if (repo != null)
                {
                    Assert.AreEqual(owner, repo.owner?.login);
                    Assert.AreEqual(repoName, repo.name);
                    Assert.AreEqual(owner + "/" + repoName, repo.full_name);
                    Assert.AreEqual(true, repo.allow_auto_merge);
                    Assert.AreEqual(true, repo.delete_branch_on_merge);
                    Assert.AreEqual(true, repo.allow_merge_commit);
                    Assert.AreEqual(false, repo.allow_rebase_merge);
                    Assert.AreEqual(true, repo.allow_squash_merge);
                    Assert.AreEqual("private", repo.visibility);
                    Assert.AreEqual("main", repo.default_branch);
                    Assert.IsNotNull(repo.RawJSON);
                    Assert.IsNotNull(repo.id);
                }
            }
        }
        finally
        {
            //Act II: End of days
            await GitHubAPIAccess.DeleteRepo(base.GitHubId, base.GitHubSecret, owner, repoName);
            try
            {
                repo = await GitHubAPIAccess.GetRepo(base.GitHubId, base.GitHubSecret, owner, repoName);
            }
            catch (Exception ex)
            {
                //Assert
                Assert.AreEqual("Response status code does not indicate success: 404 (Not Found).", ex.Message);
            }
        }
    }

    [TestMethod]
    public async Task DeleteTestRepoTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repoName = "RepoAutomationTest"; //shouldn't exist, but if it does, it's deleted

        //Act
        await GitHubAPIAccess.DeleteRepo(base.GitHubId, base.GitHubSecret, owner, repoName, false);
        if (Directory.Exists(@"C:\Users\samsm\source\repos\RepoAutomationTest"))
        {
            Directory.Delete(@"C:\Users\samsm\source\repos\RepoAutomationTest", true);
        }
    }

    [TestMethod]
    public async Task GetLastCommitForCustomQueueTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repo = "CustomQueue";

        //Act
        string? commitSHA = await GitHubAPIAccess.GetLastCommit(base.GitHubId, base.GitHubSecret, owner, repo);

        //Assert
        Assert.IsNotNull(commitSHA);
        Assert.AreEqual("e18a81f32522af019efe584c0b9655e0f0c03835", commitSHA);
    }
}