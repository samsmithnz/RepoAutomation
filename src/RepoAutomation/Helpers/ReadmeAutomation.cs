namespace RepoAutomation.Helpers
{
    public static class ReadmeAutomation
    {
        public static bool AddStatusBadge(string workingDirectory)
        {
            string contents = File.ReadAllText(workingDirectory + "\\README.md");
            contents += Environment.NewLine;
            contents += "[![CI/CD](https://github.com/samsmithnz/RepoAutomationTest/actions/workflows/workflow.yml/badge.svg)](https://github.com/samsmithnz/RepoAutomationTest/actions/workflows/workflow.yml)";
            contents += Environment.NewLine; 
            File.WriteAllText(workingDirectory + "\\README.md", contents);
            return true;
        }
    }
}
