namespace RepoAutomation.Models;

public class BranchProtectionPolicy : BaseModel
{
    public RequiredStatusCheck? required_status_checks { get; set; }
}

public class RequiredStatusCheck
{
    public Check[]? checks { get; set; }
}

public class Check
{
    public string? context { get; set; }
}
