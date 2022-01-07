namespace RepoAutomation.Core.Models
{
    public class Release : BaseModel
    {
        public string? id { get; set; }
        public string? name { get; set; }
        public string? tag_name { get; set; }
        public Asset[]? assets { get; set; }
    }

    public class Asset
    {
        public string? name { get; set; }
        public string? browser_download_url { get; set; }
    }

}
