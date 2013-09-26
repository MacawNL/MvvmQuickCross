if ((Get-Module MvvmQuickCross) -ne $null) 
{ 
    Remove-Module MvvmQuickCross
    Write-Host "Removed MvvmQuickCross PowerShell module"
}
