$parts = '%build.number%' -split '-'
$arr = $parts[0] -split '\.'
$arr += $parts[1..($parts.Length-1)]

Write-Host $arr


(Get-childitem -include AssemblyInfo.cs -recurse) | Foreach-Object { 
	$pattern = 
	(Get-Content $_) | ForEach-Object{
		if($_ -match '\[assembly: AssemblyVersion\("(.*)"\)\]'){
			$newVersion = "{0}.{1}.{2}" -f $arr[0], $arr[1], $arr[2]
			'[assembly: AssemblyVersion("{0}")]' -f $newVersion
		} elseif($_ -match '\[assembly: AssemblyFileVersion\("(.*)"\)\]'){
			$newVersion = "{0}.{1}.{2}" -f $arr[0], $arr[1], $arr[2]
			'[assembly: AssemblyFileVersion("{0}")]' -f $newVersion
		} elseif($_ -match '\[assembly: AssemblyInformationalVersion\("(.*)"\)\]'){
			$newVersion = "{0}.{1}.{2}" -f $arr[0], $arr[1], $arr[2]
			'[assembly: AssemblyInformationalVersion("{0}")]' -f '%build.number%'
        } else {
            $_
        }

    } | Set-Content $_
}