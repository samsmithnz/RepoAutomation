namespace RepoAutomation.Helpers
{
    public static class ReadmeAutomation
    {
        public static void AddStatusBadge(string workingDirectory, string repository)
        {
            string readmePath = workingDirectory + "\\README.md";
            if (File.Exists(readmePath) == true)
            {
                string contents = File.ReadAllText(readmePath);
                contents += Environment.NewLine;
                contents += $"[![CI/CD](https://github.com/samsmithnz/{repository}/actions/workflows/workflow.yml/badge.svg)](https://github.com/samsmithnz/{repository}/actions/workflows/workflow.yml)";
                contents += Environment.NewLine;
                File.WriteAllText(readmePath, contents);
            }
        }
    }
}
