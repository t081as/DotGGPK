// The MIT License (MIT)
//
// Copyright © 2017-2020 Tobias Koch
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the “Software”), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

using System.IO;
using System.Linq;
using static Mjolnir.Build.VCS.GitVersionTasks;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Execution;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using Nuke.Common.Tools.ReportGenerator;
using static Nuke.Common.ChangeLog.ChangelogTasks;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.ReportGenerator.ReportGeneratorTasks;
using System.Xml.Linq;
using System.Globalization;
using System;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Test);

    [Parameter]
    readonly Configuration Configuration = Configuration.Debug;

    [Parameter]
    readonly ulong Buildnumber = 0;

    [Parameter]
    readonly string Key = string.Empty;

    [Solution]
    readonly Solution Solution;

    readonly string coverageFiles = "**/TestResults/*/coverage.cobertura.xml";

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath TestsDirectory => RootDirectory / "tests";
    AbsolutePath OutputDirectory => RootDirectory / "output";

    string shortVersion = "0.0.0";
    string version = "0.0.0.0";
    string semanticVersion = "0.0.0+XXXXXXXX";

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(_ => _
                .SetProjectFile(Solution));
        });

    Target Clean => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            //// TestsDirectory.GlobFiles("**/TestResults/TestResults.xml").ForEach(DeleteFile);

            RootDirectory.GlobFiles("**/*.nupkg").ForEach(DeleteFile);
            RootDirectory.GlobFiles(coverageFiles).ForEach(DeleteFile);

            EnsureCleanDirectory(OutputDirectory);
        });

    Target Version => _ => _
        .Executes(() =>
        {

            (string shortVersion, string version, string semanticVersion) = GetGitTagVersion(RootDirectory, Buildnumber);

            Logger.Info($"Version: {version}");
            Logger.Info($"Short Version: {shortVersion}");
            Logger.Info($"Semantic Version: {semanticVersion}");
            Logger.Info($"Buildnumber: {Buildnumber}");

            if (Configuration == Configuration.Release)
            {
                this.shortVersion = shortVersion;
                this.version = version;
                this.semanticVersion = semanticVersion;
            }
            else
            {
                Logger.Info("Debug build - skipping version");
            }
        });

    Target Compile => _ => _
        .DependsOn(Clean)
        .DependsOn(Restore)
        .DependsOn(Version)
        .Executes(() =>
        {
            DotNetBuild(_ => _
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetVersion(semanticVersion)
                .SetAssemblyVersion(version)
                .SetFileVersion(version)
                .EnableNoRestore());
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            string loggerConfiguration = $"junit;LogFilePath={OutputDirectory / "TestResults" / "TestResults.xml"};MethodFormat=Class;FailureBodyFormat=Verbose";
            
            if (Configuration == Configuration.Release)
            {
                DotNetTest(_ => _
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .SetLogger(loggerConfiguration)
                .EnableNoBuild());
            }
            else
            {
                DotNetTest(_ => _
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .SetLogger(loggerConfiguration)
                .SetDataCollector("XPlat Code Coverage"));

                var reportFiles = RootDirectory / coverageFiles;

                if (EnvironmentInfo.IsWin)
                {
                    ReportGenerator(_ => _
                        .SetToolPath(ToolPathResolver.GetPackageExecutable("ReportGenerator", "ReportGenerator.exe", null, "netcoreapp3.0"))
                        .SetReports(reportFiles)
                        .SetTargetDirectory(OutputDirectory / "coverage")
                        .SetReportTypes(ReportTypes.TextSummary, ReportTypes.Html));

                    Logger.Info(File.ReadAllText(OutputDirectory / "coverage" / "Summary.txt"));
                }
                else
                {
                    var coverageFileNames = RootDirectory.GlobFiles(coverageFiles);

                    double overallLineCoverage = 0;

                    foreach (var coverageFileName in coverageFileNames)
                    {
                        XDocument xdoc = XDocument.Load(coverageFileName);
                        double lineCoverage = double.Parse(xdoc.Descendants("coverage").FirstOrDefault().Attribute("line-rate").Value, CultureInfo.GetCultureInfo("en-US"));

                        overallLineCoverage += lineCoverage;
                    }

                    Logger.Info("Summary");
                    Logger.Info($"  Line coverage: {Math.Round(overallLineCoverage * 100, 2).ToString(CultureInfo.GetCultureInfo("en-US"))}%");
                    Logger.Info("End Summary");
                }
            }
        });

    Target Pack => _ => _
        .DependsOn(Test)
        .Requires(() => Configuration == Configuration.Release)
        .Executes(() =>
        {
            var projects = new string[]
            {
                RootDirectory / "src" / "DotGGPK" / "DotGGPK.csproj",
            };

            var changeLog = GetNuGetReleaseNotes(RootDirectory / "CHANGELOG.md");

            foreach (var project in projects)
            {
                DotNetPack(_ => _
                    .SetProject(project)
                    .EnableNoRestore()
                    .SetVersion(semanticVersion)
                    .SetAssemblyVersion(version)
                    .SetFileVersion(version)
                    .SetIncludeSource(true)
                    .SetIncludeSymbols(true)
                    .SetSymbolPackageFormat(DotNetSymbolPackageFormat.snupkg)
                    .SetPackageReleaseNotes(changeLog)
                    .SetOutputDirectory(OutputDirectory));
            }
        });

    Target Push => _ => _
        .DependsOn(Pack)
        .Requires(() => Key)
        .Requires(() => Configuration == Configuration.Release)
        .Executes(() =>
        {
            var generatedPackages = OutputDirectory.GlobFiles("*.nupkg")
                .NotEmpty()
                .Where(p => !p.ToString().EndsWith(".symbols.nupkg"));

            foreach (var package in generatedPackages)
            {
                Logger.Info($"Pushing package {package}");

                DotNetNuGetPush(_ => _
                    .SetTargetPath(package)
                    .SetApiKey(Key)
                    .SetSource("https://api.nuget.org/v3/index.json"));
            }
        });
}