using System.Diagnostics;
using System.Text;

namespace RepoAutomation
{
    public class CommandLine
    {
        public static string RunCommand(string command, string arguments, string workingDirectory =  null)
        {
            if (workingDirectory == null)
            {
                workingDirectory = Environment.CurrentDirectory;
            }
            Process? process = new();
            ProcessStartInfo? startInfo = new()
            {
                WorkingDirectory = workingDirectory,
                FileName = command,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            process.StartInfo = startInfo;
            process.Start();
            StringBuilder sb = new();
            while (!process.StandardOutput.EndOfStream)
            {
                sb.Append(process.StandardOutput.ReadLine());
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();

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
