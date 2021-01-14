$mssqlPrefix = "mssql`$sql"
$mssqlName = (Get-Service $mssqlPrefix* | Sort-Object Name -Descending | Select-Object -first 1).Name
$mssqlVersion = $mssqlName.Substring($mssqlPrefix.Length)
Write-Host "Selected service '$mssqlName' (Version:$mssqlVersion)"

if ((Get-Service $mssqlName).Status -eq "Running")
    { Write-Host "Service '$mssqlName' is already running" }
else
{
    Write-Host "Starting service '$mssqlName'"
    Start-Service $mssqlName
    Write-Host "Service '$mssqlName' is $((Get-Service $mssqlName).Status)"
}

Start-FileDownload "https://github.com/Microsoft/sql-server-samples/releases/download/adventureworks/AdventureWorks$mssqlVersion.bak" -FileName "c:\projects\AdventureWorks$mssqlVersion.bak"

if (Get-Module -ListAvailable -Name dbatools) 
    { Write-Host "Module dbatools already installed" } 
else 
{ 
    Write-Host "Installing module dbatools ..."
    Install-Module dbatools -Scope CurrentUser 
    Write-Host "Module dbatools installed"
}

Write-Host "Restoring AdventureWorks$mssqlVersion on $env:computername\SQL$mssqlVersion ..."
Restore-DbaDatabase -SqlInstance $env:computername\SQL$mssqlVersion -Path c:\projects\AdventureWorks$mssqlVersion.bak