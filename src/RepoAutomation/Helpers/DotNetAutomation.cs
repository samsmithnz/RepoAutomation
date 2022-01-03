using System.Text;

namespace RepoAutomation.Helpers
{
    public static class DotNetAutomation
    {
        public static string CloneRepo(string repoLocation, string repository,
            string workingDirectory)
        {
            StringBuilder log = new();
            if (Directory.Exists(workingDirectory) == false)
            {
                Directory.CreateDirectory(workingDirectory);
            }
            //the project folder in the working directory
            string workingDirectoryWithRepo = workingDirectory + "\\" + repository;

            //Clone the code from the repo
            Console.WriteLine("Cloning repo '" + repository + "' to " + workingDirectory);
            log.Append(CommandLine.RunCommand("git",
                "clone " + repoLocation,
                workingDirectory));

            return log.ToString();
        }

        public static string SetupDotnetProjects(string repository, string workingDirectory,
            bool includeTests, bool includeClassLibrary, bool includeWeb)
        {
            StringBuilder log = new();

            //the project folder in the working directory
            string workingDirectoryWithRepo = workingDirectory + "\\" + repository;

            //Create a src folder
            string workingSrcDirectory = workingDirectoryWithRepo + "/src";
            if (Directory.Exists(workingSrcDirectory) == false)
            {
                Console.WriteLine("Creating directory '" + workingSrcDirectory + "'");
                Directory.CreateDirectory(workingSrcDirectory);
            }

            //Create a .NET tests project in the src folder
            string testsProject = repository + ".Tests";
            if (includeTests)
            {
                Console.WriteLine("Creating test project " + testsProject);
                log.Append(CommandLine.RunCommand("dotnet",
                    "new mstest -n " + testsProject,
                    workingSrcDirectory));
            }

            //Create a .NET class library project in the src folder
            string libraryProject = repository;
            if (includeClassLibrary)
            {
                Console.WriteLine("Creating class library project " + libraryProject);
                log.Append(CommandLine.RunCommand("dotnet",
                    "new classlib -n " + libraryProject,
                    workingSrcDirectory));
            }

            //Create a .NET web app project in the src folder
            string webAppProject = repository + ".Web";
            if (includeWeb)
            {
                Console.WriteLine("Creating web project " + webAppProject);
                log.Append(CommandLine.RunCommand("dotnet",
                    "new webapp -n " + webAppProject,
                    workingSrcDirectory));
            }

            //Create the solution file in the src folder
            string solutionName = repository;
            Console.WriteLine("Creating solution");
            log.Append(CommandLine.RunCommand("dotnet",
                "new sln --name " + solutionName,
                workingSrcDirectory));

            //Bind the previously created projects to the solution
            if (includeTests)
            {
                log.Append(CommandLine.RunCommand("dotnet",
                "sln add " + testsProject,
                workingSrcDirectory));
            }
            if (includeClassLibrary)
            {
                log.Append(CommandLine.RunCommand("dotnet",
                "sln add " + libraryProject,
                workingSrcDirectory));
            }
            if (includeWeb)
            {
                log.Append(CommandLine.RunCommand("dotnet",
                "sln add " + webAppProject,
                workingSrcDirectory));
            }

            string solutionText = File.ReadAllText(workingSrcDirectory + "/" + solutionName + ".sln");
            log.Append(solutionText);

            return log.ToString();
        }

    }
}
