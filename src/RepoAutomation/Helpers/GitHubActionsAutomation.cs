using GitHubActionsDotNet.Helpers;
using GitHubActionsDotNet.Models;
using GitHubActionsDotNet.Serialization;
using System.Text;

namespace RepoAutomation.Helpers
{
    public static class GitHubActionsAutomation
    {
        public static string SetupAction(string workingDirectory, string projectName,
            bool includeTestProject,
            bool includeClassLibraryProject,
            bool includeWebProject)
        {
            StringBuilder log = new();

            //Create the YAML
            GitHubActionsRoot root = CreateAction(projectName,
                includeTestProject,
                includeClassLibraryProject,
                includeWebProject);

            //Serialize to YAML
            string yaml = GitHubActionsSerialization.Serialize(root);

            //Save the YAML to a file
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

        private static GitHubActionsRoot CreateAction(string projectName, 
            bool includeTestProject, 
            bool includeClassLibraryProject, 
            bool includeWebProject)
        {
            JobHelper jobHelper = new();
            GitHubActionsRoot root = new();
            root.name = "CI/CD";
            root.on = TriggerHelper.AddStandardPushAndPullTrigger("main");

            string displayBuildGitVersionScript = @"
echo ""Version: ${{ steps.gitversion.outputs.SemVer }}""
echo ""CommitsSinceVersionSource: ${{ steps.gitversion.outputs.CommitsSinceVersionSource }}""";

            int stepCount = 0;
            Step[] buildSteps = new Step[] {
            CommonStepHelper.AddCheckoutStep(null,null,"0"),
            GitVersionStepHelper.AddGitVersionSetupStep(),
            GitVersionStepHelper.AddGitVersionDetermineVersionStep(),
            CommonStepHelper.AddScriptStep("Display GitVersion outputs", displayBuildGitVersionScript),
            DotNetStepHelper.AddDotNetSetupStep("Setup .NET","6.x"),
            DotNetStepHelper.AddDotNetTestStep(".NET test","src/"+projectName+".Tests/"+projectName+".Tests.csproj","Release",null,true),
            DotNetStepHelper.AddDotNetPublishStep(".NET publish","src/"+projectName+"/"+projectName+".csproj","Release",null,"-p:Version='${{ steps.gitversion.outputs.SemVer }}'", true),
            CommonStepHelper.AddUploadArtifactStep("Upload package back to GitHub","drop","src/"+projectName+"/bin/Release")
        };
            root.jobs = new();
            Job buildJob = jobHelper.AddJob(
                "Build job",
                "windows-latest",
                buildSteps);
            buildJob.outputs = new()
            {
                { "Version", "${{ steps.gitversion.outputs.SemVer }}" },
                { "CommitsSinceVersionSource", "${{ steps.gitversion.outputs.CommitsSinceVersionSource }}" }
            };
            root.jobs.Add("build", buildJob);
            return root;
        }

    }
}
