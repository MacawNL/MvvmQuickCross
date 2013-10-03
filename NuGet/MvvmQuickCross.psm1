$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

function ReplaceStringsInString
{
    Param(
        [Parameter(Mandatory=$true)] [string]$text,
        [Hashtable]$replacements
    )

    if ($replacements -ne $null) 
    {
        foreach ($replacement in $replacements.GetEnumerator())
        {
            $text = $text -replace $replacement.Name, $replacement.Value
        }
    }
    $text
}

function ReplaceStringsInFile
{
    Param(
        [Parameter(Mandatory=$true)] [string]$filePath,
        [Hashtable]$replacements
    )

    if ($replacements -ne $null) 
    {
        $content = [System.IO.File]::ReadAllText($filePath)
        $content = ReplaceStringsInString -text $content -replacements $replacements
        [System.IO.File]::WriteAllText($filePath, $content, [System.Text.Encoding]::UTF8)
    }
}

function AddProjectItemsFromDirectory
{
    Param(
        [Parameter(Mandatory=$true)] $project,
        [Parameter(Mandatory=$true)] [string]$sourceDirectory,
        [string]$destinationDirectory,
        [Hashtable]$nameReplacements,
        [Hashtable]$contentReplacements
    )

    if (-not(Test-Path -Path $sourceDirectory)) { return }
    if ("$destinationDirectory" -eq '') { $destinationDirectory = Split-Path -Path $project.FullName -Parent }

    Get-ChildItem $sourceDirectory | ForEach-Object {
        $itemName = ReplaceStringsInString -text $_.Name -replacements $nameReplacements
        $destinationPath = Join-Path -Path $destinationDirectory -ChildPath $itemName
        if ($_.PSIsContainer)
        {
            if (-not (Test-Path -Path $destinationPath)) { $null = New-Item -Path $destinationPath -ItemType directory }
            AddProjectItemsFromDirectory -sourceDirectory $_.FullName -destinationDirectory $destinationPath -project $project
        } else {
            Write-Host "Adding project item: $destinationPath"
            Copy-Item -Path $_.FullName -Destination $destinationPath -Force
            ReplaceStringsInFile -filePath $destinationPath -replacements $contentReplacements
            $null = $project.ProjectItems.AddFromFile($destinationPath)
        }
    }
}

function GetProjectPlatform
{
    Param([Parameter(Mandatory=$true)] $project)

    $targetFrameworkMoniker = $project.Properties.Item("TargetFrameworkMoniker").Value
    # E.g. valid target framework monikers are:
    # Windows Store:   .NETCore,Version=v4.5
    # Windows Phone:   WindowsPhone,Version=v8.0
    # Xamarin.Android: MonoAndroid,Version=v4.2
    # Xamarin.iOS:     TODO CHECK: MonoTouch?,?

    $targetFrameworkName = $targetFrameworkMoniker.Split(',')[0]
    switch ($targetFrameworkName)
    {
        'MonoAndroid'  { $platform = "android" }
        'MonoTouch'    { $platform = "ios"     }
        '.NETCore'     { $platform = "ws"      }
        'WindowsPhone' { $platform = "wp"      }
        default        { throw "Unsupported target framework: " + $targetFrameworkName }
    }

    $platform
}

Function IsApplicationProject
{
    Param([Parameter(Mandatory=$true)] $project)

    $projectFileContent = [System.IO.File]::ReadAllText($project.FullName)
    $platform = GetProjectPlatform -project $project

    switch ($platform)
    {
        'android' { $isApplication = $projectFileContent -match '<\s*AndroidApplication\s*>\s*true\s*</\s*AndroidApplication\s*>' }
        'ios'     { $isApplication = $projectFileContent -match '<\s*OutputType\s*>\s*Exe\s*</\s*OutputType\s*>' }
        'ws'      { $isApplication = $projectFileContent -match '<\s*OutputType\s*>\s*AppContainerExe\s*</\s*OutputType\s*>' }
        'wp'      { $isApplication = $projectFileContent -match '<\s*SilverlightApplication\s*>\s*true\s*</\s*SilverlightApplication\s*>' }
        default   { throw "Unknown platform: " + $platform }
    }

    $isApplication
}

