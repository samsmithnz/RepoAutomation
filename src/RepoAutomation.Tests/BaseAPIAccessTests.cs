using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace RepoAutomation.Tests;

[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public class BaseAPIAccessTests
{
    public string? GitHubId { get; set; }
    public string? GitHubSecret { get; set; }

    [TestInitialize]
    public void InitializeIntegrationTests()
    {
        //Load the appsettings.json configuration file
        IConfigurationBuilder? builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false)
             .AddUserSecrets<Program>();
        IConfigurationRoot configuration = builder.Build();

        GitHubId = configuration["AppSettings:GitHubClientId"];
        GitHubSecret = configuration["AppSettings:GitHubClientSecret"];
    }
}