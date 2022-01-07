namespace RepoAutomation.Core.Models;

public class BranchProtectionPolicy : BaseModel
{
    public RequiredStatusCheck? required_status_checks { get; set; }
    public bool strict { get; set; }
    public Enforce? enforce_admins { get; set; }
    public Enforce? required_conversation_resolution { get; set; }
}

public class RequiredStatusCheck
{
    public Check[]? checks { get; set; }
}

public class Check
{
    public string? context { get; set; }
}

public class Enforce
{
    public bool enabled { get; set; }
}
