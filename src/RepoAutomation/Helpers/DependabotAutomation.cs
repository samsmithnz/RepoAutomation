using GitHubActionsDotNet.Helpers;
using System.Text;

namespace RepoAutomation.Helpers
{
    public static class DependabotAutomation
    {
        public static string SetupDependabotFile(string workingDirectory)
        {
            StringBuilder log = new();

            log.Append("Scanning repo for dependabot dependencies");
            List<string> files = FileSearch.GetFilesForDirectory(workingDirectory);

            log.Append("Generating dependabot configuration");
            string yaml = GitHubActionsDotNet.Serialization.DependabotSerialization.Serialize(workingDirectory, files);

            log.Append("Writing dependabot configuration to file: " + workingDirectory + "\\.github\\dependabot.yml");
            File.WriteAllText(workingDirectory + "\\.github\\dependabot.yml", yaml);

            return log.ToString();
        }

    }
}
