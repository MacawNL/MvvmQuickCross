param(
    [Parameter(Mandatory=$true)] [string]$installPath,
    [Parameter(Mandatory=$true)] [string]$toolsPath,
    [Parameter(Mandatory=$true)] $package,
    $project
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
Write-Host "installPath : $installPath"
Write-Host "toolsPath   : $toolsPath"
Write-Host "package     : $($package -ne $null)"

if ((Get-Module MvvmQuickCross) -ne $null) 
{
    Write-Host "Removing existing MvvmQuickCross module"
    Remove-Module -Name MvvmQuickCross 
}
$modulePath = Join-Path -Path $toolsPath -ChildPath MvvmQuickCross.psm1
Write-Host "Importing MvvmQuickCross module from $modulePath"
Import-Module -Name $modulePath
Write-Host "Available MvvmQuickCross Commands:"
Get-Command -Module MvvmQuickCross -Syntax | Out-Host