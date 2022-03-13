using Newtonsoft.Json;
using RepoAutomation.Core.Models;
using System.Text;
using System.Web;

namespace RepoAutomation.Core.APIAccess;

public static class GitHubAPIAccess
{

    //https://docs.github.com/en/enterprise-cloud@latest/rest/reference/repos
    /// <summary>
    /// Get a single repo
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="clientSecret"></param>
    /// <param name="owner"></param>
    /// <param name="repo"></param>
    /// <returns></returns>
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

    //https://docs.github.com/en/rest/reference/repos#list-repositories-for-a-user
    /// <summary>
    /// Get a list of repos
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="clientSecret"></param>
    /// <param name="owner"></param>
    /// <returns></returns>
    public async static Task<List<Repo>?> GetRepos(string? clientId, string? clientSecret, string owner)
    {
        List<Repo>? result = new();
        if (clientId != null && clientSecret != null)
        {
            string url = $"https://api.github.com/users/{owner}/repos";
            string? response = await BaseAPIAccess.GetGitHubMessage(url, clientId, clientSecret, false);
            if (string.IsNullOrEmpty(response) == false &&
                response != @"{""message"":""Not Found"",""documentation_url"":""https://docs.github.com/rest/reference/repos#get-a-repository""}")
            {
                dynamic? jsonObj = JsonConvert.DeserializeObject(response);
                string? jsonString = jsonObj?.ToString();
                if (jsonString != null)
                {
                    result = JsonConvert.DeserializeObject<List<Repo>>(jsonString);
                }
                //Commented out because we are returning a list, and there is no RawJSON property on the list
                //result.RawJSON = jsonObj?.ToString();
            }
        }
        return result;
    }

    /// <summary>
    /// Create a new repo
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="clientSecret"></param>
    /// <param name="repo"></param>
    /// <param name="allowAutoMerge"></param>
    /// <param name="deleteBranchOnMerge"></param>
    /// <param name="allowRebaseMerge"></param>
    /// <param name="isPrivate"></param>
    /// <param name="gitIgnoreTemplate"></param>
    /// <returns></returns>
    public async static Task<bool> CreateRepo(string? clientId, string? clientSecret,
        string repo,
        bool allowAutoMerge,
        bool deleteBranchOnMerge,
        bool allowRebaseMerge,
        bool isPrivate,
        string gitIgnoreTemplate = "VisualStudio")
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
                gitignore_template = gitIgnoreTemplate,

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

