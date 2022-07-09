namespace RepoAutomation.Core.Models
{
    public class Repo : BaseModel
    {
        public string? id { get; set; }
        public string? name { get; set; }
        public string? full_name { get; set; }
        public Owner? owner { get; set; }
        public string? description { get; set; }
        public string? default_branch { get; set; }
        public string? visibility { get; set; }
        public bool allow_rebase_merge { get; set; }
        public bool allow_squash_merge { get; set; }
        public bool allow_merge_commit { get; set; }
        public bool allow_auto_merge { get; set; }
        public bool delete_branch_on_merge { get; set; }
    }

    public class Owner
    {
        public string? login { get; set; }
    }
}
