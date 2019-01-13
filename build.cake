#addin nuget:?package=Cake.Coverlet&version=2.1.2
#addin nuget:?package=Cake.Git&version=0.19.0
#addin nuget:?package=Cake.DocFx&version=0.11.0

#tool nuget:?package=docfx.console&version=2.40.7

using System.IO;
using System.Text;
using System.Text.RegularExpressions;

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
    .WithCriteria(configuration == "Release")
    .WithCriteria(DirectoryExists(".git"))
    .Does(() =>
{
    string major = "0";
    string minor = "0";
    string buildNumber = "0";
    string revision = "0";
    string shasum = "X";

    var gitDescription = GitDescribe("./", true, GitDescribeStrategy.Default);
    Information("Repository description: " + gitDescription);

    Regex query = new Regex(@"v(?<major>\d+).(?<minor>\d+).(?<revision>\d+)-(?<commits>\d+)-(?<shasum>.*)");
    MatchCollection matches = query.Matches(gitDescription);

    foreach (Match match in matches)
    {
        major = match.Groups["major"].Value;
        minor = match.Groups["minor"].Value;
        revision = match.Groups["revision"].Value;
        shasum = match.Groups["shasum"].Value;
    }

    buildNumber = GetPersistentBuildNumber(MakeAbsolute(new DirectoryPath("./")).FullPath).ToString();

    string versionString = string.Format("{0}.{1}.{2}.{3}", major, minor, buildNumber, revision);
    string shortVersionString = string.Format("{0}.{1}.{2}", major, minor, revision);
    string longVersionString = string.Format("{0}.{1}.{2}.{3}-{4}", major, minor, buildNumber, revision, shasum);

    Information("Version: " + versionString);
    Information("Version (long): " + longVersionString);
    Information("Version (short): " + shortVersionString);

    WriteVersion("./src/DotGGPK/Version.props", shortVersionString, versionString);
});

Task("build")
    .IsDependentOn("versioning")
    .Does(() =>
{
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
    var testSettings = new DotNetCoreTestSettings();
    var coverletSettings = new CoverletSettings
    {
        CollectCoverage = true,
        CoverletOutputFormat = CoverletOutputFormat.opencover,
        CoverletOutputDirectory = Directory(@"./src/DotGGPK.Tests/bin/Debug/"),
        CoverletOutputName = $"coverage"
    };

    DotNetCoreTest("./src/DotGGPK.Tests/DotGGPK.Tests.csproj", testSettings, coverletSettings);

    // Since CAKE report generator addin does not support .NET Core yet we'll call report generator global tool if available
    try
    {
        StartProcess("reportgenerator", "-reports:./src/DotGGPK.Tests/bin/Debug/coverage*.* -targetdir:./src/DotGGPK.Tests/bin/Debug/coverage/");
    }
    catch
    {
        Information("Unable to execute Report Generator (Global Tool)");
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
        OutputDirectory = "./"
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

//////////////////////////////////////////////////////////////////////
// FUNCTIONS
//////////////////////////////////////////////////////////////////////

public static int GetPersistentBuildNumber(string baseDirectory)
{
    int buildNumber;
    string persistentPathName = System.IO.Path.Combine(baseDirectory, ".cache");
    string persistentFileName = System.IO.Path.Combine(persistentPathName, "build-number");

    try
    {
        if (!System.IO.Directory.Exists(persistentPathName))
        {
            System.IO.Directory.CreateDirectory(persistentPathName);
        }

        buildNumber = int.Parse(System.IO.File.ReadAllText(persistentFileName).Trim());
        buildNumber++;
    }
    catch
    {
        buildNumber = 1;
    }

    System.IO.File.WriteAllText(persistentFileName, buildNumber.ToString());

    return buildNumber;
}

public static void WriteVersion(string fileName, string shortVersion, string version)
{
    StringBuilder builder = new StringBuilder();
    builder.AppendLine("<!-->");
    builder.AppendLine(" <auto-generated>");
    builder.AppendLine("     This file was generated by Cake.");
    builder.AppendLine(" </auto-generated>");
    builder.AppendLine("-->");
    builder.AppendLine("<Project>");
    builder.AppendLine("  <PropertyGroup>");
    builder.AppendLine("    <AssemblyVersion>" + version + "</AssemblyVersion>");
    builder.AppendLine("    <FileVersion>" + version + "</FileVersion>");
    builder.AppendLine("    <Version>" + shortVersion + "</Version>");
    builder.AppendLine("  </PropertyGroup>");
    builder.AppendLine("</Project>");

    System.IO.File.WriteAllText(fileName, builder.ToString());
}