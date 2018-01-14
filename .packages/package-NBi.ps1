param(
    [parameter(Mandatory=$true)]
    [string]$version
)

$root = (split-path -parent $MyInvocation.MyCommand.Definition)

Write-Host "Calculating dependencies ..."

$dependencies = @{}
$solutionRoot = Join-Path ($root) ".."
$projects = Get-ChildItem $solutionRoot | ?{ $_.PSIsContainer -and $_.Name -like "NBi.*" -and $_.Name -notLike "*.UI*" -and $_.Name -notLike "*genbi*" -and $_.Name -notLike "*Testing*" -and $_.Name -notLike "*Service*"} | select Name, FullName
foreach($proj in $projects)
{
    $projName = $proj.name
    Write-Host "Looking for dependencies in project $projName ..."
    $path = Join-Path ($proj.FullName) "packages.config"
        
    if(Test-Path $path)
    {
        [xml]$packages = Get-Content $path
        foreach($package in $packages.FirstChild.NextSibling.ChildNodes)
        {
            if (!$dependencies.ContainsKey($package.id)) {$dependencies.add($package.id, "<dependency id=""$($package.id)"" version=""$(($package.allowedVersions, $package.version -ne $null)[0])"" />")}
        }
    }
    
}

Write-Host "Found $($dependencies.Count) dependencies ..."
$depList = $dependencies.Values -join [Environment]::NewLine + "`t`t"

#For NBi.Framework (dll)
$lib = "$root\NBi.Framework\lib\461\"
If (Test-Path $lib)
{
	Remove-Item $lib -recurse
}
new-item -Path $lib -ItemType directory
new-item -Path $root\..\.nupkg -ItemType directory -force
Copy-Item $root\..\NBi.NUnit.Runtime\bin\Debug\NBi.*.dll $lib

Write-Host "Setting .nuspec version tag to $version"

$content = (Get-Content $root\NBi.Framework\NBi.Framework.nuspec -Encoding UTF8) 
$content = $content -replace '\$version\$',$version
$content = $content -replace '\$depList\$',$depList

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
Copy-Item $root\..\NBi.NUnit.Runtime\bin\Debug\NBi.*.dll $lib

Write-Host "Setting .nuspec version tag to $version"

$content = (Get-Content $root\NBi.Framework.Tools\NBi.Framework.Tools.nuspec -Encoding UTF8) 
$content = $content -replace '\$version\$',$version
$content = $content -replace '\$depList\$',$depList

$content | Out-File $root\NBi.Framework.Tools\NBi.Framework.Tools.compiled.nuspec -Encoding UTF8

& $root\..\.nuget\NuGet.exe pack $root\..\.packages\NBi.Framework.Tools\NBi.Framework.Tools.compiled.nuspec -Version $version -OutputDirectory $root\..\.nupkg