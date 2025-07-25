namespace RepoAutomation.Core.Models;

public class SecurityAlert : BaseModel
{
    public int? number { get; set; }
    public string? rule_id { get; set; }
    public string? rule_description { get; set; }
    public string? rule_severity { get; set; }
    public string? state { get; set; }
    public string? created_at { get; set; }
    public string? updated_at { get; set; }
    public string? dismissed_at { get; set; }
    public string? dismissed_by { get; set; }
    public string? dismissed_reason { get; set; }
    public string? html_url { get; set; }
    public SecurityTool? tool { get; set; }
    public SecurityMostRecentInstance? most_recent_instance { get; set; }
}

public class SecurityTool
{
    public string? name { get; set; }
    public string? version { get; set; }
}

public class SecurityMostRecentInstance
{
    public string? @ref { get; set; }
    public string? analysis_key { get; set; }
    public string? category { get; set; }
    public string? environment { get; set; }
    public SecurityLocation? location { get; set; }
}

public class SecurityLocation
{
    public string? path { get; set; }
    public int? start_line { get; set; }
    public int? end_line { get; set; }
    public int? start_column { get; set; }
    public int? end_column { get; set; }
}

// Secret scanning specific model
public class SecretScanningAlert : BaseModel
{
    public int? number { get; set; }
    public string? secret_type { get; set; }
    public string? secret_type_display_name { get; set; }
    public string? secret { get; set; }
    public string? state { get; set; }
    public string? created_at { get; set; }
    public string? updated_at { get; set; }
    public string? html_url { get; set; }
    public string? resolution { get; set; }
    public SecretLocation[]? locations { get; set; }
}

public class SecretLocation
{
    public string? type { get; set; }
    public SecretLocationDetails? details { get; set; }
}

public class SecretLocationDetails
{
    public string? path { get; set; }
    public int? start_line { get; set; }
    public int? end_line { get; set; }
    public int? start_column { get; set; }
    public int? end_column { get; set; }
    public string? blob_sha { get; set; }
    public string? blob_url { get; set; }
    public string? commit_sha { get; set; }
    public string? commit_url { get; set; }
}