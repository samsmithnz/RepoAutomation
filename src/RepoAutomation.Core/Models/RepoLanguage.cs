namespace RepoAutomation.Core.Models
{
    public class RepoLanguage
    {
        public string? Name { get; set; }
        public int Total { get; set; }
        /// <summary>
        /// Note that percent is a number with one decimal place from 1.0-100.0, not 0.0 to 1.0.
        /// </summary>
        public decimal Percent { get; set; }
        public string? Color { get; set; }
    }
}
