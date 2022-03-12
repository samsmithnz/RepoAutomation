namespace RepoAutomation.Core.Models;

public class BranchProtectionPolicy : BaseModel
{
    public RequiredStatusCheck? required_status_checks { get; set; }
    public Enforce? enforce_admins { get; set; }
    public Enforce? required_conversation_resolution { get; set; }
    public RequiredPullRequestReviews? required_pull_request_reviews { get; set; }
    public bool? restrictions { get; set; }
}

//Different structure when updating in PUT
public class BranchProtectionPolicyPut : BaseModel
{
    public RequiredStatusCheckPut? required_status_checks { get; set; }
    public bool? enforce_admins { get; set; }
    public bool? required_conversation_resolution { get; set; }
    public RequiredPullRequestReviews? required_pull_request_reviews { get; set; }
    public bool? restrictions { get; set; }
    public bool required_linear_history { get; set; }
    public bool allow_force_pushes { get; set; }
    public bool allow_deletions { get; set; }
}

public class RequiredStatusCheck
{
    public bool? strict { get; set; }
    public string[]? contexts { get; set; }
    public Check[]? checks { get; set; }
}

public class RequiredStatusCheckPut
{
    public bool? strict { get; set; }
    //NOTE: Don't include contexts property, it's deprecated
    public Check[]? checks { get; set; }
}

public class RequiredPullRequestReviews
{
    public bool dismiss_stale_reviews { get; set; }
    public int required_approving_review_count { get; set; }
    public bool require_code_owner_reviews { get; set; }
}

public class Check
{
    public string? context { get; set; }
}

public class Enforce
{
    public bool enabled { get; set; }
}
