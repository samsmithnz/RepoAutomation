using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RepoAutomation.Core.Models;
using System.Collections.Generic;

namespace RepoAutomation.Tests;

[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
[TestClass]
[TestCategory("UnitTests")]
public class SecurityAlertModelTests
{
    [TestMethod]
    public void SecurityAlertModelSerializationTest()
    {
        //Arrange
        string jsonCodeScanningAlert = @"{
            ""number"": 1,
            ""rule"": {
                ""id"": ""js/redos"",
                ""description"": ""Inefficient regular expression"",
                ""severity"": ""error""
            },
            ""state"": ""open"",
            ""created_at"": ""2022-01-01T00:00:00Z"",
            ""updated_at"": ""2022-01-01T00:00:00Z"",
            ""html_url"": ""https://github.com/example/example/security/code-scanning/1"",
            ""tool"": {
                ""name"": ""CodeQL"",
                ""version"": ""1.0.0""
            },
            ""most_recent_instance"": {
                ""ref"": ""refs/heads/main"",
                ""location"": {
                    ""path"": ""src/example.js"",
                    ""start_line"": 10,
                    ""end_line"": 10,
                    ""start_column"": 1,
                    ""end_column"": 25
                }
            }
        }";

        //Act
        SecurityAlert? alert = JsonConvert.DeserializeObject<SecurityAlert>(jsonCodeScanningAlert);

        //Assert
        Assert.IsNotNull(alert);
        Assert.AreEqual(1, alert.number);
        Assert.AreEqual("open", alert.state);
        Assert.AreEqual("2022-01-01T00:00:00Z", alert.created_at);
        Assert.IsNotNull(alert.tool);
        Assert.AreEqual("CodeQL", alert.tool.name);
        Assert.IsNotNull(alert.rule);
        Assert.AreEqual("js/redos", alert.rule.id);
        Assert.AreEqual("Inefficient regular expression", alert.rule.description);
        Assert.AreEqual("error", alert.rule.severity);
        Assert.IsNotNull(alert.most_recent_instance);
        Assert.IsNotNull(alert.most_recent_instance.location);
        Assert.AreEqual("src/example.js", alert.most_recent_instance.location.path);
    }

    [TestMethod]
    public void SecretScanningAlertModelSerializationTest()
    {
        //Arrange
        string jsonSecretAlert = @"{
            ""number"": 2,
            ""secret_type"": ""github_personal_access_token"",
            ""secret_type_display_name"": ""GitHub Personal Access Token"",
            ""state"": ""open"",
            ""created_at"": ""2022-01-01T00:00:00Z"",
            ""updated_at"": ""2022-01-01T00:00:00Z"",
            ""html_url"": ""https://github.com/example/example/security/secret-scanning/2"",
            ""locations"": [
                {
                    ""type"": ""commit"",
                    ""details"": {
                        ""path"": ""config/secrets.env"",
                        ""start_line"": 5,
                        ""end_line"": 5,
                        ""start_column"": 1,
                        ""end_column"": 40,
                        ""commit_sha"": ""abc123def456"",
                        ""commit_url"": ""https://github.com/example/example/commit/abc123def456""
                    }
                }
            ]
        }";

        //Act
        SecretScanningAlert? alert = JsonConvert.DeserializeObject<SecretScanningAlert>(jsonSecretAlert);

        //Assert
        Assert.IsNotNull(alert);
        Assert.AreEqual(2, alert.number);
        Assert.AreEqual("github_personal_access_token", alert.secret_type);
        Assert.AreEqual("GitHub Personal Access Token", alert.secret_type_display_name);
        Assert.AreEqual("open", alert.state);
        Assert.IsNotNull(alert.locations);
        Assert.AreEqual(1, alert.locations.Length);
        Assert.AreEqual("commit", alert.locations[0].type);
        Assert.IsNotNull(alert.locations[0].details);
        Assert.AreEqual("config/secrets.env", alert.locations[0].details.path);
    }

    [TestMethod]
    public void SecurityAlertListDeserializationTest()
    {
        //Arrange
        string jsonAlertsList = @"[
            {
                ""number"": 1,
                ""state"": ""open"",
                ""created_at"": ""2022-01-01T00:00:00Z""
            },
            {
                ""number"": 2,
                ""state"": ""dismissed"",
                ""created_at"": ""2022-01-02T00:00:00Z""
            }
        ]";

        //Act
        List<SecurityAlert>? alerts = JsonConvert.DeserializeObject<List<SecurityAlert>>(jsonAlertsList);

        //Assert
        Assert.IsNotNull(alerts);
        Assert.AreEqual(2, alerts.Count);
        Assert.AreEqual(1, alerts[0].number);
        Assert.AreEqual("open", alerts[0].state);
        Assert.AreEqual(2, alerts[1].number);
        Assert.AreEqual("dismissed", alerts[1].state);
    }

    [TestMethod]
    public void DependabotAlertModelSerializationTest()
    {
        //Arrange
        string jsonDependabotAlert = @"{
            ""number"": 1,
            ""state"": ""open"",
            ""dependency"": {
                ""package"": ""lodash"",
                ""manifest_path"": ""package.json"",
                ""scope"": ""runtime""
            },
            ""security_advisory"": {
                ""ghsa_id"": ""GHSA-jf85-cpcp-j695"",
                ""cve_id"": ""CVE-2021-23337"",
                ""summary"": ""Command injection in lodash"",
                ""description"": ""lodash versions prior to 4.17.21 are vulnerable to Command Injection via the template function."",
                ""severity"": ""high"",
                ""identifiers"": [""GHSA-jf85-cpcp-j695"", ""CVE-2021-23337""],
                ""references"": [""https://nvd.nist.gov/vuln/detail/CVE-2021-23337""],
                ""published_at"": ""2021-02-15T00:00:00Z"",
                ""updated_at"": ""2021-02-15T00:00:00Z""
            },
            ""security_vulnerability"": {
                ""package"": ""lodash"",
                ""severity"": ""high"",
                ""vulnerable_version_range"": ""< 4.17.21"",
                ""first_patched_version"": ""4.17.21""
            },
            ""url"": ""https://api.github.com/repos/example/example/dependabot/alerts/1"",
            ""html_url"": ""https://github.com/example/example/security/dependabot/1"",
            ""created_at"": ""2022-01-01T00:00:00Z"",
            ""updated_at"": ""2022-01-01T00:00:00Z""
        }";

        //Act
        DependabotAlert? alert = JsonConvert.DeserializeObject<DependabotAlert>(jsonDependabotAlert);

        //Assert
        Assert.IsNotNull(alert);
        Assert.AreEqual(1, alert.number);
        Assert.AreEqual("open", alert.state);
        Assert.AreEqual("2022-01-01T00:00:00Z", alert.created_at);
        Assert.IsNotNull(alert.dependency);
        Assert.AreEqual("lodash", alert.dependency.package);
        Assert.AreEqual("package.json", alert.dependency.manifest_path);
        Assert.AreEqual("runtime", alert.dependency.scope);
        Assert.IsNotNull(alert.security_advisory);
        Assert.AreEqual("GHSA-jf85-cpcp-j695", alert.security_advisory.ghsa_id);
        Assert.AreEqual("CVE-2021-23337", alert.security_advisory.cve_id);
        Assert.AreEqual("Command injection in lodash", alert.security_advisory.summary);
        Assert.AreEqual("high", alert.security_advisory.severity);
        Assert.IsNotNull(alert.security_vulnerability);
        Assert.AreEqual("lodash", alert.security_vulnerability.package);
        Assert.AreEqual("high", alert.security_vulnerability.severity);
        Assert.AreEqual("< 4.17.21", alert.security_vulnerability.vulnerable_version_range);
        Assert.AreEqual("4.17.21", alert.security_vulnerability.first_patched_version);
    }

    [TestMethod]
    public void DependabotAlertListDeserializationTest()
    {
        //Arrange
        string jsonAlertsList = @"[
            {
                ""number"": 1,
                ""state"": ""open"",
                ""created_at"": ""2022-01-01T00:00:00Z"",
                ""dependency"": {
                    ""package"": ""lodash""
                }
            },
            {
                ""number"": 2,
                ""state"": ""fixed"",
                ""created_at"": ""2022-01-02T00:00:00Z"",
                ""dependency"": {
                    ""package"": ""moment""
                }
            }
        ]";

        //Act
        List<DependabotAlert>? alerts = JsonConvert.DeserializeObject<List<DependabotAlert>>(jsonAlertsList);

        //Assert
        Assert.IsNotNull(alerts);
        Assert.AreEqual(2, alerts.Count);
        Assert.AreEqual(1, alerts[0].number);
        Assert.AreEqual("open", alerts[0].state);
        Assert.AreEqual(2, alerts[1].number);
        Assert.AreEqual("fixed", alerts[1].state);
        Assert.IsNotNull(alerts[0].dependency);
        Assert.AreEqual("lodash", alerts[0].dependency.package);
        Assert.IsNotNull(alerts[1].dependency);
        Assert.AreEqual("moment", alerts[1].dependency.package);
    }
}