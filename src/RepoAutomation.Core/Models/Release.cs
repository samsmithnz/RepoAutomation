using System.Text;

namespace RepoAutomation.Core.Models
{
    public class Release : BaseModel
    {
        public string? id { get; set; }
        public string? name { get; set; }
        public string? tag_name { get; set; }
        public DateTime? published_at { get; set; }
        public string? html_url { get; set; }
        public Asset[]? assets { get; set; }

        public string ToTimingString()
        {
            if (published_at != null)
            {
                StringBuilder sb = new();
                sb.Append(" (");
                TimeSpan span = DateTime.Now - (DateTime)published_at;
                if (span.TotalMinutes < 0) //Happens when the timezone messes with the published at date.
                {
                    sb.Append("a few moments");
                }
                else if (span.TotalMinutes < 60)
                {
                    sb.Append(span.TotalMinutes.ToString("0"));
                    sb.Append(" minutes");
                }
                else if (span.TotalHours < 24)
                {
                    sb.Append(span.TotalHours.ToString("0"));
                    sb.Append(" hours");
                }
                else if (span.TotalDays <= 30) //approximation
                {
                    sb.Append(span.TotalDays.ToString("0"));
                    sb.Append(" days");
                }
                else
                {
                    sb.Append((span.TotalDays / 30).ToString("0"));
                    sb.Append(" months");
                }
                sb.Append(" ago) ");

                return sb.ToString();
            }
            else
            {
                return "";
            }

        }

        public class Asset
        {
            public string? name { get; set; }
            public string? browser_download_url { get; set; }
        }

    }
}
