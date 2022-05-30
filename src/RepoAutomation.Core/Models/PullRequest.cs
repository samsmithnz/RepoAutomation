using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoAutomation.Core.Models
{
    public class PullRequest
    {
        public string? Title { get; set; }
        public string? Labels { get; set; }
        public bool? IsDependabotPR { get; set; }
        public bool? Approved { get; set; }
        public string Status { get; set; }
    }
}
