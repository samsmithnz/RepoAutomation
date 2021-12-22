using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RepoAutomation.APIAccess;

public static class GitHubAPIAccess
{

    //Call the GitHub Rest API to get a JSON array of repos
    public async static Task<Newtonsoft.Json.Linq.JArray?> GetRepo(string clientId, string clientSecret, string owner, string repo)
    {
        Newtonsoft.Json.Linq.JArray? list = null;
        string url = $"https://api.github.com/repos/{owner}/{repo}";
        string response = await GetGitHubMessage(url, clientId, clientSecret);
        if (string.IsNullOrEmpty(response) == false)
        {
            dynamic? jsonObj = JsonConvert.DeserializeObject(response);
            list = jsonObj?.workflow_runs;
        }
        return list;
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
