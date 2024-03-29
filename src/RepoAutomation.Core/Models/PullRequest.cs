﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace RepoAutomation.Core.Models
{
    //Friendly processing class
    public class PullRequest
    {
        public PullRequest()
        {
            Labels = new();
        }
        public string? Number { get; set; }
        public string? Title { get; set; }
        public List<string> Labels { get; set; }
        public bool? IsDependabotPR { get; set; }
        public bool? Approved { get; set; }
        public string? State { get; set; }
        public bool AutoMergeEnabled { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string? LoginUser { get; set; }
    }

    //GitHub API class
    public class PR
    {
        public string? number { get; set; }
        public string? title { get; set; }
        public string? state { get; set; }
        public string? url { get; set; }
        public List<Label>? labels { get; set; }
        //public string? auto_merge { get; set; }
        public string? updated_at { get; set; }
        public PRUser? user { get; set; }
    }

    public class Label
    {
        public string? name { get; set; }
    }

    public class PRReview
    {
        public string? id { get; set; }
        public string? state { get; set; }
        public string? submitted_at { get; set; }
        public string? commit_id { get; set; }
    }
    
    public class PRUser
    {
        public string? login { get; set; }
    }
}
