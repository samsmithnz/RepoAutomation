﻿namespace RepoAutomation.Core.Helpers
{
    public static class GitVersion
    {
        public static void AddGitVersionFile(string workingDirectory, string startingVersion = "0.1.0")
        {
            string gitVersionPath = workingDirectory + "\\GitVersion.yml";
            string contents = "next-version: " + startingVersion;
            File.WriteAllText(gitVersionPath, contents);
        }
    }
}
