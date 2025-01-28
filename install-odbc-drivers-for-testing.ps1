Write-Host "Deploying Microsoft Excel testing environment"

# Installing ODBC driver
Write-host "`tDeploying Microsoft Excel ODBC drivers"
$drivers = Get-OdbcDriver -Name "*xlsx*" -Platform "64-bit"

If ($drivers.Length -eq 0) {
	Write-Host "`t`tDownloading Microsoft Excel ODBC driver ..."
	Invoke-WebRequest "https://download.microsoft.com/download/3/5/C/35C84C36-661A-44E6-9324-8786B8DBE231/accessdatabaseengine_X64.exe" -OutFile "$env:temp\accessdatabaseengine_X64.exe"
	Write-Host "`t`tInstalling Microsoft Excel ODBC driver ..."
	& cmd /c start /wait "$env:temp\accessdatabaseengine_X64.exe" /quiet
	Write-Host "`t`tChecking installation ..."
	Get-OdbcDriver -Name "*xlsx*"
	Write-Host "`tDeployment of Microsoft Excel ODBC driver finalized."
} else {
	Write-Host "`t`tDrivers already installed:"
	Get-OdbcDriver -Name "*xlsx*" -Platform "64-bit"
	Write-Host "`t`tSkipping installation of new drivers"
}
