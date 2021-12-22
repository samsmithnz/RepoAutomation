using Newtonsoft.Json;
using RepoAutomation.Models;
using System.Net.Http.Headers;

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
            string response = await GetGitHubMessage(url, clientId, clientSecret);
            if (string.IsNullOrEmpty(response) == false)
            {
                dynamic? jsonObj = JsonConvert.DeserializeObject(response);
                result = JsonConvert.DeserializeObject<Repo>(jsonObj?.ToString());
                result.RawJSON = jsonObj?.ToString();
            }
        }
        return result;
    }

    public async static Task<bool> CreateRepo()
    {
        return false;
    }

    public async static Task<bool> DeleteRepo()
    {
        return false;
    }

    private async static Task<string> GetGitHubMessage(string url, string clientId, string clientSecret)
    {
        Console.WriteLine($"Running GitHub url: {url}");
        string responseBody = "";
        if (!url.Contains("api.github.com"))
        {
            throw new Exception("api.github.com missing from URL");
        }
        using (HttpClient client = new())
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("DevOpsMetrics", "0.1"));
            //If we use a id/secret, we significantly increase the rate from 60 requests an hour to 5000. https://developer.github.com/v3/#rate-limiting
            if (string.IsNullOrEmpty(clientId) == false && string.IsNullOrEmpty(clientSecret) == false)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", clientId, clientSecret))));
            }
            using (HttpResponseMessage response = await client.GetAsync(url))
            {
                //Throw a response exception
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    responseBody = await response.Content.ReadAsStringAsync();
                    //Console.WriteLine(responseBody);
                }
            }
        }
        return responseBody;
    }

}
