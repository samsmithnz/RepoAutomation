using Newtonsoft.Json;
using RepoAutomation.Models;
using System.Net.Http.Headers;
using System.Text;

namespace RepoAutomation.APIAccess;

public static class GitHubAPIAccess
{

    //https://docs.github.com/en/enterprise-cloud@latest/rest/reference/repos
    public async static Task<Repo?> GetRepo(string? clientId, string? clientSecret, string owner, string repo)
    {
        Repo? result = null;
        if (clientId != null && clientSecret != null)
        {
            string url = $"https://api.github.com/repos/{owner}/{repo}";
            string? response = await BaseAPIAccess.GetGitHubMessage(url, clientId, clientSecret);
            if (string.IsNullOrEmpty(response) == false)
            {
                dynamic? jsonObj = JsonConvert.DeserializeObject(response);
                result = JsonConvert.DeserializeObject<Repo>(jsonObj?.ToString());
                result.RawJSON = jsonObj?.ToString();
            }
        }
        return result;
    }

    public async static Task<bool> CreateRepo(string? clientId, string? clientSecret, string repo,
        bool allowAutoMerge, bool deleteBranchOnMerge, bool allowRebaseMerge,
        bool isPrivate)
    {
        if (clientId != null && clientSecret != null)
        {
            var body = new
            {
                name = repo,
                allow_auto_merge = allowAutoMerge,
                delete_branch_on_merge = deleteBranchOnMerge,
                allow_rebase_merge = allowRebaseMerge,
                @private = isPrivate
            };
            StringContent content = new(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            string url = $"https://api.github.com/user/repos";
            await BaseAPIAccess.PostGitHubMessage(url, clientId, clientSecret, content);
            //string response = 
            //if (string.IsNullOrEmpty(response) == false)
            //{
            //    dynamic? jsonObj = JsonConvert.DeserializeObject(response);
            //    result = JsonConvert.DeserializeObject<Repo>(jsonObj?.ToString());
            //    result.RawJSON = jsonObj?.ToString();
            //}
        }
        return true;
    }

    public async static Task<bool> DeleteRepo(string? clientId, string? clientSecret, string owner, string repo)
    {
        if (clientId != null && clientSecret != null)
        {
            string url = $"https://api.github.com/repos/{owner}/{repo}";
            string? response = await BaseAPIAccess.DeleteGitHubMessage(url, clientId, clientSecret);
            if (string.IsNullOrEmpty(response) == true)
            {
                return false;
            }
            //    dynamic? jsonObj = JsonConvert.DeserializeObject(response);
            //    result = JsonConvert.DeserializeObject<Repo>(jsonObj?.ToString());
            //    result.RawJSON = jsonObj?.ToString();
            //}
        }
        return true;
    }

    public async static Task<BranchProtectionPolicy?> GetBranchProtectionPolicy(string? clientId, string? clientSecret,
        string owner, string repo, string branch)
    {
        BranchProtectionPolicy? result = null;
        if (clientId != null && clientSecret != null)
        {
            string url = $"https://api.github.com/repos/{owner}/{repo}/branches/{branch}/protection";
            string? response = await BaseAPIAccess.GetGitHubMessage(url, clientId, clientSecret);
            if (string.IsNullOrEmpty(response) == false)
            {
                dynamic? jsonObj = JsonConvert.DeserializeObject(response);
                result = JsonConvert.DeserializeObject<BranchProtectionPolicy>(jsonObj?.ToString());
                result.RawJSON = jsonObj?.ToString();
            }
        }
        return result;
    }

    public async static Task<bool> UpdateBranchProtectionPolicy(string? clientId, string? clientSecret, string owner, string repo,
        string branch, string[] contexts)
    {
        if (clientId != null && clientSecret != null)
        {
            var body = new
            {
                required_status_checks = new
                {
                    strict = true,
                    checks = new Check[]
                    {
                         new Check() {context=contexts[0]}
                         //new Check() {context=contexts[1]},
                         //new Check() {context=contexts[2]}
                    }
                },
                enforce_admins = true
            };
            string json = JsonConvert.SerializeObject(body);
            StringContent content = new(json, Encoding.UTF8, "application/json");
            string url = $"https://api.github.com/repos/{owner}/{repo}/branches/{branch}/protection";
            string? response = await BaseAPIAccess.PutGitHubMessage(url, clientId, clientSecret, content);
            if (string.IsNullOrEmpty(response) == true)
            {
                return false;
            }
        }
        return true;
    }

}