function EnsureConditionalCompilationSymbol
{
    Param(
        [Parameter(Mandatory=$true)] $project,
        [Parameter(Mandatory=$true)] [string]$define
    )

    if ($define -ne $null)
    {
        # Add the #define for the target framework, if needed.
        Write-Host "Ensuring $define conditional compilation symbol for all project configurations and platforms"
        $project.ConfigurationManager.ConfigurationRowNames | foreach-object {
            $project.ConfigurationManager.ConfigurationRow($_) | foreach-object { 
                $property = $_.Properties.Item('DefineConstants')
                if ($property -ne $null)
                {
                    $value = "$($property.value)".Trim()
                    if ($value -notmatch "\W*$define\W*") {
                        if ($value -ne '') { $value += ';' }
                        $value += $define
                        $property.value = $value
                    }
                }
            } 
        }
    }
}

function Install-Mvvm
{
    Param(
       [string]$ProjectName
    )

    if ("$ProjectName" -eq '') { $project = Get-Project } else { $project = Get-Project $ProjectName }
    if ($project -eq $null)  { Write-Host "Project '$ProjectName' not found."; return }
    $ProjectName = $project.Name

    # Get the application name from the solution file name
    $solutionName = Split-Path ($project.DTE.Solution.FullName) -Leaf
    $appName = $solutionName.Split('.')[0]
    $defaultNamespace = $project.Properties.Item("DefaultNamespace").Value
    $platform = GetProjectPlatform -project $project
    $isApplication = IsApplicationProject -project $project

    Write-Host ("Project {0}: platform = {1}, project type = {2}" -f $project.Name, $platform, ('library', 'app')[$isApplication])

    $toolsPath = $PSScriptRoot
    $nameReplacements = @{
        "_APPNAME_" = $appName
    }
    $contentReplacements = @{
        "_APPNAME_" = $appName;
        "MvvmQuickCross\.Templates" = $defaultNamespace
    }

    $installSharedCode = -not $isApplication
    # TODO: ?check if the shared code is already present in another (referenced?) project in the solution, if not, install the shared code into the same project - to also support one-project solutions?
    #       OR: if shared code not installed, fail and give message to install and reference first? nonblocking Dialog needed?
    if ($installSharedCode) # Do the shared library file actions
    {
        Write-Host "Installing MvvmQuickCross library files"
        $librarySourceDirectory = Join-Path -Path $toolsPath -ChildPath library
        AddProjectItemsFromDirectory -project $project -sourceDirectory $librarySourceDirectory -nameReplacements $nameReplacements -contentReplacements $contentReplacements
    }

    if ($isApplication) {
        Write-Host "Installing MvvmQuickCross app files for platform $platform"
        $appSourceDirectory = Join-Path -Path $toolsPath -ChildPath "app.$platform"
        AddProjectItemsFromDirectory -project $project -sourceDirectory $appSourceDirectory -nameReplacements $nameReplacements -contentReplacements $contentReplacements

        if ($platform -eq 'android')
        {
            # TODO: Set build action of Resources\Layout\_VIEWNAME_View.axml to None
        }
    }

    $platformDefines = @{
        'android' = '__ANDROID__';
        'ios'     = '__IOS__';
        'ws'      = 'NETFX_CORE';
        'wp'      = 'WINDOWS_PHONE'
    }
    EnsureConditionalCompilationSymbol -project $project -define $platformDefines[$platform]

    Write-Host "MvvmQuickCross is installed in project $ProjectName" 
}

function New-ViewModel
{
    Param(
        [Parameter(Mandatory=$true)] [string]$ViewModelName,
        [string]$ProjectName
    )

    if ("$ProjectName" -eq '') { $project = Get-Project } else { $project = Get-Project $ProjectName }
    if ($project -eq $null)  { Write-Host "Project '$ProjectName' not found."; return }
    $ProjectName = $project.Name
    
    if (IsApplicationProject -project $project)
    {
        Write-Host "Project $ProjectName is an application project; view models should be coded in a library project. Specify a library project with the ProjectName parameter or select a library project as the default project in the Package Manager Console."
        return
    }

    # TODO: use AddProjectItem
    $projectFolder = Split-Path -Path $project.FullName -Parent
    $templatePath = Join-Path -Path $projectFolder -ChildPath 'ViewModels\_VIEWNAME_ViewModel.cs'
    if (-not(Test-Path $templatePath))
    {
        $toolsPath = $PSScriptRoot
        $templatePath = Join-Path -Path $toolsPath -ChildPath 'library\ViewModels\_VIEWNAME_ViewModel.cs'
        if (-not(Test-Path $templatePath)) { throw "Viewmodel template file not found: $templatePath" }
    }

    $destinationFolder = Join-Path -Path $projectFolder -ChildPath ViewModels
    if (-not(Test-Path -Path $destinationFolder)) { $null = New-Item $destinationFolder -ItemType Directory -Force }
    $destinationPath = Join-Path -Path $destinationFolder -ChildPath ('{0}ViewModel.cs' -f $ViewModelName)
    Copy-Item -Path $templatePath -Destination $destinationPath -Force

    $defaultNamespace = $project.Properties.Item("DefaultNamespace").Value
    $contentReplacements = @{
        'MvvmQuickCross.Templates' = $defaultNamespace;
        '_VIEWMODEL_' = $ViewModelName;
        '(?m)^\s*#if\s+TEMPLATE\s+[^\r\n]*[\r\n]+' = '';
        '(?m)^\s*#endif\s+//\s*TEMPLATE[^\r\n]*[\r\n]*' = ''
    }

    ReplaceStringsInFile -filePath $destinationPath -replacements $contentReplacements

    $null = $project.ProjectItems.AddFromFile($destinationPath)
    $null = $dte.ItemOperations.OpenFile($destinationPath)
}

