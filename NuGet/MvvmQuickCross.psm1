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
            $text = $text.Replace($replacement.Name, $replacement.Value)
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

function Install-Mvvm
{
    Param(
       [string]$projectName
    )

    if ("$projectName" -eq '') { $project = Get-Project } else { $project = Get-Project $projectName }
    if ($project -eq $null)  { Write-Host "Project '$projectName' not found."; return }
    $projectName = $project.Name

    # Get the application name from the solution file name
    $solutionName = Split-Path ($project.DTE.Solution.FullName) -Leaf
    $appName = $solutionName.Split('.')[0]
    $defaultNamespace = $project.Properties.Item("DefaultNamespace").Value
    $targetFrameworkMoniker = $project.Properties.Item("TargetFrameworkMoniker").Value
    # E.g. valid target framework monikers are:
    # Windows Store:   .NETCore,Version=v4.5
    # Windows Phone:   WindowsPhone,Version=v8.0
    # Xamarin.Android: MonoAndroid,Version=v4.2
    # Xamarin.iOS:     TODO MonoTouch,?
    # Portable:        .NETPortable,Version=v4.5,Profile=Profile78

    $targetFrameworkName = $targetFrameworkMoniker.Split(',')[0]
    $projectFileContent = [System.IO.File]::ReadAllText($project.FullName)

    switch ($targetFrameworkName)
    {
        'MonoAndroid'  { $platform = "android"; $define = '__ANDROID__'  ; $isApplication = $projectFileContent -match '<\s*AndroidApplication\s*>\s*true\s*</\s*AndroidApplication\s*>' }
        'MonoTouch'    { $platform = "ios"    ; $define = '__IOS__'      ; $isApplication = $projectFileContent -match '<\s*OutputType\s*>\s*Exe\s*</\s*OutputType\s*>' }
        '.NETCore'     { $platform = "ws"     ; $define = 'NETFX_CORE'   ; $isApplication = $projectFileContent -match '<\s*OutputType\s*>\s*AppContainerExe\s*</\s*OutputType\s*>' }
        'WindowsPhone' { $platform = "wp"     ; $define = 'WINDOWS_PHONE'; $isApplication = $projectFileContent -match '<\s*SilverlightApplication\s*>\s*true\s*</\s*SilverlightApplication\s*>' }
        '.NETPortable' { $platform = $null    ; $define = $null          ; $isApplication = $false }
        default        { throw "Unsupported target framework: " + $targetFrameworkName }
    }

    Write-Host ("Project {0}: platform = {1}, project type = {2}" -f $project.Name, $platform, ('library', 'app')[$isApplication])

    $toolsPath = $PSScriptRoot

    $nameReplacements = @{
        "_APPNAME_" = $appName
    }

    $contentReplacements = @{
        "_APPNAME_" = $appName;
        "MvvmQuickCross.Templates" = $defaultNamespace
    }

    $installSharedCode = -not $isApplication
    # TODO: ?check if the shared code is already present in another (referenced?) project in the solution, if not, install the shared code into the same project - to also support one-project solutions?
    #       OR: if shared code not installed, fail and give message to install and reference first? nonblocking Dialog needed?
    if ($installSharedCode) # Do the shared library file actions
    {
        Write-Host "Installing MvvmQuickCross library files in project $projectName"
        $librarySourceDirectory = Join-Path -Path $toolsPath -ChildPath library
        AddProjectItemsFromDirectory -project $project -sourceDirectory $librarySourceDirectory -nameReplacements $nameReplacements -contentReplacements $contentReplacements
    }

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


    Write-Host "MvvmQuickCross is installed in project $projectName" 
}

Export-ModuleMember -Function Install-Mvvm
