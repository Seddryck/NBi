param(
    [parameter(Mandatory=$true)]
    [string[]]$versions
)

$root = (split-path -parent $MyInvocation.MyCommand.Definition)
if (!(Test-Path $root\NBi.Core.SqlServer.csproj))
{
    Write-Error "No project file found. Exit from multiple build"
    exit
}


$dotNetVersion = "4.0"
$regKey = "HKLM:\software\Microsoft\MSBuild\ToolsVersions\$dotNetVersion"
$regProperty = "MSBuildToolsPath"

$msbuildExe = join-path -path (Get-ItemProperty $regKey).$regProperty -childpath "msbuild.exe"

foreach ($version in $versions)
{
    Write-Verbose "Building NBi.Core.SqlServer for version $version ..."
    &$msbuildExe $root\NBi.Core.SqlServer.csproj /p:Version=SqlServer$version
    if ($LastExitCode -eq 0)
    {
        Write-Verbose "Succesful build NBi.Core.SqlServer for version $version"
        Write-Verbose "Copying NBi.Core.SqlServer for version $version to NBi.NUnit.Runtime ..."
        Copy-Item $root\Bin\Debug\SqlServer$version\NBi.Core.SqlServer$version.* $root\..\NBi.NUnit.Runtime\Bin\Debug\
        Write-Verbose "Files for version $version copied to NBi.NUnit.Runtime ..."
    }
    else
    {
        Write-Warning "Impossible to build NBi.Core.SqlServer for version $version. Skipping this version."
    }
}