namespace RepoAutomation.Core.Models;

public class RepositoryRuleset : BaseModel
{
    public int id { get; set; }
    public string? name { get; set; }
    public string? target { get; set; }
    public string? source_type { get; set; }
    public string? source { get; set; }
    public string? enforcement { get; set; }
    public Conditions? conditions { get; set; }
    public Rule[]? rules { get; set; }
}

public class Conditions
{
    public RefName? ref_name { get; set; }
}

public class RefName
{
    public string[]? include { get; set; }
    public string[]? exclude { get; set; }
}

public class Rule
{
    public string? type { get; set; }
    public RuleParameters? parameters { get; set; }
}

public class RuleParameters
{
    // Branch protection parameters
    public bool? required_linear_history { get; set; }
    public bool? allow_force_pushes { get; set; }
    public bool? allow_deletions { get; set; }
    public bool? required_conversation_resolution { get; set; }
    
    // Pull request parameters
    public bool? dismiss_stale_reviews_on_push { get; set; }
    public bool? require_code_owner_review { get; set; }
    public int? required_approving_review_count { get; set; }
    
    // Status check parameters
    public bool? strict_required_status_checks_policy { get; set; }
    public RepositoryRuleStatusCheck[]? required_status_checks { get; set; }
}

public class RepositoryRuleStatusCheck
{
    public string? context { get; set; }
    public int? integration_id { get; set; }
}

// Model for creating/updating repository rulesets (PUT/POST operations)
public class RepositoryRulesetPut : BaseModel
{
    public string? name { get; set; }
    public string? target { get; set; }
    public string? enforcement { get; set; }
    public Conditions? conditions { get; set; }
    public Rule[]? rules { get; set; }
}