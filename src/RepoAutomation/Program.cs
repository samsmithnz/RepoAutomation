using CommandLine;
using Microsoft.Extensions.Configuration;
using RepoAutomation.APIAccess;
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

            //1. Create the repo (if it doesn't exist)
            Repo? repo = await GitHubAPIAccess.GetRepo(id, secret, owner, repository);
            if (repo == null)
            {
                await GitHubAPIAccess.CreateRepo(id, secret, repository, true, true, false, true);
            }

            //2. Clone the repo (create the working directory if it doesn't exist)
            if (Directory.Exists(workingDirectory) == false)
            {
                Directory.CreateDirectory(workingDirectory);
            }
            //git clone <repo> <directory>
            CommandLine.RunCommand("git",
                $"clone https://github.com/{owner}/{repository}",
                workingDirectory);

            //3. Create .NET projects
            //4. Create the GitHub Action
            //5. Create the Dependabot file
            //6. Push back to main
            //7. Set the branch policy

            Console.WriteLine("Hello world " + repo?.full_name);
        }
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
