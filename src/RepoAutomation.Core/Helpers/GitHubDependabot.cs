﻿using GitHubActionsDotNet.Helpers;
using System.Text;

namespace RepoAutomation.Core.Helpers
{
    public static class GitHubDependabot
    {
        public static string SetupDependabotFile(string workingDirectory, string owner)
        {
            StringBuilder log = new();

            log.Append("Scanning repo for dependabot dependencies");
            List<string> files = FileSearch.GetFilesForDirectory(workingDirectory);

            log.Append("Generating dependabot configuration");
            string yaml = GitHubActionsDotNet.Serialization.DependabotSerialization.Serialize(workingDirectory, files,
                "daily",
                "06:00",
                "America/New_York",
                new() { owner },
                10,
                true,
                "core",
                new string[] { "*" },
                new string[] { "minor", "patch" });

            log.Append("Writing dependabot configuration to file: " + workingDirectory + "\\.github\\dependabot.yml");
            File.WriteAllText(workingDirectory + "\\.github\\dependabot.yml", yaml);

            return log.ToString();
        }

    }
}
