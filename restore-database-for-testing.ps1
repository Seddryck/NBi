$mssqlPrefix = "mssql`$sql"
$mssqlName = (Get-Service $mssqlPrefix* | Sort-Object Name -Descending | Select-Object -first 1).Name
$mssqlVersion = $mssqlName.Substring($mssqlPrefix.Length)
Write-Host "Starting service '$mssqlName' (Version:$mssqlVersion)"

Start-Service $mssqlName
Write-Host "Service '$mssqlName' is $((Get-Service $mssqlName).Status)"
Start-FileDownload "https://github.com/Microsoft/sql-server-samples/releases/download/adventureworks/AdventureWorksDW$mssqlVersion.bak" -FileName "c:\projects\AdventureWorksDW$mssqlVersion.bak"

if (Get-Module -ListAvailable -Name dbatools) 
    { Write-Host "Module dbatools already installed" } 
else 
    { Install-Module dbatools -Scope CurrentUser }

Restore-DbaDatabase -SqlInstance $env:computername\SQL$mssqlVersion -Path c:\temp\AdventureWorksDW$mssqlVersion.bak