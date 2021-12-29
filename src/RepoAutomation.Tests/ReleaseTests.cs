using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoAutomation.APIAccess;
using RepoAutomation.Models;
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
        string repoName = "RepoAutomation"; //inception!!

        //Act
        Release? release = await GitHubAPIAccess.GetReleaseLatest(base.GitHubId, base.GitHubSecret, owner, repoName);

        //Assert
        Assert.IsNotNull(release);
        if (release != null)
        {
            Assert.IsNotNull(release.tag_name);
            string releaseTag = release.tag_name;
            Assert.IsTrue(release.assets?.Length > 0);
            Assert.AreEqual($"https://github.com/{owner}/{repoName}/releases/download/{releaseTag}/RepoAutomation.Linux_x64.{releaseTag}.zip", release.assets?[0].browser_download_url);
        }
    }

}