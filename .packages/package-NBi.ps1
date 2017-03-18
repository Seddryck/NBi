param(
    [parameter(Mandatory=$true)]
    [string]$version
)

$root = (split-path -parent $MyInvocation.MyCommand.Definition)

#For NBi.Framework (dll)
$lib = "$root\NBi.Framework\lib\45\"
If (Test-Path $lib)
{
	Remove-Item $lib -recurse
}
new-item -Path $lib -ItemType directory
new-item -Path $root\..\.nupkg -ItemType directory -force
Copy-Item $root\..\NBi.NUnit.Runtime\bin\Debug\*.dll $lib


Write-Host "Setting .nuspec version tag to $version"

$content = (Get-Content $root\NBi.Framework\NBi.Framework.nuspec -Encoding UTF8) 
$content = $content -replace '\$version\$',$version

$content | Out-File $root\NBi.Framework\NBi.Framework.compiled.nuspec -Encoding UTF8

& $root\..\.nuget\NuGet.exe pack $root\..\.packages\NBi.Framework\NBi.Framework.compiled.nuspec -Version $version -OutputDirectory $root\..\.nupkg

#For NBi.Framework.Tools
$lib = "$root\NBi.Framework.Tools\tools\"
If (Test-Path $lib)
{
	Remove-Item $lib -recurse
}
new-item -Path $lib -ItemType directory
new-item -Path $root\..\.nupkg -ItemType directory -force
Copy-Item $root\..\NBi.NUnit.Runtime\bin\Debug\*.dll $lib

Write-Host "Setting .nuspec version tag to $version"

$content = (Get-Content $root\NBi.Framework.Tools\NBi.Framework.Tools.nuspec -Encoding UTF8) 
$content = $content -replace '\$version\$',$version

$content | Out-File $root\NBi.Framework.Tools\NBi.Framework.Tools.compiled.nuspec -Encoding UTF8

& $root\..\.nuget\NuGet.exe pack $root\..\.packages\NBi.Framework.Tools\NBi.Framework.Tools.compiled.nuspec -Version $version -OutputDirectory $root\..\.nupkg