namespace RepoAutomation.Core.Models
{
    public class SearchResult : BaseModel
    {
        public int? total_count { get; set; }
        public SearchItem[]? items { get; set; }
    }

    public class SearchItem : BaseModel
    {
        public string? name { get; set; }
        public string? path { get; set; }
        public float? score { get; set; }
    }

}


//{
//    "total_count": 12,
//  "incomplete_results": false,
//  "items": [
//    {
//        "login": "mojombo",
//      "id": 1,
//      "node_id": "MDQ6VXNlcjE=",
//      "avatar_url": "https://secure.gravatar.com/avatar/25c7c18223fb42a4c6ae1c8db6f50f9b?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png",
//      "gravatar_id": "",
//      "url": "https://api.github.com/users/mojombo",
//      "html_url": "https://github.com/mojombo",
//      "followers_url": "https://api.github.com/users/mojombo/followers",
//      "subscriptions_url": "https://api.github.com/users/mojombo/subscriptions",
//      "organizations_url": "https://api.github.com/users/mojombo/orgs",
//      "repos_url": "https://api.github.com/users/mojombo/repos",
//      "received_events_url": "https://api.github.com/users/mojombo/received_events",
//      "type": "User",
//      "score": 1,
//      "following_url": "https://api.github.com/users/mojombo/following{/other_user}",
//      "gists_url": "https://api.github.com/users/mojombo/gists{/gist_id}",
//      "starred_url": "https://api.github.com/users/mojombo/starred{/owner}{/repo}",
//      "events_url": "https://api.github.com/users/mojombo/events{/privacy}",
//      "site_admin": true
//    }
//  ]
//}