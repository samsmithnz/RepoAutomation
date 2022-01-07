using CommandLine;
using Microsoft.Extensions.Configuration;
using RepoAutomation.Core.APIAccess;
using RepoAutomation.Core.Helpers;
using RepoAutomation.Core.Models;

namespace RepoAutomation;

public class Program
{
    public async static Task Main(string[] args)
    {
        //Load the appsettings.json configuration file
        IConfigurationBuilder? builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false)
             .AddUserSecrets<Program>(true);
        IConfigurationRoot configuration = builder.Build();

        //Parse arguments
        string workingDirectory = Environment.CurrentDirectory;
        string workingTempDirectory = workingDirectory + "\\temp";
        string owner = "";
        string repository = "";
        string projectTypes = "";
        bool isPrivate = true;
        Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(o =>
        {
            if (string.IsNullOrEmpty(o.Directory) == false)
            {
                workingDirectory = o.Directory;
            }
            if (string.IsNullOrEmpty(o.Owner) == false)
            {
                owner = o.Owner;
            }
            if (string.IsNullOrEmpty(o.Repo) == false)
            {
                repository = o.Repo;
            }
            if (string.IsNullOrEmpty(o.Visibility) == false)
            {
                if (o.Visibility.ToLower() == "public")
                {
                    isPrivate = false;
                }
                else if (o.Visibility.ToLower() == "private")
                {
                    isPrivate = true;
                }
            }
            else
            {
                isPrivate = true;
            }
            if (string.IsNullOrEmpty(o.ProjectTypes) == false)
            {
                projectTypes = o.ProjectTypes;
            }
        });

        //Do the work
        if (string.IsNullOrEmpty(workingDirectory) == false &&
            string.IsNullOrEmpty(owner) == false &&
            string.IsNullOrEmpty(repository) == false)
        {
            string id = configuration["AppSettings:GitHubClientId"];
            string secret = configuration["AppSettings:GitHubClientSecret"];
            string repoURL = $"https://github.com/{owner}/{repository}";

            //1. Create the repo (if it doesn't exist)
            Repo? repo = await GitHubAPIAccess.GetRepo(id, secret, owner, repository);
            if (repo == null)
            {
                Console.WriteLine("Creating repo: " + repository);
                await GitHubAPIAccess.CreateRepo(id,
                    secret,
                    repository,
                    true,
                    true,
                    false,
                    isPrivate);
            }

            //2. Clone the repo
            DotNetAutomation.CloneRepo(repoURL, repository, workingDirectory);

            //3. create the .NET projects
            if (string.IsNullOrEmpty(projectTypes) == false)
            {
                DotNetAutomation.SetupDotnetProjects(repository, workingDirectory,
                    projectTypes);
            }

            //4. Create the GitHub Action
            Console.WriteLine("Creating GitHub Action");
            GitHubActionsAutomation.SetupAction(workingDirectory + "\\" + repository,
                repository,
                projectTypes);

            //5. Create the Dependabot file
            Console.WriteLine("Create Dependabot configuration");
            DependabotAutomation.SetupDependabotFile(workingDirectory + "\\" + repository, owner);

            //6. Customize the README.md file
            Console.WriteLine("Adding GitHub Actions status badge to README.md file");
            ReadmeAutomation.AddStatusBadge(workingDirectory + "\\" + repository, repository);

            //7. Add GitVersion file
            Console.WriteLine("Adding GitVersion.yml file");
            GitVersionAutomation.AddGitVersionFile(workingDirectory + "\\" + repository, "0.1.0");

            //8. Push back to main         
            Console.WriteLine(Core.Helpers.CommandLine.RunCommand("git", "add .", workingDirectory + "\\" + repository));
            Console.WriteLine(Core.Helpers.CommandLine.RunCommand("git", @"commit -m""Created .NET projects, setup action, and created dependabot configuration""", workingDirectory + "\\" + repository));
            Console.WriteLine(Core.Helpers.CommandLine.RunCommand("git", "push", workingDirectory + "\\" + repository));

            //9. Set the branch policy
            //TODO: Set the branch policy
        }
    }


    //private static async Task<Asset[]?> GetReleaseAssets(string id, string secret, string owner, string repoName)
    //{
    //    Asset[]? assets = null;
    //    Release? release = await GitHubAPIAccess.GetReleaseLatest(id, secret, owner, repoName);
    //    if (release != null && release.assets != null && release.assets.Length > 0)
    //    {
    //        assets = release.assets;
    //    }
    //    return assets;
    //}

    public class Options
    {
        [Option('d', "directory", Required = false, HelpText = "set working directory")]
        public string? Directory { get; set; }

        [Option('o', "owner", Required = false, HelpText = "GitHub owner")]
        public string? Owner { get; set; }

        [Option('r', "repo", Required = false, HelpText = "new repo name")]
        public string? Repo { get; set; }

        [Option('v', "visibility", Required = false, HelpText = "new repo visibility. Can be set to private or public")]
        public string? Visibility { get; set; }

        [Option('p', "projecttypes", Required = false, HelpText = "dotnet project types, comma delimited, to create. See dotnet new docs for details: https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-new#arguments")]
        public string? ProjectTypes { get; set; }

    }
}
