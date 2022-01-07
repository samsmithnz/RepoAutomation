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
            //string workingDirectoryWithRepo = workingDirectory + "\\" + repository;

            //Clone the code from the repo
            Console.WriteLine("Cloning repo '" + repository + "' to " + workingDirectory);
            log.Append(CommandLine.RunCommand("git",
                "clone " + repoLocation,
                workingDirectory));

            return log.ToString();
        }

        public static string SetupDotnetProjects(string repository, string workingDirectory,
            string projectTypes)
        {
            StringBuilder log = new();

            //Split out the project types
            Dictionary<string, string> projectsToCreate = CreateProjectTypeArray(projectTypes);

            //the project folder in the working directory
            string workingDirectoryWithRepo = workingDirectory + "\\" + repository;

            //Create a src folder
            string workingSrcDirectory = workingDirectoryWithRepo + "/src";
            if (Directory.Exists(workingSrcDirectory) == false)
            {
                Console.WriteLine("Creating directory '" + workingSrcDirectory + "'");
                Directory.CreateDirectory(workingSrcDirectory);
            }

            //Create each project
            foreach (KeyValuePair<string, string> item in projectsToCreate)
            {
                Console.WriteLine("Creating " + item.Key + " project " + repository + item.Value);
                log.Append(CommandLine.RunCommand("dotnet",
                    "new " + item.Key + " -n " + repository + item.Value, //e.g. new mstest -n MyProject.Tests
                    workingSrcDirectory));
            }

            //Create the solution file in the src folder
            string solutionName = repository;
            Console.WriteLine("Creating solution");
            log.Append(CommandLine.RunCommand("dotnet",
                "new sln --name " + solutionName,
                workingSrcDirectory));

            //Bind the previously created projects to the solution
            foreach (KeyValuePair<string, string> item in projectsToCreate)
            {
                log.Append(CommandLine.RunCommand("dotnet",
                    "sln add " + repository + item.Value,
                    workingSrcDirectory));
            }

            string solutionText = File.ReadAllText(workingSrcDirectory + "/" + solutionName + ".sln");
            log.Append(solutionText);

            return log.ToString();
        }

        public static Dictionary<string, string> CreateProjectTypeArray(string projectTypes)
        {
            Dictionary<string, string> projectsToCreate = new();
            if (string.IsNullOrEmpty(projectTypes) == false)
            {
                string[] projectsArray = projectTypes.Replace(" ", "").Split(',');
                foreach (string project in projectsArray)
                {
                    switch (project)
                    {
                        case "mstest":
                        case "nunit":
                        case "nunit-test":
                        case "xunit":
                            projectsToCreate.Add(project, ".Tests");
                            break;

                        case "console":
                        case "classlib":
                            projectsToCreate.Add(project, "");
                            break;

                        case "wpf":
                            projectsToCreate.Add(project, ".WPF");
                            break;

                        case "winforms":
                            projectsToCreate.Add(project, ".Winforms");
                            break;

                        case "web":
                        case "mvc":
                        case "webapp":
                        case "razor":
                        case "angular":
                        case "react":
                            projectsToCreate.Add(project, ".Web");
                            break;


                        case "webapi":
                        case "grpc":
                            projectsToCreate.Add(project, ".Service");
                            break;

                        default:
                            throw new Exception(project + " is an unknown or currently unsupported .NET project type;");
                    }
                }
            }
            return projectsToCreate;
        }

    }
}
