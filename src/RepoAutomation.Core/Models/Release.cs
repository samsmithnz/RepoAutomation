using System.Text;

namespace RepoAutomation.Core.Models
{
    public class Release : BaseModel
    {
        public string? id { get; set; }
        public string? name { get; set; }
        public string? tag_name { get; set; }
        public DateTime? published_at { get; set; }
        public string? url { get; set; }
        public Asset[]? assets { get; set; }

        public string Timing
        {
            get
            {
                if (published_at != null)
                {
                    StringBuilder sb = new();
                    sb.Append("(");
                    
                    sb.Append(") ");

                    return sb.ToString();
                }
                else
                {
                    return "";
                }
            }
        }
    }

    public class Asset
    {
        public string? name { get; set; }
        public string? browser_download_url { get; set; }
    }

}
