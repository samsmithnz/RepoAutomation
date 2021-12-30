namespace RepoAutomation.Helpers
{
    public static class GitVersionAutomation
    {
        public static void AddGitVersionFile(string workingDirectory, string startingVersion = "0.1.0")
        {
            string gitVersionPath = workingDirectory + "\\GitVersion.yml";
            if (File.Exists(gitVersionPath) == true)
            {
                string contents = File.ReadAllText(gitVersionPath);
                contents += "next-version: " + startingVersion;
                File.WriteAllText(gitVersionPath, contents);
            }
        }
    }
}
