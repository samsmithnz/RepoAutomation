namespace RepoAutomation.Models
{
    public class Repo
    {
        public string? id { get; set; }
        public string? name { get; set; }
        public string? full_name { get; set; }
        public string? url { get; set; }
        public string? default_branch { get; set; }
        public string? visibility { get; set; }
        public string? allow_rebase_merge { get; set; }
        public string? allow_squash_merge { get; set; }
        public string? allow_auto_merge { get; set; }
    }
}
