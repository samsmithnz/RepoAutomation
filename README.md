# Repo Automation

[![CI/ CD](https://github.com/samsmithnz/RepoAutomation/actions/workflows/dotnet.yml/badge.svg)](https://github.com/samsmithnz/RepoAutomation/actions/workflows/dotnet.yml)

.NET code to create and configure a repo with C#. (`Repo name` is a parameter)
- Enables repo "auto-merge" open
- Enables repo "Delete head branch on merge" option 
- Sets up branch policies on default branch (require a pull request and successful build action)
- Creates the initial project.
    - Clone the project to a folder (`Target folder`) https://docs.github.com/en/repositories/creating-and-managing-repositories/cloning-a-repository
    - Create src folder
    - Creates .NET test project (`test project name` and `test project type`) https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-new
    - Creates .NET console\library\webapi/web app project (`project name` and `project type`) https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-new
    - Creates .NET solution (`solution name`) https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-sln

## Usage

```C#
RepoAutomation [-r|--repo <repository Name>] [-v|--visibility <repo visibility>] [-l|--license <repo license>] [-p|--patToken <GitHub Pat Token>]  [-bp|--branchpolicy <default branch policy>]
```

Requires a [PAT token](https://github.com/settings/tokens) to be setup, with `repo` and `workflow` permissions