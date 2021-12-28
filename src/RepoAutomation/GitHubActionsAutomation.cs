using System.Text;

namespace RepoAutomation
{
    public static class GitHubActionsAutomation
    {
        public static string SetupAction(string workingDirectory, string workingTempDirectory, string[]? urls)
        {
            StringBuilder log = new();
            if (Directory.Exists(workingTempDirectory) == false)
            {
                Directory.CreateDirectory(workingTempDirectory);
            }

            ////Clone the code from the repo
            //log.Append(CommandLine.RunCommand("git", 
            //    "clone " + repoLocation,
            //    workingDirectory));

            ////Create a src folder
            //string workingSrcDirectory = workingDirectory + "/src";
            //if (Directory.Exists(workingSrcDirectory) == false)
            //{
            //    Directory.CreateDirectory(workingSrcDirectory);
            //}

            ////Create a .NET tests project in the src folder
            //string testsProject = projectName + ".Tests";
            //log.Append(CommandLine.RunCommand("dotnet",
            //    "new mstest -n " + testsProject,
            //    workingSrcDirectory));

            ////Create a .NET web app project in the src folder
            //string webAppProject = projectName + ".Web";
            //log.Append(CommandLine.RunCommand("dotnet",
            //    "new webapp -n " + webAppProject,
            //    workingSrcDirectory));

            ////Create the solution file in the src folder
            //string solutionName = projectName;
            //log.Append(CommandLine.RunCommand("dotnet",
            //    "new sln --name " + solutionName,
            //    workingSrcDirectory));

            ////Bind the previously created projects to the solution
            //log.Append(CommandLine.RunCommand("dotnet",
            //    "sln add " + testsProject,
            //    workingSrcDirectory));
            //log.Append(CommandLine.RunCommand("dotnet",
            //    "sln add " + webAppProject,
            //    workingSrcDirectory));

            //string solutionText = System.IO.File.ReadAllText(workingSrcDirectory + "/" + solutionName + ".sln");
            //log.Append(solutionText);

            return log.ToString();
        }

    }
}
