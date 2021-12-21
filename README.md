# RepoAutomationDotNet
.NET code to create and configure a repo with C#.  
- Enables repo "auto-merge" open
- Enables repo "Delete head branch on merge" option 
- Sets up branch policies on default branch (require a pull request and successful build action)

## Usage

```
RepoAutomation [-r|--repo <repository Name>] [-v|--visibility <repo visibility>] [-l|--license <repo license>] [-p|--patToken <GitHub Pat Token>]  [-bp|--branchpolicy <default branch policy>]
```

