#addin nuget:?package=Cake.Coverlet&version=2.1.2
#addin nuget:?package=Cake.DocFx&version=0.11.0

#tool nuget:?package=docfx.console&version=2.40.7
#tool nuget:?package=ReportGenerator&version=4.0.4

#load nuget:?package=Mjolnir.Cake

using System.IO;
using System.Text;
using System.Text.RegularExpressions;

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");
var buildnumber = Argument("buildnumber", 0);

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("restore")
    .Does(() =>
{
    DotNetCoreRestore();
});

Task("versioning")
    .IsDependentOn("restore")
    .WithCriteria(configuration == "Release")
    .WithCriteria(DirectoryExists(".git"))
    .Does((context) =>
{
    (string version, string versionShort, string versionSematic) versions = GetGitTagVersion(context, buildnumber);

    Information("Version: " + versions.version);
    Information("Version (sematic): " + versions.versionSematic);
    Information("Version (short): " + versions.versionShort);

    CreateVersionProps("./src/DotGGPK/Version.props", versions.version, versions.version, versions.versionSematic);
});

Task("build")
    .IsDependentOn("versioning")
    .Does(() =>
{
    DotNetCoreClean("./");
    DotNetCoreBuild("./", new DotNetCoreBuildSettings
    {
        Configuration = configuration
    });

    if (configuration == "Release")
    {
        Information("Release - generating documentation");
        DocFxMetadata("./docs/docfx.json");
        DocFxBuild("./docs/docfx.json");
        CopyDirectory("docs/_site", "./public");
    }
});

Task("test")
    .IsDependentOn("build")
    .Does(() =>
{
    var testSettings = new DotNetCoreTestSettings
    {
        Logger = "junit"
    };

    var coverletSettings = new CoverletSettings
    {
        CollectCoverage = true,
        CoverletOutputFormat = CoverletOutputFormat.opencover,
        CoverletOutputDirectory = Directory(@"./src/DotGGPK.Tests/bin/Debug/"),
        CoverletOutputName = $"coverage"
    };

    DotNetCoreTest("./src/DotGGPK.Tests/DotGGPK.Tests.csproj", testSettings, coverletSettings);
    
    if (IsRunningOnWindows())
    {
        ReportGenerator("./src/DotGGPK.Tests/bin/Debug/coverage*.*", "./src/DotGGPK.Tests/bin/Debug/coverage/");
    }
});

Task("pack")
    .IsDependentOn("test")
    .WithCriteria(configuration == "Release")
    .Does(() =>
{
    var settings = new DotNetCorePackSettings
    {
        Configuration = "Release",
        OutputDirectory = "./",
        IncludeSource = true,
        IncludeSymbols = true,
        ArgumentCustomization = args => args.Append("-p:SymbolPackageFormat=snupkg")
    };

    DotNetCorePack("./", settings);
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("pack");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);