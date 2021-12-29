using System.Text;

namespace RepoAutomation.Helpers
{
    public static class DotNetAutomation
    {
        public static string SetupProject(string repoLocation, string repository, string workingDirectory)
        {
            StringBuilder log = new();
            if (Directory.Exists(workingDirectory) == false)
            {
                Directory.CreateDirectory(workingDirectory);
            }
            //Create the project folder in the working directory
            string workingDirectoryWithRepo = workingDirectory + "\\" + repository;
            //if (Directory.Exists(workingDirectoryWithRepo) == false)
            //{
            //    Directory.CreateDirectory(workingDirectoryWithRepo);    
            //}

            //Clone the code from the repo
            log.Append(CommandLine.RunCommand("git",
                "clone " + repoLocation,
                workingDirectory));

            //Create a src folder
            string workingSrcDirectory = workingDirectoryWithRepo + "/src";
            if (Directory.Exists(workingSrcDirectory) == false)
            {
                Directory.CreateDirectory(workingSrcDirectory);
            }

            //Create a .NET tests project in the src folder
            string testsProject = repository + ".Tests";
            log.Append(CommandLine.RunCommand("dotnet",
                "new mstest -n " + testsProject,
                workingSrcDirectory));

            //Create a .NET web app project in the src folder
            string webAppProject = repository + ".Web";
            log.Append(CommandLine.RunCommand("dotnet",
                "new webapp -n " + webAppProject,
                workingSrcDirectory));

            //Create the solution file in the src folder
            string solutionName = repository;
            log.Append(CommandLine.RunCommand("dotnet",
                "new sln --name " + solutionName,
                workingSrcDirectory));

            //Bind the previously created projects to the solution
            log.Append(CommandLine.RunCommand("dotnet",
                "sln add " + testsProject,
                workingSrcDirectory));
            log.Append(CommandLine.RunCommand("dotnet",
                "sln add " + webAppProject,
                workingSrcDirectory));

            string solutionText = System.IO.File.ReadAllText(workingSrcDirectory + "/" + solutionName + ".sln");
            log.Append(solutionText);

            return log.ToString();
        }

    }
}
