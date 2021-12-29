using GitHubActionsDotNet.Helpers;
using GitHubActionsDotNet.Models;
using RepoAutomation.APIAccess;
using RepoAutomation.Models;
using System.Text;
using GitHubActionsDotNet.Serialization;

namespace RepoAutomation
{
    public static class GitHubActionsAutomation
    {
        public static async Task<string> SetupAction(string workingDirectory, string workingTempDirectory, Asset[]? assets)
        {
            StringBuilder log = new();

            JobHelper jobHelper = new();
            GitHubActionsRoot root = new();
            root.name = "CI/CD";
            root.on = TriggerHelper.AddStandardPushAndPullTrigger("main");

            string displayBuildGitVersionScript = @"
echo ""Version: ${{ steps.gitversion.outputs.SemVer }}""
echo ""CommitsSinceVersionSource: ${{ steps.gitversion.outputs.CommitsSinceVersionSource }}""";

            Step[] buildSteps = new Step[] {
            CommonStepHelper.AddCheckoutStep(null,null,"0"),
            GitVersionStepHelper.AddGitVersionSetupStep(),
            GitVersionStepHelper.AddGitVersionDetermineVersionStep(),
            CommonStepHelper.AddScriptStep("Display GitVersion outputs", displayBuildGitVersionScript),
            DotNetStepHelper.AddDotNetSetupStep("Setup .NET","6.x"),
            DotNetStepHelper.AddDotNetTestStep(".NET test","src/GitHubActionsDotNet.Tests/GitHubActionsDotNet.Tests.csproj","Release",null,true),
            DotNetStepHelper.AddDotNetPublishStep(".NET publish","src/GitHubActionsDotNet/GitHubActionsDotNet.csproj","Release",null,"-p:Version='${{ steps.gitversion.outputs.SemVer }}'", true),
            CommonStepHelper.AddUploadArtifactStep("Upload package back to GitHub","nugetPackage","src/GitHubActionsDotNet/bin/Release")
        };
            root.jobs = new();
            Job buildJob = jobHelper.AddJob(
                "Build job",
                "${{matrix.os}}",
                buildSteps);
            //Add the strategy
            buildJob.strategy = new()
            {
                matrix = new()
                {
                    { "os", new string[] { "ubuntu-latest", "windows-latest" } }
                }
            };
            buildJob.outputs = new()
            {
                { "Version", "${{ steps.gitversion.outputs.SemVer }}" },
                { "CommitsSinceVersionSource", "${{ steps.gitversion.outputs.CommitsSinceVersionSource }}" }
            };
            root.jobs.Add("build", buildJob);

            string yaml = GitHubActionsSerialization.Serialize(root);

            if (Directory.Exists(workingDirectory) == false)
            {
                log.Append("Create directory " + workingDirectory);
                Directory.CreateDirectory(workingDirectory);
            }
            if (Directory.Exists(workingDirectory + "\\.github") == false)
            {
                log.Append("Create directory " + workingDirectory + "\\.github");
                Directory.CreateDirectory(workingDirectory + "\\.github");
            }
            if (Directory.Exists(workingDirectory + "\\.github\\workflows") == false)
            {
                log.Append("Create directory " + workingDirectory + "\\.github\\workflows");
                Directory.CreateDirectory(workingDirectory + "\\.github\\workflows");
            }
            log.Append("Writing workflow to file: " + workingDirectory + "\\.github\\workflows\\workflow.yml");
            File.WriteAllText(workingDirectory + "\\.github\\workflows\\workflow.yml", yaml);

            return log.ToString();
        }

    }
}
