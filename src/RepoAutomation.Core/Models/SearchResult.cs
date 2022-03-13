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