function AddProjectItem
{
    Param(
        [Parameter(Mandatory=$true)] $project,
        [Parameter(Mandatory=$true)] [string]$templateProjectRelativePath,
        [Parameter(Mandatory=$true)] [string]$destinationProjectRelativePath,
        [Hashtable] $contentReplacements
    )

    $projectFolder = Split-Path -Path $project.FullName -Parent

    $templatePath = Join-Path -Path $projectFolder -ChildPath $templateProjectRelativePath
    if (-not(Test-Path $templatePath))
    {
        $toolsPath = $PSScriptRoot
        $templatePath = Join-Path -Path $toolsPath -ChildPath "app.android\$templateProjectRelativePath"
        if (-not(Test-Path $templatePath)) { throw "Template file not found: $templatePath" }
    }

    $destinationPath = Join-Path -Path $projectFolder -ChildPath $destinationProjectRelativePath
    $destinationFolder = Split-Path -Path $destinationPath -Parent
    if (-not(Test-Path -Path $destinationFolder)) { $null = New-Item $destinationFolder -ItemType Directory -Force }

    if (Test-Path -Path $destinationPath) { Write-Host "Project item not created because it already exists: $destinationPath"; return }
    Copy-Item -Path $templatePath -Destination $destinationPath -Force
    ReplaceStringsInFile -filePath $destinationPath -replacements $contentReplacements

    $null = $project.ProjectItems.AddFromFile($destinationPath)
    $null = $dte.ItemOperations.OpenFile($destinationPath)
}

function New-View
{
    Param(
        [Parameter(Mandatory=$true)] [string]$ViewName,
        [string]$ProjectName
    )

    if ("$ProjectName" -eq '') { $project = Get-Project } else { $project = Get-Project $ProjectName }
    if ($project -eq $null)  { Write-Host "Project '$ProjectName' not found."; return }
    $ProjectName = $project.Name
    
    if (-not(IsApplicationProject -project $project))
    {
        Write-Host "Project $ProjectName is not an application project; views should be coded in an application project. Specify an application project with the ProjectName parameter or select an application project as the default project in the Package Manager Console."
        return
    }

    $solutionName = Split-Path ($project.DTE.Solution.FullName) -Leaf
    $appName = $solutionName.Split('.')[0]
    $defaultNamespace = $project.Properties.Item("DefaultNamespace").Value

    # TODO: Move to function
    $csContentReplacements = @{
        'MvvmQuickCross.Templates' = $defaultNamespace;
        '_APPNAME_' = $appName;
        '_VIEWNAME_' = $ViewName;
        '(?m)^\s*#if\s+TEMPLATE\s+[^\r\n]*[\r\n]+' = '';
        '(?m)^\s*#endif\s+//\s*TEMPLATE[^\r\n]*[\r\n]*' = ''
    }

    $platform = GetProjectPlatform -project $project
    switch ($platform)
    {
        'android' {
            AddProjectItem -project $project `
                           -templateProjectRelativePath    'MvvmQuickCross\_VIEWNAME_ActivityView.cs' `
                           -destinationProjectRelativePath ('{0}View.cs' -f $ViewName) `
                           -contentReplacements            $csContentReplacements
            AddProjectItem -project $project `
                           -templateProjectRelativePath    'MvvmQuickCross\_VIEWNAME_View.axml' `
                           -destinationProjectRelativePath ('Resources\Layout\{0}View.axml' -f $ViewName) `
                           -contentReplacements            @{ '_VIEWNAME_' = $ViewName }
        }

        default { Write-Host "New-View currenty only supports Android application projects"; return }
    }
}

Export-ModuleMember -Function Install-Mvvm
Export-ModuleMember -Function New-ViewModel
Export-ModuleMember -Function New-View