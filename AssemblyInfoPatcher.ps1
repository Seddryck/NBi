param(
    [parameter(Mandatory=$true)]
    [string]$version
)

Write-Host "Looking to setup the version $version"

$parts = $version -split '-'
$arr = $parts[0] -split '\.'
if ($arr.Count -eq 2) {$arr+=0}

if ($parts.Count -gt 1) {
    $arr += $parts[1..($parts.Length-1)]
}
if ($arr.Count -eq 2) {$arr+=0}
if ($arr.Count -eq 3) {$arr+=0}

Write-Host "Major: $($arr[0])"
Write-Host "Minor: $($arr[1])"
Write-Host "Patch: $($arr[2])"
Write-Host "Build number: $($arr[3])"


(Get-childitem -include AssemblyInfo.cs -recurse) | Foreach-Object { 
    $path = $_
	(Get-Content $_) | ForEach-Object{
		if($_ -match '\[assembly: AssemblyVersion\("(.*)"\)\]'){
			$newVersion = "{0}.{1}.{2}" -f $arr[0], $arr[1], $arr[2]
			'[assembly: AssemblyVersion("{0}")]' -f $newVersion
            Write-Host("Setting version to $newVersion in $path")
		} elseif($_ -match '\[assembly: AssemblyFileVersion\("(.*)"\)\]'){
			$newVersion = "{0}.{1}.{2}" -f $arr[0], $arr[1], $arr[2]
			'[assembly: AssemblyFileVersion("{0}")]' -f $newVersion
            Write-Host("Setting file version to $newVersion in $path")
		} elseif($_ -match '\[assembly: AssemblyInformationalVersion\("(.*)"\)\]'){
			$newVersion = "{0}.{1}.{2}" -f $arr[0], $arr[1], $arr[2]
			'[assembly: AssemblyInformationalVersion("{0}")]' -f $version
            Write-Host("Setting informational version to $version in $path")
        } else {
            $_
        }

    } | Set-Content $_
}