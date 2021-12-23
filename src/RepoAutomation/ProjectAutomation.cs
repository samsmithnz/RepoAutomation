using System.Text;

namespace RepoAutomation
{
    public static class ProjectAutomation
    {
        public static string SetupProject(string projectName, string workingDirectory)
        {
            StringBuilder log = new();
            Directory.CreateDirectory(workingDirectory);
            string workingSrcDirectory = workingDirectory + "/src";
            Directory.CreateDirectory(workingSrcDirectory);

            string testsProject = projectName + ".Tests";
            log.Append(CommandLine.RunCommand("dotnet",
                "new mstest -n " + testsProject,
                workingSrcDirectory));

            string webAppProject = projectName + ".Web";
            log.Append(CommandLine.RunCommand("dotnet",
                "new webapp -n " + webAppProject,
                workingSrcDirectory));

            //Create the solution
            string solutionName = projectName;
            log.Append(CommandLine.RunCommand("dotnet",
                "new sln --name " + solutionName,
                workingSrcDirectory));

            //Bind the projects to the solution
            log.Append(CommandLine.RunCommand("dotnet",
                "sln add " + testsProject,
                workingSrcDirectory));
            log.Append(CommandLine.RunCommand("dotnet",
                "sln add " + webAppProject,
                workingSrcDirectory));

            string solutionText = System.IO.File.ReadAllText(workingSrcDirectory + "/" + solutionName + ".sln");
            log.Append(solutionText);



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
