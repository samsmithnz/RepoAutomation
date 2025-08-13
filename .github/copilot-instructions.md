# RepoAutomation - .NET Repository Automation CLI Tool

Always reference these instructions first and fallback to search or bash commands only when you encounter unexpected information that does not match the info here.

## Overview

RepoAutomation is a .NET 8 CLI application that automates the creation and configuration of .NET repositories on GitHub. It handles project setup, GitHub Actions workflows, Dependabot configuration, and repository settings through GitHub's API.

## Working Effectively

### Prerequisites & Setup
- .NET 8.0+ SDK required (currently using .NET 8.0.118)
- GitHub Personal Access Token (PAT) required for full functionality
- Git must be available in PATH

### Bootstrap & Build Process
- `cd src` - Navigate to source directory
- `dotnet restore` - Restore NuGet packages (~16 seconds for first run, ~2 seconds subsequent)
- `dotnet build` - Build solution (~10 seconds Debug, ~2 seconds Release incremental)
  - **NEVER CANCEL**: Build takes up to 15 seconds from clean. ALWAYS set timeout to 60+ seconds
- `dotnet build -c Release` - Release build (~2 seconds incremental)
- `dotnet clean` - Clean all build artifacts (~1 second)

### Testing
- `dotnet test --filter "TestCategory!=IntegrationTests"` - Unit tests only (~2 seconds, 5 tests pass)
  - **NEVER CANCEL**: Unit tests complete in under 5 seconds. Set timeout to 30+ seconds minimum
- `dotnet test` - All tests including integration tests (~18 seconds, requires GitHub credentials)
  - **NEVER CANCEL**: Integration tests take 15-20 seconds. ALWAYS set timeout to 60+ seconds
  - **CREDENTIAL REQUIREMENT**: Integration tests REQUIRE GitHub API credentials in appsettings.json or user secrets

### Code Quality & Formatting
- `dotnet format --verify-no-changes` - Check code formatting (fails if formatting issues exist)
- `dotnet format` - Auto-fix code formatting issues
- **ALWAYS run `dotnet format` before committing** - CI will fail if code is not properly formatted

### Running the Application
- `dotnet run --help` - Show CLI help and available options
- **REQUIRES GITHUB CREDENTIALS**: Application cannot run without valid GitHub PAT token
- Configure credentials in `src/RepoAutomation/appsettings.json` or user secrets:
  ```json
  {
    "AppSettings": {
      "GitHubClientId": "your-github-client-id",
      "GitHubClientSecret": "your-github-pat-token"
    }
  }
  ```

### CLI Usage Example
```bash
# Example command (requires GitHub credentials)
dotnet run -- --owner samsmithnz --repo TestRepo --directory /tmp/test --visibility public --projecttypes "classlib,mstest"
```

**Parameters:**
- `--owner` / `-o`: GitHub username or organization
- `--repo` / `-r`: Repository name to create
- `--directory` / `-d`: Working directory for local operations
- `--visibility` / `-v`: Repository visibility (public/private)
- `--projecttypes` / `-p`: Comma-separated .NET project types (classlib, mstest, webapi, etc.)

## Validation Scenarios

### After Making Changes - ALWAYS Test These Scenarios:
1. **Build Validation**: `dotnet clean && dotnet build` (should complete in ~15 seconds)
2. **Unit Test Validation**: `dotnet test --filter "TestCategory!=IntegrationTests"` (should pass 5 tests in ~2 seconds)
3. **Format Validation**: `dotnet format --verify-no-changes` (should pass or be fixed with `dotnet format`)
4. **CLI Help Test**: `dotnet run --help` (should show usage without errors)

### Manual Testing Guidelines (with GitHub credentials):
- **ALWAYS test the end-to-end workflow** after core changes
- Create a test repository with: `--owner testuser --repo testrepo-$(date +%s) --visibility public --projecttypes classlib,mstest`
- Verify all generated files: .github/workflows/, GitVersion.yml, Dependabot config
- Test both public and private repository creation

## Critical Build & Test Timing

