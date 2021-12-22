namespace RepoAutomation.Models;

public class BranchProtectionPolicy
{
    public RequiredStatusCheck? required_status_checks { get; set; }
    public string? RawJSON { get;set; }

}

public class RequiredStatusCheck
{
    public string[]? contexts { get; set; }
}
