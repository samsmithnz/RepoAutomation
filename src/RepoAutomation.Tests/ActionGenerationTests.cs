using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoAutomation.Core.Helpers;
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
        string projectTypes = "";

        //Act
        string yaml = GitHubActions.CreateActionYaml(projectName,
            projectTypes);

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
      Version: ${{ steps.gitversion.outputs.MajorMinorPatch }}
      CommitsSinceVersionSource: ${{ steps.gitversion.outputs.CommitsSinceVersionSource }}
    steps:
    - uses: actions/checkout@v4
      with:
        fetch-depth: 0
    - name: Setup GitVersion
      uses: gittools/actions/gitversion/setup@v4.01
      with:
        versionSpec: 6.x
    - name: Determine Version
      id: gitversion
      uses: gittools/actions/gitversion/execute@v4.01
    - name: Display GitVersion outputs
      run: |
        echo ""Version: ${{ steps.gitversion.outputs.MajorMinorPatch }}""
        echo ""CommitsSinceVersionSource: ${{ steps.gitversion.outputs.CommitsSinceVersionSource }}""
    - name: Setup .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: 8.0.x";
        Assert.AreEqual(expected, Utility.TrimNewLines(yaml));
    }

    [TestMethod]
    public void CreateFullAction()
    {
        //Arrange
        string projectName = "TestProject";
        string projectTypes = "mstest, classlib, mvc";

        //Act
        string yaml = GitHubActions.CreateActionYaml(projectName,
            projectTypes);

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
      Version: ${{ steps.gitversion.outputs.MajorMinorPatch }}
      CommitsSinceVersionSource: ${{ steps.gitversion.outputs.CommitsSinceVersionSource }}
    steps:
    - uses: actions/checkout@v4
      with:
        fetch-depth: 0
    - name: Setup GitVersion
      uses: gittools/actions/gitversion/setup@v4.0.1
      with:
        versionSpec: 6.x
    - name: Determine Version
      id: gitversion
      uses: gittools/actions/gitversion/execute@v4.0.1
    - name: Display GitVersion outputs
      run: |
        echo ""Version: ${{ steps.gitversion.outputs.MajorMinorPatch }}""
        echo ""CommitsSinceVersionSource: ${{ steps.gitversion.outputs.CommitsSinceVersionSource }}""
    - name: Setup .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: 8.0.x
    - name: .NET test
      run: dotnet test src/TestProject.Tests/TestProject.Tests.csproj -c Release
    - name: .NET publish
      run: dotnet publish src/TestProject/TestProject.csproj -c Release -p:Version='${{ steps.gitversion.outputs.MajorMinorPatch }}'
    - name: Upload package back to GitHub
      uses: actions/upload-artifact@v4
      with:
        name: drop
        path: src/TestProject/bin/Release
    - name: .NET publish
      run: dotnet publish src/TestProject.Web/TestProject.Web.csproj -c Release -p:Version='${{ steps.gitversion.outputs.MajorMinorPatch }}'
    - name: Upload package back to GitHub
      uses: actions/upload-artifact@v4
      with:
        name: web
        path: src/TestProject.Web/bin/Release";
        Assert.AreEqual(expected, Utility.TrimNewLines(yaml));
    }

}
