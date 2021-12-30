using CommandLine;
using Microsoft.Extensions.Configuration;
using RepoAutomation.APIAccess;
using RepoAutomation.Helpers;
using RepoAutomation.Models;

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
        string workingTempDirectory = workingDirectory + "\temp";
        string owner = "";
        string repository = "";
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

            //2. Clone the repo and create the .NET projects
            bool includeTests = true;
            bool includeClassLibrary = true;
            bool includeWeb = false;
            DotNetAutomation.SetupProject(repoURL, repository, workingDirectory,
                includeTests, includeClassLibrary, includeWeb);

            //3. Create the GitHub Action
            Console.WriteLine("Creating GitHub Action");
            GitHubActionsAutomation.SetupAction(workingDirectory + "\\" + repository,
                repository,
                includeTests,
                includeClassLibrary,
                includeWeb);

            //4. Create the Dependabot file
            Console.WriteLine("Create Dependabot configuration");
            DependabotAutomation.SetupDependabotFile(workingDirectory + "\\" + repository, owner);

            //5. Customize the README.md file
            Console.WriteLine("Adding GitHub Actions status badge to README.md file");
            ReadmeAutomation.AddStatusBadge(workingDirectory + "\\" + repository);

            //6. Push back to main         
            Console.WriteLine(Helpers.CommandLine.RunCommand("git", "add .", workingDirectory + "\\" + repository));
            Console.WriteLine(Helpers.CommandLine.RunCommand("git", @"commit -m""Created .NET projects, setup action, and created dependabot configuration""", workingDirectory + "\\" + repository));
            Console.WriteLine(Helpers.CommandLine.RunCommand("git", "push", workingDirectory + "\\" + repository));

            //7. Set the branch policy

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

    }
}
