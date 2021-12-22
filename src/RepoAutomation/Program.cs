using CommandLine;
using Microsoft.Extensions.Configuration;

namespace RepoAutomation;

public class Program
{
    public static void Main(string[] args)
    {
       //Load the appsettings.json configuration file
       IConfigurationBuilder? builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddUserSecrets<Program>();
        IConfigurationRoot configuration = builder.Build();
       
        Console.WriteLine("Hello world" + configuration["AppSettings:GitHubClientId"]);
        //Parse arguments
        string workingDirectory = Environment.CurrentDirectory;
        Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(o =>
        {
            if (string.IsNullOrEmpty(o.Directory) == false)
            {
                workingDirectory = o.Directory;
            }
        });

    }

    public class Options
    {
        [Option('d', "directory", Required = false, HelpText = "set working directory")]
        public string? Directory { get; set; }
    }
}
