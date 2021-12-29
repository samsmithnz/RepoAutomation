﻿using CommandLine;
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
                await GitHubAPIAccess.CreateRepo(id, secret, repository, true, true, false, true);
            }

            //2. Clone the repo and create the .NET projects
            DotNetAutomation.SetupProject(repoURL, repository, workingDirectory);

            //3. Create the GitHub Action
            GitHubActionsAutomation.SetupAction(workingDirectory + "\\" + repository, repository);

            ////4. Create the Dependabot file
            //Asset[]? dependabotURLs = await GetReleaseAssets(id, secret, owner, "Dependabot-Configuration-Builder");
            //await DependabotAutomation.SetupDependabotFile(workingDirectory, workingTempDirectory, dependabotURLs);

            //6. Push back to main
            Console.WriteLine(Helpers.CommandLine.RunCommand("git", "add .", workingDirectory + "\\" + repository));
            Console.WriteLine(Helpers.CommandLine.RunCommand("git", @"commit -m""Created .NET projects, setup action, and created dependabot configuration""", workingDirectory + "\\" + repository));
            Console.WriteLine(Helpers.CommandLine.RunCommand("git", "push", workingDirectory + "\\" + repository));

            //7. Set the branch policy

            Console.WriteLine("Hello world " + repo?.full_name);
        }
    }


    private static async Task<Asset[]?> GetReleaseAssets(string id, string secret, string owner, string repoName)
    {
        Asset[]? assets = null;
        Release? release = await GitHubAPIAccess.GetReleaseLatest(id, secret, owner, repoName);
        if (release != null && release.assets != null && release.assets.Length > 0)
        {
            assets = release.assets;
        }
        return assets;
    }

    public class Options
    {
        [Option('d', "directory", Required = false, HelpText = "set working directory")]
        public string? Directory { get; set; }

        [Option('o', "owner", Required = false, HelpText = "GitHub owner")]
        public string? Owner { get; set; }

        [Option('r', "repo", Required = false, HelpText = "new repo name")]
        public string? Repo { get; set; }

    }
}
