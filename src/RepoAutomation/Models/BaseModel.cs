namespace RepoAutomation.Models
{
    public class BaseModel
    {
        public string? RawJSON { get; set; }
        public bool IsSuccessfulResponse { get; set; }
        public int? StatusCode { get; set; }
        public string? Status { get; set; }
    }
}
