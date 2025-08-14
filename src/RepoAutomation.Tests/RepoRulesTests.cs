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
public class RepoRulesTests : BaseAPIAccessTests
{
    [TestMethod]
    public async Task GetRepositoryRulesTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repoName = "RepoAutomationUnitTests";

        //Act
        List<RepositoryRuleset>? repositoryRules = await GitHubApiAccess.GetRepositoryRules(base.GitHubId, base.GitHubSecret,
            owner, repoName);

        //Assert
        Assert.IsNotNull(repositoryRules);
        // Note: Repository rules might not exist for all repos, so we just check the call works
        if (repositoryRules != null && repositoryRules.Count > 0)
        {
            Assert.IsNotNull(repositoryRules[0].name);
            Assert.IsNotNull(repositoryRules[0].RawJSON);
        }
    }

    [TestMethod]
    public async Task GetRepositoryRulesetTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repoName = "RepoAutomationUnitTests";

        //Act - First get the list of rules to find a valid ID
        List<RepositoryRuleset>? repositoryRules = await GitHubApiAccess.GetRepositoryRules(base.GitHubId, base.GitHubSecret,
            owner, repoName);

        //Assert
        Assert.IsNotNull(repositoryRules);
        if (repositoryRules != null && repositoryRules.Count > 0)
        {
            int rulesetId = repositoryRules[0].id;

            //Act - Get specific ruleset
            RepositoryRuleset? ruleset = await GitHubApiAccess.GetRepositoryRuleset(base.GitHubId, base.GitHubSecret,
                owner, repoName, rulesetId);

            //Assert
            Assert.IsNotNull(ruleset);
            if (ruleset != null)
            {
                Assert.AreEqual(rulesetId, ruleset.id);
                Assert.IsNotNull(ruleset.name);
                Assert.IsNotNull(ruleset.RawJSON);
            }
        }
    }

    [TestMethod]
    public async Task GetRepositoryRulesForRepoWithoutRulesTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repoName = "CustomQueue"; // A repo that likely doesn't have repository rules

        //Act
        List<RepositoryRuleset>? repositoryRules = await GitHubApiAccess.GetRepositoryRules(base.GitHubId, base.GitHubSecret,
            owner, repoName);

        //Assert
        Assert.IsNotNull(repositoryRules);
        // The call should succeed even if no rules exist (empty list)
    }

    [TestMethod]
    public async Task CreateRepositoryRulesetTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repoName = "RepoAutomationUnitTests";

        RepositoryRulesetPut newRuleset = new()
        {
            name = "Test Ruleset",
            target = "branch",
            enforcement = "active",
            conditions = new Conditions
            {
                ref_name = new RefName
                {
                    include = new string[] { "main", "develop" },
                    exclude = new string[] { }
                }
            },
            rules = new Rule[]
            {
                new Rule
                {
                    type = "required_status_checks",
                    parameters = new RuleParameters
                    {
                        strict_required_status_checks_policy = true,
                        required_status_checks = new RepositoryRuleStatusCheck[]
                        {
                            new RepositoryRuleStatusCheck { context = "Build job" }
                        }
                    }
                }
            }
        };

        //Act
        bool result = await GitHubApiAccess.CreateRepositoryRuleset(base.GitHubId, base.GitHubSecret,
            owner, repoName, newRuleset);

        //Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task UpdateRepositoryRulesetTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repoName = "RepoAutomationUnitTests";

        // First get existing rulesets to find one to update
        List<RepositoryRuleset>? repositoryRules = await GitHubApiAccess.GetRepositoryRules(base.GitHubId, base.GitHubSecret,
            owner, repoName);

        Assert.IsNotNull(repositoryRules);
        if (repositoryRules != null && repositoryRules.Count > 0)
        {
            int rulesetId = repositoryRules[0].id;

            RepositoryRulesetPut updatedRuleset = new()
            {
                name = "Updated Test Ruleset",
                target = "branch",
                enforcement = "active",
                conditions = new Conditions
                {
                    ref_name = new RefName
                    {
                        include = new string[] { "main" },
                        exclude = new string[] { }
                    }
                },
                rules = new Rule[]
                {
                    new Rule
                    {
                        type = "required_status_checks",
                        parameters = new RuleParameters
                        {
                            strict_required_status_checks_policy = true,
                            required_status_checks = new RepositoryRuleStatusCheck[]
                            {
                                new RepositoryRuleStatusCheck { context = "Updated Build job" }
                            }
                        }
                    }
                }
            };

            //Act
            bool result = await GitHubApiAccess.UpdateRepositoryRuleset(base.GitHubId, base.GitHubSecret,
                owner, repoName, rulesetId, updatedRuleset);

            //Assert
            Assert.IsTrue(result);
        }
        else
        {
            Assert.Inconclusive("No existing rulesets found to update");
        }
    }
}