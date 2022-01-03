using RepoAutomation.APIAccess;
using RepoAutomation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoAutomation.Helpers
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
                    if (extension != null && gitHubFile.name != null)
                    {
                        string[] splitFileName = gitHubFile.name.Split(".");
                        if (splitFileName.Length > 0 && splitFileName[splitFileName.Length - 1] == extension)
                        {
                            results.Add(gitHubFile.name);
                        }
                    }
                }
            }

            return results;
        }
    }
}
