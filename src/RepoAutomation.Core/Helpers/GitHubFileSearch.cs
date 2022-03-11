using RepoAutomation.Core.APIAccess;
using RepoAutomation.Core.Models;

namespace RepoAutomation.Core.Helpers
{
    public static class GitHubFileSearch
    {
        public async static Task<List<string>?> SearchForFiles(string? id, string? secret,
            string owner, string repository, string? file, string? extension, string path)
        {
            GitHubFile[]? searchResult = await GitHubAPIAccess.GetFiles(id, secret,
                owner, repository, path);

            List<string> results = new();
            if (searchResult == null)
            {
                return null;
            }
            else
            {
                foreach (GitHubFile gitHubFile in searchResult)
                {
                    if (file != null && gitHubFile.name == file)
                    {
                        results.Add(gitHubFile.name);
                    }
                    else if (extension != null && gitHubFile.name != null)
                    {
                        string[] splitFileName = gitHubFile.name.Split(".");
                        if (splitFileName.Length > 0 && splitFileName[^1] == extension)
                        {
                            results.Add(gitHubFile.name);
                        }
                    }
                    else if (file == null && extension == null)
                    {
                        if (gitHubFile != null && gitHubFile.name != null)
                        {
                            results.Add(gitHubFile.name);
                        }
                    }
                }
            }

            return results;
        }

        public async static Task<GitHubFile> GetFileContents(string? id, string? secret,
            string owner, string repository, string filePath)
        {
            GitHubFile? file = await GitHubAPIAccess.GetFile(id, secret,
                owner, repository, filePath);

            if (file != null)
            {
                return file;
            }
            else
            {
                return null;
            }
        }
    }
}
