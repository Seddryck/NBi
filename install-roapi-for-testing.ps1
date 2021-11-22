$archiveName = "roapi-http-windows.tar.gz"
$exeName = "roapi-http.exe"
$downloadUrl = "https://github.com/roapi/roapi/releases/latest/download"
$localFolder = "c:\projects"

if (!($Env:Path -like "*7-Zip*")) {
    $Env:Path+= "C:\Program Files\7-Zip\;"
    $Env:Path+= "E:\Program Files\7-Zip\;"
    refreshenv
}

if (Test-Path "$localFolder\$archiveName" -NewerThan (Get-Date).AddHours(-10)) {
    Write-Host "Local copy of archive already existing (less than 10 hours old)"
} else {
    Write-Host "Downloading roapi package ..."
    if ($env:AppVeyor -eq $null) {$appVeyor="false"} else {$appVeyor=$env:AppVeyor}
    if ($appVeyor.ToLower() -eq "true") {
        Start-FileDownload "$downloadurl/$archiveName" -FileName "$localFolder\$archiveName"
    } else {
        Invoke-WebRequest -Uri "$downloadurl/$archiveName" -OutFile "$localFolder\$archiveName"
    }
    Write-Host "Roapi package downloaded"
}

Write-Host "Extracting roapi from package ..."
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
       Write-Host "Package extraction successful!"
    }
}

$version = &("$localFolder\$exeName") -V
Write-Host "$version installed in $localFolder"

&("$localFolder\$exeName") -c "./hello.yml"
&curl -v "http://127.0.0.1:8084/api/tables/cities?columns=LatD"



