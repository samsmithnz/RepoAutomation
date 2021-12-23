using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RepoAutomation
{
    public static class ProjectAutomation
    {
        public static string SetupProject(string projectName, string workingDirectory)
        {
            StringBuilder log = new();
            Directory.CreateDirectory(workingDirectory);
            Directory.CreateDirectory(workingDirectory + "/src");

            string testsProject = projectName + ".Tests";
            log.Append(CommandLine.RunCommand("dotnet",
                "new mstest -n " + testsProject,
                workingDirectory + "/src"));

            string webAppProject = projectName + ".Web";
            log.Append(CommandLine.RunCommand("dotnet",
                "new webapp -n " + webAppProject,
                workingDirectory + "/src"));

            //Create the solution
            string solutionName = projectName;
            log.Append(CommandLine.RunCommand("dotnet",
                "new sln --name " + solutionName,
                workingDirectory + "/src"));

            //Bind the projects to the solution
            log.Append(CommandLine.RunCommand("dotnet",
                "sln add " + testsProject,
                workingDirectory + "/src"));
            log.Append(CommandLine.RunCommand("dotnet",
                "sln add " + webAppProject,
                workingDirectory + "/src"));

            return log.ToString();

            //$ProjectName = "RepoAutomationTest"

            //dir
            //cd\
            //cd \users\samsm\source\repos
            //dir
            //mkdir $ProjectName
            //cd $ProjectName
            //clone 
            //mkdir src
            //cd src
            //dotnet new mstest -n "$ProjectName.Tests"
            //dotnet new webapp -n "$ProjectName.Web"
            //dotnet new sln --name "$ProjectName"
            //dotnet sln add "$ProjectName.Tests"
            //dotnet sln add "$ProjectName.Web"
        }

    }
}
