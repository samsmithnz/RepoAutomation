# Repo Automation

[![CI/ CD](https://github.com/samsmithnz/RepoAutomation/actions/workflows/dotnet.yml/badge.svg)](https://github.com/samsmithnz/RepoAutomation/actions/workflows/dotnet.yml)
[![Coverage Status](https://coveralls.io/repos/github/samsmithnz/RepoAutomation/badge.svg?branch=main)](https://coveralls.io/github/samsmithnz/RepoAutomation?branch=main)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=samsmithnz_RepoAutomation&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=samsmithnz_RepoAutomation)
[![Latest NuGet package](https://img.shields.io/nuget/v/RepoAutomation.Core)](https://www.nuget.org/packages/RepoAutomation.Core/)
[![Current Release](https://img.shields.io/github/release/samsmithnz/RepoAutomation/all.svg)](https://github.com/samsmithnz/RepoAutomation/releases)


**.NET code to create and configure a repo with C#.** I'm in GitHub everyday. There are certain activities I found myself manually doing on a regular basis. Not super long activities, but boring, repetitive activities - the types of activities that are perfect for automation. Hence this project:
- Enables repo "auto-merge" open
- Enables repo "Delete head branch on merge" option 
- Enables repo visibility to be set
- Creates the initial project.
    - Clone the project to a folder (`Target folder`) https://docs.github.com/en/repositories/creating-and-managing-repositories/cloning-a-repository
    - Create src folder
    - Creates .NET test project (`test project name` and `test project type`) https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-new
    - Creates .NET console\library\webapi\web app project (`project name` and `project type`) https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-new
    - Creates .NET solution (`solution name`) https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-sln
- Creates the GitHub action to build/test/publish the .NET projects added above
    - Adds an action status badge to the readme file
- Creates the dependabot configuration
- Push all of the changes back to the repository/GitHub
- Sets up branch policies on default branch (e.g. require a pull request and successful build action)

## Usage

```Shell
RepoAutomation [-o|--owner <GITHUB-OWNER/ACCOUNT>] 
               [-r|--repo <REPOSITORY-NAME>] 
               [-d|--directory <WORKING-DIRECTORY>] 
               [-v|--visibility <REPO-VISIBILITY>]
               [-p|--projectTypes <.NET-COMMA-DELIMITED-PROJECTS]
```
<!-- TODO: RepoAutomation [-l|--license <repo license>] [-p|--patToken <GitHub Pat Token>]  [-bp|--branchpolicy <default branch policy>] -->

Requires a [Finely-Grained PAT token](https://github.com/settings/tokens) to be setup, with: 
- Actions: Read-only (can view workflows and actions, but cannot modify or trigger them)
- Administration: Read and write (can view and perform admin tasks on repositories)
- Code scanning alerts: Read-only (can view code scanning alerts, but cannot resolve or dismiss them)
- Contents: Read (can view repository contents, but cannot modify them)
- Dependabot alerts: Read (can view Dependabot alerts)
- Deployments: Read (can view deployment statuses)
- Metadata: Read (can view repository metadata)
- Pull Requests: Read and write (can view, create, update, and merge pull requests)
- Repository security advisories: Read (can view security advisories for repos)
- Secret Scanning alerts: Read (can view secret scanning alerts)

## Example

Using default settings to create a new repository "RepoAutomationTest", with a .NET 6 class library and unit tests project:
```
RepoAutomation --owner samsmithnz --repo RepoAutomationTest --directory c:\users\sam\source\repos --visibility public --projectTypes classlib,mstest
```

![image](https://user-images.githubusercontent.com/8389039/147719122-13fad701-8305-4a85-bb93-de07f90e8c1c.png)

Here is the result:
![image](https://user-images.githubusercontent.com/8389039/147702917-076d9502-4979-40f3-9b90-44664e495afe.png)


## Website

For [governance](https://github.com/samsmithnz/repogovernance), we are building a website to monitor repos and take corrective action:

![image](https://user-images.githubusercontent.com/8389039/148411631-46d2e485-022e-4815-b798-d88f344fc157.png)
