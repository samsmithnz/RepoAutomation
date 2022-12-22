using GitHubActionsDotNet.Helpers;
using GitHubActionsDotNet.Models;
using GitHubActionsDotNet.Serialization;
using System.Text;

namespace RepoAutomation.Core.Helpers
{
    public static class GitHubActions
    {
        public static string SetupAction(string workingDirectory, string projectName,
            string projectTypes)
        {
            StringBuilder log = new();

            //Create and Serialize to YAML
            string yaml = CreateActionYaml(projectName, projectTypes);

            //Save the YAML to a file
            if (!Directory.Exists(workingDirectory))
            {
                log.Append("Create directory " + workingDirectory);
                Directory.CreateDirectory(workingDirectory);
            }
            if (!Directory.Exists(workingDirectory + "\\.github"))
            {
                log.Append("Create directory " + workingDirectory + "\\.github");
                Directory.CreateDirectory(workingDirectory + "\\.github");
            }
            if (!Directory.Exists(workingDirectory + "\\.github\\workflows"))
            {
                log.Append("Create directory " + workingDirectory + "\\.github\\workflows");
                Directory.CreateDirectory(workingDirectory + "\\.github\\workflows");
            }
            log.Append("Writing workflow to file: " + workingDirectory + "\\.github\\workflows\\workflow.yml");
            File.WriteAllText(workingDirectory + "\\.github\\workflows\\workflow.yml", yaml);

            return log.ToString();
        }

        public static string CreateActionYaml(string projectName, string projectTypes)
        {
            //Split out the project types
            Dictionary<string, string> projectsToCreate = DotNet.CreateProjectTypeArray(projectTypes);

            JobHelper jobHelper = new();
            GitHubActionsRoot root = new();
            root.name = "CI/CD";
            root.on = TriggerHelper.AddStandardPushAndPullTrigger("main");

            string displayBuildGitVersionScript = @"
echo ""Version: ${{ steps.gitversion.outputs.SemVer }}""
echo ""CommitsSinceVersionSource: ${{ steps.gitversion.outputs.CommitsSinceVersionSource }}""";

            List<Step> steps = new()
            {
                CommonStepHelper.AddCheckoutStep(null, null, "0"),
                GitVersionStepHelper.AddGitVersionSetupStep(),
                GitVersionStepHelper.AddGitVersionDetermineVersionStep(),
                CommonStepHelper.AddScriptStep("Display GitVersion outputs", displayBuildGitVersionScript),
                DotNetStepHelper.AddDotNetSetupStep("Setup .NET", "7.0.x")
            };
            //Add all of the project types
            foreach (KeyValuePair<string, string> item in projectsToCreate)
            {
                if (item.Value == ".Tests")
                {
                    steps.Add(DotNetStepHelper.AddDotNetTestStep(".NET test", "src/" + projectName + item.Value + "/" + projectName + ".Tests.csproj", "Release", null, true));
                }
                else if (item.Value == "") //Console or library
                {
                    steps.Add(DotNetStepHelper.AddDotNetPublishStep(".NET publish", "src/" + projectName + "/" + projectName + item.Value + ".csproj", "Release", null, "-p:Version='${{ steps.gitversion.outputs.SemVer }}'", true));
                    steps.Add(CommonStepHelper.AddUploadArtifactStep("Upload package back to GitHub", "drop", "src/" + projectName + item.Value + "/bin/Release"));
                }
                else if (item.Value == ".Web") //Website
                {
                    steps.Add(DotNetStepHelper.AddDotNetPublishStep(".NET publish", "src/" + projectName + item.Value + "/" + projectName + item.Value + ".csproj", "Release", null, "-p:Version='${{ steps.gitversion.outputs.SemVer }}'", true));
                    steps.Add(CommonStepHelper.AddUploadArtifactStep("Upload package back to GitHub", "web", "src/" + projectName + item.Value + "/bin/Release"));
                }
                else if (item.Value == ".Service") //Service
                {
                    steps.Add(DotNetStepHelper.AddDotNetPublishStep(".NET publish", "src/" + projectName + item.Value + "/" + projectName + item.Value + ".csproj", "Release", null, "-p:Version='${{ steps.gitversion.outputs.SemVer }}'", true));
                    steps.Add(CommonStepHelper.AddUploadArtifactStep("Upload package back to GitHub", "service", "src/" + projectName + item.Value + "/bin/Release"));
                }
                else if (item.Value == ".WPF") //WPF
                {
                    steps.Add(DotNetStepHelper.AddDotNetPublishStep(".NET publish", "src/" + projectName + "/" + projectName + item.Value + ".csproj", "Release", null, "-p:Version='${{ steps.gitversion.outputs.SemVer }}'", true));
                    steps.Add(CommonStepHelper.AddUploadArtifactStep("Upload package back to GitHub", "wpf", "src/" + projectName + item.Value + "/bin/Release"));
                }
                else if (item.Value == ".Winforms") //Winforms
                {
                    steps.Add(DotNetStepHelper.AddDotNetPublishStep(".NET publish", "src/" + projectName + "/" + projectName + item.Value + ".csproj", "Release", null, "-p:Version='${{ steps.gitversion.outputs.SemVer }}'", true));
                    steps.Add(CommonStepHelper.AddUploadArtifactStep("Upload package back to GitHub", "winforms", "src/" + projectName + item.Value + "/bin/Release"));
                }
            }
            Step[] buildSteps = steps.ToArray();

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
            string yaml = GitHubActionsSerialization.Serialize(root);
            return yaml;
        }

    }
}
