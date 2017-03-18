$root = (split-path -parent $MyInvocation.MyCommand.Definition)
$env:GitVersion_NuGetVersion = "1.15.0.0"

#For NBi.Framework (dll)
$lib = "$root\NBi.Framework\lib\45\"
If (Test-Path $lib)
{
	Remove-Item $lib -recurse
}
new-item -Path $lib -ItemType directory
new-item -Path $root\..\.nupkg -ItemType directory -force
Copy-Item $root\..\NBi.NUnit.Runtime\bin\Debug\*.dll $lib


Write-Host "Setting .nuspec version tag to $env:GitVersion_NuGetVersion"

$content = (Get-Content $root\NBi.Framework\NBi.Framework.nuspec -Encoding UTF8) 
$content = $content -replace '\$version\$',$env:GitVersion_NuGetVersion

$content | Out-File $root\NBi.Framework\NBi.Framework.compiled.nuspec -Encoding UTF8

& $root\..\.nuget\NuGet.exe pack $root\..\.packages\NBi.Framework\NBi.Framework.compiled.nuspec -Version $env:GitVersion_NuGetVersion -OutputDirectory $root\..\.nupkg

#For NBi.Framework.Tools
$lib = "$root\NBi.Framework.Tools\tools\"
If (Test-Path $lib)
{
	Remove-Item $lib -recurse
}
new-item -Path $lib -ItemType directory
new-item -Path $root\..\.nupkg -ItemType directory -force
Copy-Item $root\..\NBi.NUnit.Runtime\bin\Debug\*.dll $lib

Write-Host "Setting .nuspec version tag to $env:GitVersion_NuGetVersion"

$content = (Get-Content $root\NBi.Framework.Tools\NBi.Framework.Tools.nuspec -Encoding UTF8) 
$content = $content -replace '\$version\$',$env:GitVersion_NuGetVersion

$content | Out-File $root\NBi.Framework.Tools\NBi.Framework.Tools.compiled.nuspec -Encoding UTF8

& $root\..\.nuget\NuGet.exe pack $root\..\.packages\NBi.Framework.Tools\NBi.Framework.Tools.compiled.nuspec -Version $env:GitVersion_NuGetVersion -OutputDirectory $root\..\.nupkg