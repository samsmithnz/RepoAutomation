using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoAutomation.Core.APIAccess;
using RepoAutomation.Core.Models;
using RepoAutomation.Tests.Helpers;
using System.Threading.Tasks;

namespace RepoAutomation.Tests;

[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
[TestClass]
[TestCategory("IntegrationTests")]
public class ReleaseTests : BaseAPIAccessTests
{
    [TestMethod]
    public async Task GetReleaseTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repoName = "RepoAutomationUnitTests";

        //Act
        Release? release = await GitHubApiAccess.GetReleaseLatest(base.GitHubId, base.GitHubSecret, owner, repoName);

        //Assert
        Assert.IsNotNull(release);
        if (release != null)
        {
            Assert.IsNotNull(release.tag_name);
            //string releaseTag = release.tag_name;
            //Assert.IsTrue(release.assets?.Length > 0);
            //Assert.AreEqual($"https://github.com/{owner}/{repoName}/releases/download/{releaseTag}/RepoAutomation.Linux_x64.{releaseTag}.zip", release.assets?[0].browser_download_url);
            Assert.IsNotNull(release.id);
            Assert.IsNotNull(release.published_at);
            Assert.IsNotNull(release.html_url);
            Assert.IsNotNull(release.ToTimingString());
            Assert.AreEqual("v1.0.0", release.name);
        }
    }
    
    [TestMethod]
    public async Task GetReleaseFromRepoWithNoReleasesTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repoName = "FictionBook";

        //Act
        Release? release = await GitHubApiAccess.GetReleaseLatest(base.GitHubId, base.GitHubSecret, owner, repoName);

        //Assert
        Assert.IsNull(release);
    }

}