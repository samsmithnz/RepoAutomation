namespace RepoAutomation.Core.Models
{
    public class GitHubFile : BaseModel
    {
        public string? type { get; set; }
        public int? size { get; set; }
        public string? name { get; set; }
        public string? path { get; set; }
    }
}
