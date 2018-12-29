#addin nuget:?package=Cake.Coverlet&version=2.1.2

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("clean")
    .Does(() =>
{
    DotNetCoreClean("./");
});

Task("restore")
    .IsDependentOn("clean")
    .Does(() =>
{
    DotNetCoreRestore();
});

Task("versioning")
    .IsDependentOn("restore")
    .Does(() =>
{
    Information("versioning");
});

Task("build")
    .IsDependentOn("versioning")
    .Does(() =>
{
    DotNetCoreBuild("./", new DotNetCoreBuildSettings
    {
        Configuration = configuration
    });
});

Task("test")
    .IsDependentOn("build")
    .Does(() =>
{
    var testSettings = new DotNetCoreTestSettings();
    var coverletSettings = new CoverletSettings
    {
        CollectCoverage = true,
        CoverletOutputFormat = CoverletOutputFormat.opencover,
        CoverletOutputDirectory = Directory(@"./DotGGPK/DotGGPK.Tests/bin/Debug/"),
        CoverletOutputName = $"coverage"
    };

    DotNetCoreTest("./DotGGPK/DotGGPK.Tests/DotGGPK.Tests.csproj", testSettings, coverletSettings);
});

Task("pack")
    .IsDependentOn("test")
    .WithCriteria(configuration == "Release")
    .Does(() =>
{
    Information("pack");
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