    /// <summary>
    /// Delete the repo
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="clientSecret"></param>
    /// <param name="owner"></param>
    /// <param name="repo"></param>
    /// <param name="processErrors"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Get a list of all files at a path
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="clientSecret"></param>
    /// <param name="owner"></param>
    /// <param name="repo"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public async static Task<GitHubFile[]?> GetFiles(string? clientId, string? clientSecret,
        string owner, string repo, string path)
    {
        GitHubFile[]? result = null;
        if (clientId != null && clientSecret != null)
        {
            path = HttpUtility.UrlEncode(path);
            string url = $"https://api.github.com/repos/{owner}/{repo}/contents/{path}";
            string? response = await BaseAPIAccess.GetGitHubMessage(url, clientId, clientSecret, false);
            if (string.IsNullOrEmpty(response) == false && response.Contains(@"""message"":""Not Found""") == false)
            {
                dynamic? jsonObj = JsonConvert.DeserializeObject(response);
                result = JsonConvert.DeserializeObject<GitHubFile[]>(jsonObj?.ToString());
            }
        }
        return result;
    }

    /// <summary>
    /// Get a file and it's contents
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="clientSecret"></param>
    /// <param name="owner"></param>
    /// <param name="repo"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public async static Task<GitHubFile?> GetFile(string? clientId, string? clientSecret,
        string owner, string repo, string path)
    {
        GitHubFile? result = null;
        if (clientId != null && clientSecret != null)
        {
            path = HttpUtility.UrlEncode(path);
            string url = $"https://api.github.com/repos/{owner}/{repo}/contents/{path}";
            string? response = await BaseAPIAccess.GetGitHubMessage(url, clientId, clientSecret, false);
            if (string.IsNullOrEmpty(response) == false && response.Contains(@"""message"":""Not Found""") == false)
            {
                dynamic? jsonObj = JsonConvert.DeserializeObject(response);
                result = JsonConvert.DeserializeObject<GitHubFile>(jsonObj?.ToString());

                //Decode the Base64 file contents result
                if (result != null && result.content != null)
                {
                    byte[]? valueBytes = System.Convert.FromBase64String(result.content);
                    result.content = Encoding.UTF8.GetString(valueBytes);
                }
            }
        }
        return result;
    }

    /// <summary>
    /// Get the branch policy for a repo/branch
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="clientSecret"></param>
    /// <param name="owner"></param>
    /// <param name="repo"></param>
    /// <param name="branch"></param>
    /// <returns></returns>
    public async static Task<BranchProtectionPolicy?> GetBranchProtectionPolicy(string? clientId, string? clientSecret,
        string owner, string repo, string branch)
    {
        BranchProtectionPolicy? result = null;
        if (clientId != null && clientSecret != null)
        {
            string url = $"https://api.github.com/repos/{owner}/{repo}/branches/{branch}/protection";
            string? response = await BaseAPIAccess.GetGitHubMessage(url, clientId, clientSecret, false);
            if (string.IsNullOrEmpty(response) == false && response.Contains(@"""message"":""Branch not protected""") == false)
            {
                dynamic? jsonObj = JsonConvert.DeserializeObject(response);
                result = JsonConvert.DeserializeObject<BranchProtectionPolicy>(jsonObj?.ToString());
                result.RawJSON = jsonObj?.ToString();
            }
        }
        return result;
    }

    /// <summary>
    /// Update a branch policy for a repo/branch. Lots of assumptions/simplifications are made today. This definition WILL change.
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="clientSecret"></param>
    /// <param name="owner"></param>
    /// <param name="repo"></param>
    /// <param name="branch"></param>
    /// <param name="requiredStatusCheck"></param>
    /// <returns></returns>
    public async static Task<bool> UpdateBranchProtectionPolicy(string? clientId, string? clientSecret, string owner, string repo,
        string branch, RequiredStatusCheckPut? requiredStatusCheck)
    {
        if (clientId != null && clientSecret != null)
        {
            BranchProtectionPolicyPut body = new()
            {
                required_status_checks = requiredStatusCheck,
                required_pull_request_reviews = new()
                {
                    dismiss_stale_reviews = false,
                    required_approving_review_count = 0,
                    require_code_owner_reviews = false
                },
                restrictions = null,
                required_conversation_resolution = true,
                required_linear_history = false,
                enforce_admins = true,
                allow_force_pushes = false,
                allow_deletions = false
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

    /// <summary>
    /// Get the latest release for a repo
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="clientSecret"></param>
    /// <param name="owner"></param>
    /// <param name="repo"></param>
    /// <returns></returns>
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

    public async static Task<SearchResult?> SearchFiles(string? clientId, string? clientSecret,
        string owner, string repo, string extension)
    {
        SearchResult? result = null;
        if (clientId != null && clientSecret != null)
        {
            //"https://api.github.com/search/code?q=extension:js+repo:vnation/NewsAggregator";
            string url = $"https://api.github.com/search/code?q=extension:{extension}+repo:{owner}/{repo}";
            string? response = await BaseAPIAccess.GetGitHubMessage(url, clientId, clientSecret, false);
            if (string.IsNullOrEmpty(response) == false && response.Contains(@"""message"":""Not Found""") == false)
            {
                dynamic? jsonObj = JsonConvert.DeserializeObject(response);
                result = JsonConvert.DeserializeObject<SearchResult>(jsonObj?.ToString());
            }
        }
        return result;
    }

}
