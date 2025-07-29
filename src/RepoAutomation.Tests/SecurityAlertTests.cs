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
public class SecurityAlertTests : BaseAPIAccessTests
{
    [TestMethod]
    public async Task GetCodeScanningAlertsTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repo = "RepoAutomation"; // Repository that should have some code scanning setup

        //Act
        List<SecurityAlert> alerts = await GitHubApiAccess.GetCodeScanningAlerts(base.GitHubId, base.GitHubSecret, owner, repo);

        //Assert
        Assert.IsNotNull(alerts);
        // We don't assert on specific count as security alerts may vary over time
        // Just ensure the method doesn't crash and returns a list
    }

    [TestMethod]
    public async Task GetSecretScanningAlertsTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repo = "RepoAutomation";

        //Act
        List<SecretScanningAlert> alerts = await GitHubApiAccess.GetSecretScanningAlerts(base.GitHubId, base.GitHubSecret, owner, repo);

        //Assert
        Assert.IsNotNull(alerts);
        // We don't assert on specific count as secret alerts may vary over time
        // Just ensure the method doesn't crash and returns a list
    }

    [TestMethod]
    public async Task GetSecurityAlertsCountTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repo = "RepoAutomation";

        //Act
        (int codeScanningCount, int secretScanningCount, int dependabotCount, int totalCount) = await GitHubApiAccess.GetSecurityAlertsCount(base.GitHubId, base.GitHubSecret, owner, repo);

        //Assert
        Assert.IsTrue(codeScanningCount >= 0);
        Assert.IsTrue(secretScanningCount >= 0);
        Assert.IsTrue(dependabotCount >= 0);
        Assert.AreEqual(codeScanningCount + secretScanningCount + dependabotCount, totalCount);
    }

    [TestMethod]
    public async Task GetCodeScanningAlertsClosedStateTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repo = "RepoAutomation";
        string state = "closed";

        //Act
        List<SecurityAlert> alerts = await GitHubApiAccess.GetCodeScanningAlerts(base.GitHubId, base.GitHubSecret, owner, repo, state);

        //Assert
        Assert.IsNotNull(alerts);
        // Verify all returned alerts have the correct state
        foreach (var alert in alerts)
        {
            if (alert.state != null)
            {
                Assert.AreEqual(state, alert.state);
            }
        }
    }

    [TestMethod]
    public async Task GetSecretScanningAlertsResolvedStateTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repo = "RepoAutomation";
        string state = "resolved";

        //Act
        List<SecretScanningAlert> alerts = await GitHubApiAccess.GetSecretScanningAlerts(base.GitHubId, base.GitHubSecret, owner, repo, state);

        //Assert
        Assert.IsNotNull(alerts);
        // Verify all returned alerts have the correct state
        foreach (var alert in alerts)
        {
            if (alert.state != null)
            {
                Assert.AreEqual(state, alert.state);
            }
        }
    }

    [TestMethod]
    public async Task GetSecurityAlertsForNonExistentRepoTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repo = "NonExistentRepo12345";

        //Act
        List<SecurityAlert> codeScanningAlerts = await GitHubApiAccess.GetCodeScanningAlerts(base.GitHubId, base.GitHubSecret, owner, repo);
        List<SecretScanningAlert> secretScanningAlerts = await GitHubApiAccess.GetSecretScanningAlerts(base.GitHubId, base.GitHubSecret, owner, repo);

        //Assert
        Assert.IsNotNull(codeScanningAlerts);
        Assert.IsNotNull(secretScanningAlerts);
        Assert.AreEqual(0, codeScanningAlerts.Count);
        Assert.AreEqual(0, secretScanningAlerts.Count);
    }

    [TestMethod]
    public async Task GetDependabotAlertsTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repo = "RepoAutomation";

        //Act
        List<DependabotAlert> alerts = await GitHubApiAccess.GetDependabotAlerts(base.GitHubId, base.GitHubSecret, owner, repo);

        //Assert
        Assert.IsNotNull(alerts);
        // We don't assert on specific count as dependabot alerts may vary over time
        // Just ensure the method doesn't crash and returns a list
    }

    [TestMethod]
    public async Task GetDependabotAlertsFixedStateTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repo = "RepoAutomation";
        string state = "fixed";

        //Act
        List<DependabotAlert> alerts = await GitHubApiAccess.GetDependabotAlerts(base.GitHubId, base.GitHubSecret, owner, repo, state);

        //Assert
        Assert.IsNotNull(alerts);
        // Verify all returned alerts have the correct state
        foreach (var alert in alerts)
        {
            if (alert.state != null)
            {
                Assert.AreEqual(state, alert.state);
            }
        }
    }

    [TestMethod]
    public async Task GetDependabotAlertsForNonExistentRepoTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repo = "NonExistentRepo12345";

        //Act
        List<DependabotAlert> dependabotAlerts = await GitHubApiAccess.GetDependabotAlerts(base.GitHubId, base.GitHubSecret, owner, repo);

        //Assert
        Assert.IsNotNull(dependabotAlerts);
        Assert.AreEqual(0, dependabotAlerts.Count);
    }
}