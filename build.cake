#addin nuget:?package=Cake.Coverlet&version=2.3.4
#addin nuget:?package=Cake.DocFx&version=0.13.1

#tool nuget:?package=ReportGenerator&version=4.4.0

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
    .WithCriteria(DirectoryExists(".git"))
    .Does((context) =>
{
    (string version, string versionShort, string versionSematic) versions;

    try
    {
        versions = GetGitTagVersion(context, buildnumber);
    }
    catch (Exception ex)
    {
        Exception currentException = ex;

        while (currentException != null)
        {
            Warning(currentException.Message);
            Warning(currentException.StackTrace);
            Warning("---");

            currentException = currentException.InnerException;
        }

        throw;
    }

    Information("Version: " + versions.version);
    Information("Version (sematic): " + versions.versionSematic);
    Information("Version (short): " + versions.versionShort);

    if (configuration == "Release")
    {
        Information("Release build - writing version output");
        CreateVersionProps("./src/DotGGPK/Version.props", versions.version, versions.version, versions.versionSematic);
    }
    else
    {
        Information("Debug build - skipping version output");
    }
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