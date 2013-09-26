param($installPath, $toolsPath, $package, $project)

Write-Host "Init: installPath = $installPath, toolsPath = $toolsPath, project = $($project.Name)"
if ((Get-Module MvvmQuickCross) -ne $null) { Remove-Module MvvmQuickCross }
Import-Module (Join-Path -Path $toolsPath -ChildPath MvvmQuickCross.psm1)

