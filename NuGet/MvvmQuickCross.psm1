function ReplaceStringsInString([string]$text, [Hashtable]$replacements)
{
    foreach ($replacement in $replacements.GetEnumerator())
    {
        $text = $text.Replace($replacement.Name, $replacement.Value)
    }
    $text
}

function ReplaceStringsInFile([string]$filePath, [Hashtable]$replacements)
{
    $content = [System.IO.File]::ReadAllText($filePath)
    $content = ReplaceStringsInString -text $content -replacements $replacements
    [System.IO.File]::WriteAllText($filePath, $content, [System.Text.Encoding]::UTF8)
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
        Write-Host "Installing MvvmQuickCross library support in project $projectName ..."
        $librarySourcePath = Join-Path -Path $toolsPath -ChildPath library
        $libraryDestinationPath = Split-Path -Path $project.FullName -Parent
        Get-ChildItem $librarySourcePath -Recurse | ForEach-Object {
            $relativePath = $_.FullName.Substring($librarySourcePath.Length)
            $destinationPath = Join-Path -Path $libraryDestinationPath -ChildPath $relativePath
            Copy-Item -Path $_.FullName -Destination $destinationPath -Force
            $dte.ItemOperations.AddExistingItem($destinationPath) # TODO: ***HERE*** use http://msdn.microsoft.com/en-us/library/envdte.projectitems.addfromfile.ASPX instead, maybe reuse some factory util code?
        $projectItems = (
            $project.ProjectItems.Item("_APPNAME_Application.cs"),
            $project.ProjectItems.Item("I_APPNAME_Navigator.cs"),
            $project.ProjectItems.Item("ViewModels").ProjectItems.Item("_VIEWNAME_ViewModel.cs")
        )

        # Replace variables in project item names and content
        foreach ($projectItem in $projectItems)
        {
            $projectItem.Name = ReplaceStringsInString -text $projectItem.Name -replacements $nameReplacements
            ReplaceStringsInFile -filePath $projectItem.Properties.Item("FullPath").Value -replacements $contentReplacements
        }
    }

    # TODO: 
    # Add the #define for the target framework, if needed.
    Write-Host "Installing MvvmQuickCross done."

}

Export-ModuleMember -Function Install-Mvvm
