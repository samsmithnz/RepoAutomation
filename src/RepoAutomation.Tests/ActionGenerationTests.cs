using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoAutomation.Helpers;
using RepoAutomation.Tests.Helpers;

namespace RepoAutomation.Tests;

[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
[TestClass]
[TestCategory("IntegrationTests")]
public class ActionGenerationTests
{

    [TestMethod]
    public void CreateEmptyAction()
    {
        //Arrange
        string projectName = "TestProject";
        bool includeTestProject = false;
        bool includeClassLibraryProject = false;
        bool includeWebProject = false;

        //Act
        string yaml = GitHubActionsAutomation.CreateActionYaml(projectName,
            includeTestProject,
            includeClassLibraryProject,
            includeWebProject);

        //Assert
        string expected = @"name: CI/CD
on:
  push:
    branches:
    - main
  pull_request:
    branches:
    - main
jobs:
  build:
    name: Build job
    runs-on: windows-latest
    outputs:
      Version: ${{ steps.gitversion.outputs.SemVer }}
      CommitsSinceVersionSource: ${{ steps.gitversion.outputs.CommitsSinceVersionSource }}
    steps:
    - uses: actions/checkout@v2
      with:
        fetch-depth: 0
    - name: Setup GitVersion
      uses: gittools/actions/gitversion/setup@v0.9.11
      with:
        versionSpec: 5.x
    - name: Determine Version
      id: gitversion
      uses: gittools/actions/gitversion/execute@v0.9.11
    - name: Display GitVersion outputs
      run: |
        echo ""Version: ${{ steps.gitversion.outputs.SemVer }}""
        echo ""CommitsSinceVersionSource: ${{ steps.gitversion.outputs.CommitsSinceVersionSource }}""
    - name: Setup .NET
      uses: actions/setup-dotnet@v1
      with:
        dotnet-version: 6.0.x";
        Assert.AreEqual(expected, Utility.TrimNewLines(yaml));
    }

    [TestMethod]
    public void CreateFullAction()
    {
        //Arrange
        string projectName = "TestProject";
        bool includeTestProject = true;
        bool includeClassLibraryProject = true;
        bool includeWebProject = true;

        //Act
        string yaml = GitHubActionsAutomation.CreateActionYaml(projectName,
            includeTestProject,
            includeClassLibraryProject,
            includeWebProject);

        //Assert
        string expected = @"name: CI/CD
on:
  push:
    branches:
    - main
  pull_request:
    branches:
    - main
jobs:
  build:
    name: Build job
    runs-on: windows-latest
    outputs:
      Version: ${{ steps.gitversion.outputs.SemVer }}
      CommitsSinceVersionSource: ${{ steps.gitversion.outputs.CommitsSinceVersionSource }}
    steps:
    - uses: actions/checkout@v2
      with:
        fetch-depth: 0
    - name: Setup GitVersion
      uses: gittools/actions/gitversion/setup@v0.9.11
      with:
        versionSpec: 5.x
    - name: Determine Version
      id: gitversion
      uses: gittools/actions/gitversion/execute@v0.9.11
    - name: Display GitVersion outputs
      run: |
        echo ""Version: ${{ steps.gitversion.outputs.SemVer }}""
        echo ""CommitsSinceVersionSource: ${{ steps.gitversion.outputs.CommitsSinceVersionSource }}""
    - name: Setup .NET
      uses: actions/setup-dotnet@v1
      with:
        dotnet-version: 6.0.x
    - name: .NET test
      run: dotnet test src/TestProject.Tests/TestProject.Tests.csproj -c Release
    - name: .NET publish
      run: dotnet publish src/TestProject/TestProject.csproj -c Release -p:Version='${{ steps.gitversion.outputs.SemVer }}'
    - name: Upload package back to GitHub
      uses: actions/upload-artifact@v2
      with:
        name: drop
        path: src/TestProject/bin/Release
    - name: .NET publish
      run: dotnet publish src/TestProject.Web/TestProject.Web.csproj -c Release -p:Version='${{ steps.gitversion.outputs.SemVer }}'
    - name: Upload package back to GitHub
      uses: actions/upload-artifact@v2
      with:
        name: web
        path: src/TestProject.Web/bin/Release";
        Assert.AreEqual(expected, Utility.TrimNewLines(yaml));
    }

}
