using Newtonsoft.Json;
using RepoAutomation.Core.Models;
using System.Text;
using System.Web;

namespace RepoAutomation.Core.APIAccess;

public static class GitHubApiAccess
{
    private static bool ProcessGitHubHTTPErrors = false;

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
            string? response = await BaseApiAccess.GetGitHubMessage(url, clientId, clientSecret, ProcessGitHubHTTPErrors);
            if (!string.IsNullOrEmpty(response) &&
                response != @"{""message"":""Not Found"",""documentation_url"":""https://docs.github.com/rest/repos/repos#get-a-repository"",""status"":""404""}")
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
            string? response = await BaseApiAccess.GetGitHubMessage(url, clientId, clientSecret, ProcessGitHubHTTPErrors);
            if (!string.IsNullOrEmpty(response) &&
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
            await BaseApiAccess.PostGitHubMessage(url, clientId, clientSecret, content);
        }
        return true;
    }

    /// <summary>
    /// Update a new repo
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="clientSecret"></param>
    /// <param name="owner"></param>
    /// <param name="repo"></param>
    /// <param name="allowAutoMerge"></param>
    /// <param name="deleteBranchOnMerge"></param>
    /// <param name="allowRebaseMerge"></param>
    /// <param name="isPrivate"></param>
    /// <returns></returns>
    public async static Task<bool> UpdateRepo(string? clientId, string? clientSecret,
        string owner,
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
                auto_init = true
            };
            StringContent content = new(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            //https://docs.github.com/en/rest/repos/repos#update-a-repository
            string url = $"https://api.github.com/repos/{owner}/{repo}";
            await BaseApiAccess.PatchGitHubMessage(url, clientId, clientSecret, content);
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
            string? response = await BaseApiAccess.DeleteGitHubMessage(url, clientId, clientSecret, processErrors);
            if (string.IsNullOrEmpty(response))
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
            string? response = await BaseApiAccess.GetGitHubMessage(url, clientId, clientSecret, ProcessGitHubHTTPErrors);
            if (!string.IsNullOrEmpty(response) &&
                !response.Contains(@"""message"":""Not Found"""))
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
        path = HttpUtility.UrlEncode(path);
        string url = $"https://api.github.com/repos/{owner}/{repo}/contents/{path}";
        string? response = await BaseApiAccess.GetGitHubMessage(url, clientId, clientSecret, ProcessGitHubHTTPErrors);
        if (!string.IsNullOrEmpty(response) &&
            !response.Contains(@"""message"":""Not Found"""))
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
            string? response = await BaseApiAccess.GetGitHubMessage(url, clientId, clientSecret, ProcessGitHubHTTPErrors);
            if (!string.IsNullOrEmpty(response) &&
                !response.Contains(@"""message"":""Branch not protected"""))
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
            string? response = await BaseApiAccess.PutGitHubMessage(url, clientId, clientSecret, content);
            if (string.IsNullOrEmpty(response))
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
            string? response = await BaseApiAccess.GetGitHubMessage(url, clientId, clientSecret, ProcessGitHubHTTPErrors);
            if (!string.IsNullOrEmpty(response))
            {
                dynamic? jsonObj = JsonConvert.DeserializeObject(response);
                result = JsonConvert.DeserializeObject<Release>(jsonObj?.ToString());
                result.RawJSON = jsonObj?.ToString();
            }
            //Check if the release is effectively empty - if so, mark it as null
            if (result != null && result.name == null)
            {
                result = null;
            }
        }
        return result;
    }

    //IMPORTANT: Note that search has a rate limit of 30 requests per minute: https://docs.github.com/en/rest/reference/search#rate-limit
    public async static Task<SearchResult?> SearchFiles(string? clientId, string? clientSecret,
        string owner, string repo, string? extension = null, string? fileName = null, int counter = 0)
    {
        SearchResult? result = new();
        if (clientId != null && clientSecret != null)
        {
            string url = "";
            if (extension != null)
            {
                //"https://api.github.com/search/code?q=extension:js+repo:vnation/NewsAggregator";
                url = $"https://api.github.com/search/code?q=extension:{extension}+repo:{owner}/{repo}";
            }
            else if (fileName != null)
            {
                //https://github.com/search?q=user%3Asamsmithnz+ProjectVersion.txt+filename%3AProjectVersion.txt&type=Repositories&ref=advsearch&l=&l=
                url = $"https://api.github.com/search/code?q=filename%3A{fileName}+repo:{owner}/{repo}";
            }
            if (!string.IsNullOrEmpty(url))
            {
                string? response = await BaseApiAccess.GetGitHubMessage(url, clientId, clientSecret, ProcessGitHubHTTPErrors);
                if (!string.IsNullOrEmpty(response))
                {
                    dynamic? jsonObj = JsonConvert.DeserializeObject(response);
                    result = JsonConvert.DeserializeObject<SearchResult>(jsonObj?.ToString());
                }
                result.RawJSON = response;
            }
        }
        if (result?.incomplete_results == true && counter < 3)
        {
            counter++;
            result = await SearchFiles(clientId, clientSecret, owner, repo, extension, fileName, counter);
        }

        return result;
    }

    public async static Task<string?> GetLastCommit(string? clientId, string? clientSecret,
        string owner, string repo)
    {
        string? result = null;
        if (clientId != null && clientSecret != null)
        {
            //https://api.github.com/repos/torvalds/linux/commits?per_page=1
            string url = $"https://api.github.com/repos/{owner}/{repo}/commits?per_page=1";
            string? response = await BaseApiAccess.GetGitHubMessage(url, clientId, clientSecret, ProcessGitHubHTTPErrors);
            if (!string.IsNullOrEmpty(response))
            {
                dynamic? jsonObj = JsonConvert.DeserializeObject(response);
                Commit[] commits = JsonConvert.DeserializeObject<Commit[]>(jsonObj?.ToString());
                if (commits != null && commits.Length > 0)
                {
                    result = commits[0].sha;
                }
            }
        }
        return result;
    }

    public async static Task<List<PullRequest>> GetPullRequests(string? clientId, string? clientSecret,
        string owner, string repo, bool onlyGetDependabotPRs = false)
    {
        List<PullRequest>? pullRequests = new();
        if (clientId != null && clientSecret != null)
        {
            //https://docs.github.com/en/rest/pulls/pulls#list-pull-requests (only first 30)
            string url = $"https://api.github.com/repos/{owner}/{repo}/pulls?state=open";
            string? response = await BaseApiAccess.GetGitHubMessage(url, clientId, clientSecret, ProcessGitHubHTTPErrors);
            if (!string.IsNullOrEmpty(response))
            {
                dynamic? jsonObj = JsonConvert.DeserializeObject(response);
                PR[] prs = JsonConvert.DeserializeObject<PR[]>(jsonObj?.ToString());
                if (prs != null && prs.Length > 0)
                {
                    foreach (PR pr in prs)
                    {
                        PullRequest newPullRequest = new()
                        {
                            Number = pr.number,
                            Title = pr.title,
                            State = pr.state,
                            LoginUser = pr?.user?.login
                        };
                        if (pr != null)
                        {
                            if (pr.updated_at != null)
                            {
                                newPullRequest.LastUpdated = DateTime.Parse(pr.updated_at);
                            }
                            //if (pr.auto_merge == null)
                            //{
                            //    newPullRequest.AutoMergeEnabled = false;
                            //}
                            //else
                            //{
                            //    newPullRequest.AutoMergeEnabled = bool.Parse(pr.auto_merge);
                            //}
                            if (pr.labels != null)
                            {
                                foreach (Label item in pr.labels)
                                {
                                    if (item != null && item.name != null)
                                    {
                                        newPullRequest.Labels.Add(item.name);
                                        if (item.name == "dependencies")
                                        {
                                            newPullRequest.IsDependabotPR = true;
                                        }
                                    }
                                }
                            }
                        }
                        if (onlyGetDependabotPRs &&
                            newPullRequest.IsDependabotPR != null &&
                            (bool)newPullRequest.IsDependabotPR)
                        {
                            pullRequests.Add(newPullRequest);
                        }
                        else if (onlyGetDependabotPRs == false)
                        {
                            pullRequests.Add(newPullRequest);
                        }
                    }
                }
            }
        }
        return pullRequests;
    }

    public async static Task<List<PRReview>> GetPullRequestReview(string? clientId, string? clientSecret,
        string owner, string repo, string pullRequestNumber)
    {
        List<PRReview> prReview = new();
        if (clientId != null && clientSecret != null)
        {
            //https://docs.github.com/en/rest/pulls/reviews#submit-a-pull-request-review
            string url = $"https://api.github.com/repos/{owner}/{repo}/pulls/{pullRequestNumber}/reviews";
            string? response = await BaseApiAccess.GetGitHubMessage(url, clientId, clientSecret, ProcessGitHubHTTPErrors);
            if (!string.IsNullOrEmpty(response))
            {
                dynamic? jsonObj = JsonConvert.DeserializeObject(response);
                prReview = JsonConvert.DeserializeObject<List<PRReview>>(jsonObj?.ToString());
            }
        }

        return prReview;
    }

    public async static Task<Dictionary<string, int>> GetRepoLanguages(string? clientId, string? clientSecret,
        string owner, string repo)
    {
        Dictionary<string, int> languages = new();
        if (clientId != null && clientSecret != null)
        {
            //https://docs.github.com/en/rest/repos/repos?apiVersion=2022-11-28#list-repository-languages
            string url = $"https://api.github.com/repos/{owner}/{repo}/languages";
            string? response = await BaseApiAccess.GetGitHubMessage(url, clientId, clientSecret, ProcessGitHubHTTPErrors);
            if (!string.IsNullOrEmpty(response))
            {
                dynamic? jsonObj = JsonConvert.DeserializeObject(response);
                languages = JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonObj?.ToString());
            }
        }

        return languages;
    }

    //Approve Pull Request
    public async static Task<bool> ApprovePullRequests(string? clientId, string? clientSecret,
        string owner, string repo, string approver)
    {
        bool result = false;
        bool onlyGetDependabotPRs = true;
        //Get the pull request details
        List<PullRequest> pullRequests = await GetPullRequests(clientId, clientSecret, owner, repo, onlyGetDependabotPRs);

        foreach (PullRequest pr in pullRequests)
        {
            if (pr != null && pr.State == "open" && pr.LoginUser != approver && pr.Number !=null)
            {
                List<PRReview> prReviews = await GetPullRequestReview(clientId, clientSecret, owner, repo, pr.Number.ToString());
                bool needsApproval = true;
                foreach (PRReview prReviewItem in prReviews)
                {
                    if (prReviewItem.state == "APPROVED")
                    {
                        needsApproval = false;
                        result = true;
                        break;
                    }
                }

                //Approve the pull request if it isn't approved (if the approver is not the author)
                if (needsApproval == true)
                {
                    //https://api.github.com/repos/OWNER/REPO/pulls/PULL_NUMBER/reviews
                    if (clientId != null && clientSecret != null)
                    {
                        var body = new
                        {
                            @event = "APPROVE" //Note that event is a reserved word and therefore needs the @prefix
                        };
                        //https://docs.github.com/en/rest/pulls/reviews?apiVersion=2022-11-28#create-a-review-for-a-pull-request
                        string url = $"https://api.github.com/repos/{owner}/{repo}/pulls/{pr.Number}/reviews";
                        StringContent content = new(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
                        string? response = await BaseApiAccess.PostGitHubMessage(url, clientId, clientSecret, content, ProcessGitHubHTTPErrors);
                        if (!string.IsNullOrEmpty(response))
                        {
                            dynamic? jsonObj = JsonConvert.DeserializeObject(response);
                            PRReview pullRequestReview = JsonConvert.DeserializeObject<PRReview>(jsonObj?.ToString());
                            if (pullRequestReview.submitted_at != null)
                            {
                                result = true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
        }

        return result;
    }

}
