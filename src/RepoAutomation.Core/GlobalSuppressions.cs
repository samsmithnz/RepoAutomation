﻿// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "lower case is correct casing for GitHub Actions", Scope = "namespaceanddescendants", Target = "~N:RepoAutomation.Core.Models")]
[assembly: SuppressMessage("Style", "IDE0074:Use compound assignment", Justification = "<Pending>", Scope = "member", Target = "~M:RepoAutomation.Core.Helpers.CommandLine.RunCommand(System.String,System.String,System.String)~System.String")]
//[assembly: SuppressMessage("Style", "IDE0063:Use simple 'using' statement", Justification = "This rule ruins read-ability, in my opinion.", Scope = "member", Target = "~M:RepoAutomation.Core.APIAccess.BaseAPIAccess.GetGitHubMessage(System.String,System.String,System.String)~System.Threading.Tasks.Task{System.String}")]
//[assembly: SuppressMessage("Style", "IDE0063:Use simple 'using' statement", Justification = "This rule ruins read-ability, in my opinion.", Scope = "member", Target = "~M:RepoAutomation.Core.APIAccess.BaseAPIAccess.PostGitHubMessage(System.String,System.String,System.String,System.Net.Http.StringContent)~System.Threading.Tasks.Task{System.String}")]
