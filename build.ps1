##########################################################################
# This is the Cake bootstrapper script for PowerShell.
# This file was downloaded from https://github.com/cake-build/resources
# Feel free to change this file to fit your needs.
##########################################################################

<#

.SYNOPSIS
This is a Powershell script to bootstrap a Cake build.

.DESCRIPTION
This Powershell script will restore Cake and execute your Cake build script
with the parameters you provide.

.PARAMETER Script
The build script to execute.
.PARAMETER Target
The build script target to run.
.PARAMETER Configuration
The build configuration to use.
.PARAMETER Verbosity
Specifies the amount of information to be displayed.
.PARAMETER ShowDescription
Shows description about tasks.
.PARAMETER SkipToolPackageRestore
Skips restoring of packages.
.PARAMETER ScriptArgs
Remaining arguments are added here.

.LINK
https://cakebuild.net

#>

[CmdletBinding()]
Param(
    [string]$Script = "build.cake",
    [string]$Target,
    [string]$Configuration,
    [ValidateSet("Quiet", "Minimal", "Normal", "Verbose", "Diagnostic")]
    [string]$Verbosity,
    [switch]$ShowDescription,
    [Alias("WhatIf", "Noop")]
    [switch]$DryRun,
    [Parameter(Position=0,Mandatory=$false,ValueFromRemainingArguments=$true)]
    [string[]]$ScriptArgs
)

Write-Host "Preparing to run build script..."

if(!$PSScriptRoot){
    $PSScriptRoot = Split-Path $MyInvocation.MyCommand.Path -Parent
}

$TOOLS_DIR = Join-Path $PSScriptRoot "tools"
$TOOLS_PROJ = Join-Path $TOOLS_DIR "tools.csproj"
$CAKE_VERSION = "0.31.0"
$CAKE_DLL = Join-Path $TOOLS_DIR "Cake.CoreCLR.$CAKE_VERSION/cake.coreclr/$CAKE_VERSION/Cake.dll"

# Make sure tools folder exists
if ((Test-Path $PSScriptRoot) -and !(Test-Path $TOOLS_DIR)) {
    Write-Verbose -Message "Creating tools directory..."
    New-Item -Path $TOOLS_DIR -Type directory | out-null
}

# Make sure that cake exist.
if (!(Test-Path $CAKE_DLL)) {
    Write-Verbose -Message "Downloading cake..."
    $contents = '<Project Sdk="Microsoft.NET.Sdk"><PropertyGroup><TargetFramework>netstandard1.6</TargetFramework></PropertyGroup></Project>'
    Out-File -InputObject $contents -FilePath $TOOLS_PROJ
    Invoke-Expression "&dotnet add $TOOLS_PROJ package cake.coreclr -v `"$CAKE_VERSION`" --package-directory `"$TOOLS_DIR/Cake.CoreCLR.$CAKE_VERSION`""
}

# Make sure that Cake has been installed.
if (!(Test-Path $CAKE_DLL)) {
    Throw "Could not find Cake.dll at $CAKE_DLL"
}

# Build Cake arguments
$cakeArguments = @("$Script");
if ($Target) { $cakeArguments += "--target=$Target" }
if ($Configuration) { $cakeArguments += "--configuration=$Configuration" }
if ($Verbosity) { $cakeArguments += "--verbosity=$Verbosity" }
if ($ShowDescription) { $cakeArguments += "--showdescription" }
if ($DryRun) { $cakeArguments += "--dryrun" }
$cakeArguments += $ScriptArgs

# Start Cake
Write-Host "Running build script..."
&dotnet $CAKE_DLL $cakeArguments 
exit $LASTEXITCODE