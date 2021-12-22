using Newtonsoft.Json;
using RepoAutomation.Models;
using System.Net.Http.Headers;
using System.Text;

namespace RepoAutomation.APIAccess;

public static class GitHubAPIAccess
{

    //https://docs.github.com/en/enterprise-cloud@latest/rest/reference/repos
    /// <summary>
    /// Get the target repo JSON
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="clientSecret"></param>
    /// <param name="owner">e.g. samsmithnz</param>
    /// <param name="repo">e.g. RepoAutomation</param>
    /// <returns>Repo objec</returns>
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="clientSecret"></param>
    /// <param name="owner"></param>
    /// <param name="repo"></param>
    /// <returns></returns>
    public async static Task<bool> CreateRepo(string? clientId, string? clientSecret, string repo)
    {
        if (clientId != null && clientSecret != null)
        {
            var body = new
            {
                name = repo
            };
            string url = $"https://api.github.com/user/repos";
            StringContent content = new(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
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
    /// 
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="clientSecret"></param>
    /// <param name="owner"></param>
    /// <param name="repo"></param>
    /// <returns></returns>
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

}