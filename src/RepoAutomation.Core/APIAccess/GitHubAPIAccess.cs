using Newtonsoft.Json;
using RepoAutomation.Core.Models;
using System.Text;
using System.Web;

namespace RepoAutomation.Core.APIAccess;

public static class GitHubAPIAccess
{

    //https://docs.github.com/en/enterprise-cloud@latest/rest/reference/repos
    public async static Task<Repo?> GetRepo(string? clientId, string? clientSecret, string owner, string repo)
    {
        Repo? result = null;
        if (clientId != null && clientSecret != null)
        {
            string url = $"https://api.github.com/repos/{owner}/{repo}";
            string? response = await BaseAPIAccess.GetGitHubMessage(url, clientId, clientSecret, false);
            if (string.IsNullOrEmpty(response) == false &&
                response != @"{""message"":""Not Found"",""documentation_url"":""https://docs.github.com/rest/reference/repos#get-a-repository""}")
            {
                dynamic? jsonObj = JsonConvert.DeserializeObject(response);
                result = JsonConvert.DeserializeObject<Repo>(jsonObj?.ToString());
                result.RawJSON = jsonObj?.ToString();
            }
        }
        return result;
    }

    public async static Task<bool> CreateRepo(string? clientId, string? clientSecret,
        string repo,
        bool allowAutoMerge,
        bool deleteBranchOnMerge,
        bool allowRebaseMerge,
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
                @private = isPrivate,
                auto_init = true,
                gitignore_template = "VisualStudio",

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

    public async static Task<bool> DeleteRepo(string? clientId, string? clientSecret, string owner, string repo, bool processErrors = true)
    {
        if (clientId != null && clientSecret != null)
        {
            string url = $"https://api.github.com/repos/{owner}/{repo}";
            string? response = await BaseAPIAccess.DeleteGitHubMessage(url, clientId, clientSecret, processErrors);
            if (string.IsNullOrEmpty(response) == true)
            {
                return false;
            }
        }
        return true;
    }

    //public async static Task<SearchResult?> GetSearchCode(string? clientId, string? clientSecret,
    //    string owner, string repo, string? file = null, string? extension = null, string? path = null)
    //{
    //    SearchResult? result = null;
    //    if (clientId != null && clientSecret != null)
    //    {
    //        StringBuilder sb = new();
    //        sb.Append($"repo:{owner}/{repo}");
    //        //if (file != null)
    //        //{
    //        //    sb.Append($"+filename:{file}");
    //        //}
    //        //if (extension != null)
    //        //{
    //        //    sb.Append($"+extension:{extension}");
    //        //}
    //        //if (path != null)
    //        //{
    //        //    sb.Append($"+path:{path}");
    //        //}
    //        string searchString = HttpUtility.UrlEncode(sb.ToString());
    //        string url = $"https://api.github.com/search/code?q={searchString}";
    //        string? response = await BaseAPIAccess.GetGitHubMessage(url, clientId, clientSecret);
    //        if (string.IsNullOrEmpty(response) == false)
    //        {
    //            dynamic? jsonObj = JsonConvert.DeserializeObject(response);
    //            result = JsonConvert.DeserializeObject<SearchResult>(jsonObj?.ToString());
    //            result.RawJSON = jsonObj?.ToString();
    //        }
    //    }
    //    return result;
    //}

    public async static Task<GitHubFile[]?> GetFiles(string? clientId, string? clientSecret,
        string owner, string repo, string path)
    {
        GitHubFile[]? result = null;
        if (clientId != null && clientSecret != null)
        {
            path = HttpUtility.UrlEncode(path);
            string url = $"https://api.github.com/repos/{owner}/{repo}/contents/{path}";
            string? response = await BaseAPIAccess.GetGitHubMessage(url, clientId, clientSecret);
            if (string.IsNullOrEmpty(response) == false)
            {
                dynamic? jsonObj = JsonConvert.DeserializeObject(response);
                result = JsonConvert.DeserializeObject<GitHubFile[]>(jsonObj?.ToString());
            }
        }
        return result;
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

    public async static Task<Release?> GetReleaseLatest(string? clientId, string? clientSecret,
        string owner, string repo)
    {
        Release? result = null;
        if (clientId != null && clientSecret != null)
        {
            string url = $"https://api.github.com/repos/{owner}/{repo}/releases/latest";
            string? response = await BaseAPIAccess.GetGitHubMessage(url, clientId, clientSecret);
            if (string.IsNullOrEmpty(response) == false)
            {
                dynamic? jsonObj = JsonConvert.DeserializeObject(response);
                result = JsonConvert.DeserializeObject<Release>(jsonObj?.ToString());
                result.RawJSON = jsonObj?.ToString();
            }
        }
        return result;
    }

    //    public async static Task<bool> UpdateBranchProtectionPolicy(string? clientId, string? clientSecret, string owner, string repo,
    //        string branch, string[] contexts)
    //    {
    //        if (clientId != null && clientSecret != null)
    //        {
    //            var body = new
    //            {
    //                required_status_checks = new
    //                {
    //                    strict = true,
    //                    contexts = new string[] { "version" },
    //                    checks = new Check[]
    //                    {
    //                         new Check() { context = "version" }
    //                    //     //new Check() {context=contexts[1]},
    //                    //     //new Check() {context=contexts[2]}
    //                    }
    //                },
    //                enforce_admins = true,
    //                required_pull_request_reviews = new
    //                {
    //                    dismiss_stale_reviews = true
    //                }
    //            };
    //            //string json = JsonConvert.SerializeObject(body);
    //            string json = @"
    //{
    //  ""url"": ""https://api.github.com/repos/samsmithnz/RepoAutomation/branches/main/protection"",
    //  ""required_status_checks"": {
    //    ""url"": ""https://api.github.com/repos/samsmithnz/RepoAutomation/branches/main/protection/required_status_checks"",
    //    ""strict"": true,
    //    ""contexts"": [
    //      ""version""
    //    ],
    //    ""contexts_url"": ""https://api.github.com/repos/samsmithnz/RepoAutomation/branches/main/protection/required_status_checks/contexts"",
    //    ""checks"": [
    //      {
    //        ""context"": ""version"",
    //        ""app_id"": 15368
    //      }
    //    ]
    //  },
    //  ""required_pull_request_reviews"": {
    //    ""url"": ""https://api.github.com/repos/samsmithnz/RepoAutomation/branches/main/protection/required_pull_request_reviews"",
    //    ""dismiss_stale_reviews"": false,
    //    ""require_code_owner_reviews"": false,
    //    ""required_approving_review_count"": 0
    //  },
    //  ""required_signatures"": {
    //    ""url"": ""https://api.github.com/repos/samsmithnz/RepoAutomation/branches/main/protection/required_signatures"",
    //    ""enabled"": false
    //  },
    //  ""enforce_admins"": {
    //    ""url"": ""https://api.github.com/repos/samsmithnz/RepoAutomation/branches/main/protection/enforce_admins"",
    //    ""enabled"": true
    //  },
    //  ""required_linear_history"": {
    //    ""enabled"": false
    //  },
    //  ""allow_force_pushes"": {
    //    ""enabled"": false
    //  },
    //  ""allow_deletions"": {
    //    ""enabled"": false
    //  },
    //  ""required_conversation_resolution"": {
    //    ""enabled"": false
    //  }
    //}
    //";
    //            StringContent content = new(json, Encoding.UTF8, "application/json");
    //            string url = $"https://api.github.com/repos/{owner}/{repo}/branches/{branch}/protection";
    //            string? response = await BaseAPIAccess.PutGitHubMessage(url, clientId, clientSecret, content);
    //            if (string.IsNullOrEmpty(response) == true)
    //            {
    //                return false;
    //            }
    //        }
    //        return true;
    //    }

}