using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoAutomation.Core.APIAccess;
using RepoAutomation.Core.Helpers;
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
        string repoName = "RepoAutomationUnitTests";

        //Act
        Repo? repo = await GitHubApiAccess.GetRepo(base.GitHubId, base.GitHubSecret, owner, repoName);

        //Assert
        Assert.IsNotNull(repo);
        if (repo != null)
        {
            Assert.AreEqual(owner, repo.owner?.login);
            Assert.AreEqual(repoName, repo.name);
            Assert.AreEqual(owner + "/" + repoName, repo.full_name);
            Assert.AreEqual("A test project, where everything is configured 'wrong'", repo.description);
            Assert.AreEqual(false, repo.allow_auto_merge);
            Assert.AreEqual(false, repo.delete_branch_on_merge);
            Assert.AreEqual(true, repo.allow_merge_commit);
            Assert.AreEqual(expected: true, repo.allow_rebase_merge);
            Assert.AreEqual(true, repo.allow_squash_merge);
            Assert.AreEqual("private", repo.visibility);
            Assert.AreEqual("main", repo.default_branch);
            Assert.IsNotNull(repo.RawJSON);
            Assert.IsNotNull(repo.id);
        }

        //Act 2
        bool isPrivate = false;
        if (repo != null)
        {
            if (repo.visibility == "private")
            {
                isPrivate = true;
            }
            bool result = await GitHubApiAccess.UpdateRepo(base.GitHubId, base.GitHubSecret, owner, repoName,
                     repo.allow_auto_merge,
                     repo.delete_branch_on_merge,
                     repo.allow_rebase_merge,
                     isPrivate);

            //Assert 2
            Assert.IsTrue(result);
        }


        //Act 3
        repo = await GitHubApiAccess.GetRepo(base.GitHubId, base.GitHubSecret, owner, repoName);

        //Assert 3
        Assert.IsNotNull(repo);
        if (repo != null)
        {
            Assert.AreEqual(owner, repo.owner?.login);
            Assert.AreEqual(repoName, repo.name);
            Assert.AreEqual(owner + "/" + repoName, repo.full_name);
            Assert.AreEqual(false, repo.allow_auto_merge);
            Assert.AreEqual(false, repo.delete_branch_on_merge);
            Assert.AreEqual(true, repo.allow_merge_commit);
            Assert.AreEqual(true, repo.allow_rebase_merge);
            Assert.AreEqual(true, repo.allow_squash_merge);
            Assert.AreEqual("private", repo.visibility);
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
            Repo? repo = await GitHubApiAccess.GetRepo(base.GitHubId, base.GitHubSecret, owner, repoName);
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
            bool result = await GitHubApiAccess.DeleteRepo(base.GitHubId, base.GitHubSecret, owner, repoName);
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
        List<Repo>? repos = await GitHubApiAccess.GetRepos(base.GitHubId, base.GitHubSecret,
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
            repo = await GitHubApiAccess.GetRepo(base.GitHubId, base.GitHubSecret, owner, repoName);
            if (repo == null)
            {
                //Act I: Creation
                await GitHubApiAccess.CreateRepo(base.GitHubId, base.GitHubSecret, repoName,
                    true, true, false, true);
                repo = await GitHubApiAccess.GetRepo(base.GitHubId, base.GitHubSecret, owner, repoName);

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
            await GitHubApiAccess.DeleteRepo(base.GitHubId, base.GitHubSecret, owner, repoName);
            try
            {
                repo = await GitHubApiAccess.GetRepo(base.GitHubId, base.GitHubSecret, owner, repoName);
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
        await GitHubApiAccess.DeleteRepo(base.GitHubId, base.GitHubSecret, owner, repoName, false);
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
        string? commitSHA = await GitHubApiAccess.GetLastCommit(base.GitHubId, base.GitHubSecret, owner, repo);

        //Assert
        Assert.IsNotNull(commitSHA);
    }

    [TestMethod]
    public async Task GetRepoLanguagesTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repo = "RepoAutomation";

        //Act
        Dictionary<string, int>? languages = await GitHubApiAccess.GetRepoLanguages(base.GitHubId, base.GitHubSecret, owner, repo);

        //Assert
        Assert.IsNotNull(languages);
        Assert.AreEqual(5, languages.Count);
    }

    [TestMethod]
    public async Task GetRepoLanguagesWithHelperTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repo = "RepoAutomation";

        //Act
        List<RepoLanguage> repoLanguages = await RepoLanguageHelper.GetRepoLanguages(base.GitHubId, base.GitHubSecret, owner, repo);

        //Assert
        Assert.IsNotNull(repoLanguages);
        Assert.AreEqual(5, repoLanguages.Count);
        Assert.AreEqual("C#", repoLanguages[0].Name);
        Assert.IsTrue(repoLanguages[0].Percent < 96M);
        Assert.AreEqual("#178600", repoLanguages[0].Color);
        Assert.AreEqual("HTML", repoLanguages[1].Name);
        Assert.IsTrue(repoLanguages[1].Percent < 4M);
        Assert.AreEqual("#e34c26", repoLanguages[1].Color);
        Assert.AreEqual("CSS", repoLanguages[2].Name);
        Assert.IsTrue(repoLanguages[2].Percent < 2M);
        Assert.AreEqual("#663399", repoLanguages[2].Color);
        Assert.AreEqual("Dockerfile", repoLanguages[3].Name);
        Assert.IsTrue(repoLanguages[3].Percent < 1M);
        Assert.AreEqual("#384d54", repoLanguages[3].Color);
        Assert.AreEqual("JavaScript", repoLanguages[4].Name);
        Assert.IsTrue(repoLanguages[4].Percent < 1M);
        Assert.AreEqual("#178600", repoLanguages[0].Color);
    }

    [TestMethod]
    public async Task GetRepoLanguagesInAnotherOrgWithHelperTest()
    {
        //Arrange
        string owner = "DeveloperMetrics";
        string repo = "DevOpsMetrics";

        //Act
        List<RepoLanguage> repoLanguages = await RepoLanguageHelper.GetRepoLanguages(base.GitHubId, base.GitHubSecret, owner, repo);

        //Assert
        Assert.IsNotNull(repoLanguages);
        Assert.AreEqual(5, repoLanguages.Count);
        Assert.AreEqual("C#", repoLanguages[0].Name);
        Assert.IsTrue(repoLanguages[0].Percent < 90M);
        Assert.AreEqual("#178600", repoLanguages[0].Color);
        Assert.AreEqual("HTML", repoLanguages[1].Name);
        Assert.IsTrue(repoLanguages[1].Percent < 10M);
        Assert.AreEqual("#e34c26", repoLanguages[1].Color);
        Assert.AreEqual("PowerShell", repoLanguages[2].Name);
        Assert.IsTrue(repoLanguages[2].Percent < 3M);
        Assert.AreEqual("#012456", repoLanguages[2].Color);
        Assert.AreEqual("CSS", repoLanguages[3].Name);
        Assert.IsTrue(repoLanguages[3].Percent < 2M);
        Assert.AreEqual("#663399", repoLanguages[3].Color);
        Assert.AreEqual("JavaScript", repoLanguages[4].Name);
        Assert.IsTrue(repoLanguages[4].Percent < 1M);
        Assert.AreEqual("#178600", repoLanguages[0].Color);
    }

}