| Command | Expected Time | NEVER CANCEL Timeout | Notes |
|---------|---------------|----------------------|--------|
| `dotnet restore` | 2-16 seconds | 60 seconds | Depends on cache state |
| `dotnet build` | 2-15 seconds | 60 seconds | Warnings are normal |
| `dotnet build -c Release` | 2 seconds | 30 seconds | Incremental builds are fast |
| `dotnet test` (unit only) | 1-2 seconds | 30 seconds | 5 tests should pass |
| `dotnet test` (all) | 15-20 seconds | 90 seconds | Requires GitHub credentials |
| `dotnet clean` | 1 second | 15 seconds | Always fast |
| `dotnet format` | 2-5 seconds | 30 seconds | May show errors before fixing |

**⚠️ CRITICAL**: NEVER CANCEL builds or tests. Build warnings about nullable types are expected and normal.

## Project Structure

### Solution Layout
```
src/
├── RepoAutomation/           # Console application (main entry point)
├── RepoAutomation.Core/      # Core business logic and GitHub API integration  
├── RepoAutomation.Tests/     # MSTest unit and integration tests
└── RepoAutomation.sln        # Visual Studio solution file
```

### Key Files & Directories
- `src/RepoAutomation/Program.cs` - Main CLI entry point with argument parsing
- `src/RepoAutomation.Core/Helpers/` - Core automation logic (DotNet, GitHubActions, etc.)
- `src/RepoAutomation.Core/APIAccess/` - GitHub API integration classes
- `src/RepoAutomation.Tests/` - Unit tests (5 non-integration) and integration tests (50+)
- `.github/workflows/dotnet.yml` - CI/CD pipeline (Windows & Ubuntu)
- `GitVersion.yml` - Semantic versioning configuration

### Important Helper Classes
- `DotNet.cs` - .NET project creation and solution management
- `GitHubActions.cs` - GitHub Actions workflow generation
- `GitHubDependabot.cs` - Dependabot configuration creation
- `GitHubApiAccess.cs` - GitHub REST API integration

## Development Workflow

### Making Changes
1. **Always start from `src` directory**: `cd src`
2. **Build and test before changes**: `dotnet build && dotnet test --filter "TestCategory!=IntegrationTests"`
3. **Make minimal focused changes**
4. **Validate incrementally**: Build and test after each logical change
5. **Format code**: `dotnet format` before committing
6. **Test end-to-end scenarios**: If changing core logic, test with real GitHub credentials

### CI/CD Pipeline
- **GitHub Actions** runs on push to main and PRs
- **Windows & Ubuntu** matrix builds
- **Requires GitHub secrets** for integration tests:
  - `GitHubClientId` and `GitHubClientSecret`
- **NuGet package** publishing to nuget.org on release
- **SonarCloud analysis** (currently disabled)

### VSCode Integration
- `.vscode/tasks.json` - Build, publish, and watch tasks
- `.vscode/launch.json` - Debug configurations (may need .NET version update)
- Primary development supported in VSCode with C# extension

## Common Issues & Solutions

### Build Warnings
- **Nullable reference type warnings** are expected and normal
- **7 warnings typically appear** - these do not prevent successful builds
- Warnings are in SecurityAlertModelTests.cs, RepoTests.cs, and Program.cs

### Test Failures
- **Integration tests fail without GitHub credentials** - this is expected
- **"Unexpected character encountered while parsing value: B"** error means missing/invalid GitHub credentials
- **Unit tests should always pass** - if they fail, there's a real issue

### Runtime Requirements
- **GitHub PAT token** required for any meaningful functionality
- **Git** must be in PATH for repository cloning operations
- **Internet connectivity** required for GitHub API calls and NuGet restore

## Supported .NET Project Types
The tool supports these `--projecttypes` values:
- `classlib` - Class library
- `console` - Console application  
- `mstest` - MSTest unit test project
- `webapi` - ASP.NET Core Web API
- `webapp` / `razor` - ASP.NET Core Razor Pages
- `mvc` - ASP.NET Core MVC
- `grpc` - gRPC service
- `blazor` / `blazorserver` - Blazor applications

Multiple types can be specified comma-separated: `classlib,mstest,webapi`