namespace RepoAutomation.Core.Models;

public class BranchProtectionPolicy : BaseModel
{
    public RequiredStatusCheck? required_status_checks { get; set; }
    public bool strict { get; set; }
    public Enforce? enforce_admins { get; set; }
    public Enforce? required_conversation_resolution { get; set; }
    public bool? required_pull_request_reviews { get; set; }
    public bool? restrictions { get; set; }
}

//Different structure when updating in PUT
public class BranchProtectionPolicyPut : BaseModel
{
    public RequiredStatusCheckPut? required_status_checks { get; set; }
    public bool strict { get; set; }
    public bool? enforce_admins { get; set; }
    public bool? required_conversation_resolution { get; set; }
    public bool? required_pull_request_reviews { get; set; }
    public bool? restrictions { get; set; }
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
    public string[]? contexts { get; set; }
    public string[]? checks { get; set; }
}

public class Check
{
    public string? context { get; set; }
}

public class Enforce
{
    public bool enabled { get; set; }
}
