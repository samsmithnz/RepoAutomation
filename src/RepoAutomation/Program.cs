using CommandLine;

namespace RepoAutomation;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello world");
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
