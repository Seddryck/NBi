#Define the constants
$downloadUrl = "https://github.com/roapi/roapi/releases/latest/download"
$archiveName = "roapi-windows.tar.gz"
$localFolder = "c:\projects\roapi"
$exeName = "roapi.exe"
$configFile = "default.yml"
$port = 8084


#Add PATH environment variable for 7-Zip and Curl
foreach ($util in @("7-Zip", "curl")) {
    if (!($Env:Path -like "*$util*")) {
        Write-Host "$util not registered in the PATH environment variable. Trying to find a path where it's installed."
        $candidates = Get-PSDrive -PSProvider FileSystem | ForEach-Object {ForEach ($folder in @("Program Files", "Program Files (x86)")) {"$($_.Root)$folder\$util\"}}
        $found = $false
        foreach ($candidate in $candidates) {
            if ((Test-Path -Path $candidate)){
                $found = $true
                Write-Host "$util added to PATH environment variable from installation path $candidate."
                $Env:Path+= "$candidate;"
            }
        }
        if (!($found)) {
            Write-Host "No suitable path found for $util."
        }
    } else {
        Write-Host "$util already registered in the PATH environment variable"
    }
}

#Check if local folder exists and if not create it
if (!(Test-Path -Path "$localFolder")){
    New-Item "$localFolder" -ItemType "directory"
}

#Check if we need to download a new version of Roapi
if ($env:AppVeyor -eq $null) {$appVeyor="false"} else {$appVeyor=$env:AppVeyor}
if (Test-Path "$localFolder\$archiveName" -NewerThan (Get-Date).AddHours(-10)) {
    Write-Host "Local copy of archive already existing (less than 10 hours old)"
} else {
    Write-Host "Downloading roapi package ..."
    Write-Host "$downloadurl/$archiveName"
    if ($appVeyor.ToLower() -eq "true") {
        Write-Host "$downloadurl/$archiveName"
        Start-FileDownload "$downloadurl/$archiveName" -FileName "$localFolder\$archiveName"
    } else {
        Invoke-WebRequest -Uri "$downloadurl/$archiveName" -OutFile "$localFolder\$archiveName"
    }
    Write-Host "Roapi package downloaded"
}


#Check if we don't need to kill some roapi before unzipping (to be sure that we can delete the)
Get-Process roapi* `
    | Where-Object {$_.Path -eq "$localFolder\$exeName"} `
    | ForEach-Object -Process {Write-Host "Killing the running $($_.Name) process $($_.Id)"; Stop-Process -Id $_.Id;}


#Extracting Roapi EXE from the ZIP file
Write-Host "Extracting roapi from package ..."
$defaultLocation = Get-Location
Set-Location -Path "$localFolder\"
&7z x -aoa "$localFolder\$archiveName"
if (!(Test-Path "$localFolder\$($archiveName.Substring(0, $archiveName.LastIndexOf('.')))")){
   Write-Host "Something went wrong during extraction or package structure not compatible with this script (1st level of extraction)"
   exit
} else {
    &7z x -aoa "$localFolder\$($archiveName.Substring(0, $archiveName.LastIndexOf('.')))"
    if (!(Test-Path "$localFolder\$exeName")){
       Write-Host "Something went wrong during extraction or package structure not cmpatible with this script (2nd level of extraction)"
       exit
    } else {
       Get-ChildItem -Path "$localFolder\"
       Write-Host "Package extraction successful!"
    }
}

#Checking that roapi is effectively unzipped and available
$version = &("$localFolder\$exeName") -V
Write-Host "$version installed in $localFolder"

#Copy data folder 
if ($appVeyor.ToLower() -eq "true") {   
    if (!(Test-Path -Path "$localFolder\Data")){
        New-Item -Path "$localFolder" -Name "Data" -ItemType "directory"
    }
    Copy-Item -Path "$env:APPVEYOR_BUILD_FOLDER\NBi.Testing\Acceptance\Resources\Roapi\*" -Destination "$localFolder\Data" -Recurse
    Write-Host "Files in the directory containing data:"
    Get-ChildItem -Path "$localFolder\Data"
}

#Start roapi in an external process
Write-Host "Starting roapi with config file $configFile"
$roapi = Start-Process -FilePath ("$localFolder\$exeName") -ArgumentList @("-c", ".\Data\$configFile") -WorkingDirectory $localFolder -PassThru
Write-Host "Process $($roapi.Name) with id $($roapi.Id) is responding"

#Check if roapi is effectively started or not
$attempt = 0; $max = 3
$client = New-Object System.Net.Sockets.TcpClient([System.Net.Sockets.AddressFamily]::InterNetwork)
do {
	try {    
		$client.Connect("127.0.0.1", $port)
		write-host "Roapi listening on port $port"
	}
	
	catch {
		$client.Close()
		if($attempt -eq $max) {
			write-host "Roapi was not started"
		} else {
			[int]$sleepTime = 2 * (++$attempt)
			write-host "Roapi is not listening on port $port. Retry after $sleepTime seconds..."
			sleep $sleepTime;
		}
	}
}while(!$client.Connected -and $attempt -lt $max)


if ($client.Connected) {
    Write-Host "Retrieving schemas ..."
    $schema = &curl "127.0.0.1:$port/api/schema"
    Write-Host "List of loaded tables:"
    (ConvertFrom-Json $schema).psobject.properties.name
} else {
    Write-Host "Roapi is not correctly started or has stopped."
}

Set-Location -Path "$defaultLocation"
