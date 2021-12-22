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
             .AddUserSecrets<Program>();
        IConfigurationRoot configuration = builder.Build();

        //Parse arguments
        string workingDirectory = Environment.CurrentDirectory;
        Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(o =>
        {
            if (string.IsNullOrEmpty(o.Directory) == false)
            {
                workingDirectory = o.Directory;
            }
        });

        //Do the work
        string id = configuration["AppSettings:GitHubClientId"];
        string secret = configuration["AppSettings:GitHubClientSecret"];
        Repo? repo = await GitHubAPIAccess.GetRepo(id, secret, "samsmithnz", "RepoAutomation");
        Console.WriteLine("Hello world " + repo?.full_name);
    }

    public class Options
    {
        [Option('d', "directory", Required = false, HelpText = "set working directory")]
        public string? Directory { get; set; }
    }
}
