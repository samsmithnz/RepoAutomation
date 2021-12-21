# RepoAutomationDotNet
.NET code to create and configure a repo with C#. (`Repo name` is a parameter)
- Enables repo "auto-merge" open
- Enables repo "Delete head branch on merge" option 
- Sets up branch policies on default branch (require a pull request and successful build action)
- Creates the initial project.
    - Clone the project to a folder (`Target folder`)
    - Create src folder
    - Creates .NET test project (`test project name` and `test project type`)
    - Creates .NET console\library\webapi/web app project (`project name` and `project type`)
    - Creates .NET solution (`solution name`)

## Usage

```
RepoAutomation [-r|--repo <repository Name>] [-v|--visibility <repo visibility>] [-l|--license <repo license>] [-p|--patToken <GitHub Pat Token>]  [-bp|--branchpolicy <default branch policy>]
```

