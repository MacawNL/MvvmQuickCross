param($installPath, $toolsPath, $package, $project)

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

function Install($project)
{
    # Get the application name from the solution file name
    $solutionName = Split-Path ($project.DTE.Solution.FullName) -Leaf
    $appName = $solutionName.Split('.')[0]
    $defaultNamespace = $project.Properties.Item("DefaultNamespace").Value
    $projectFileContent = [System.IO.File]::ReadAllText($project.FullName)

	# Determine the platform (android/ios/ws/wp) and project type (app / shared):
    if ($projectFileContent -match '<\s*Reference\s+Include\s*=\s*"Mono.Android"') {
        $platform = "android"
        $isApplication = $projectFileContent -match '<\s*AndroidApplication\s*>\s*true\s*</\s*AndroidApplication\s*>'
    } elseif ($projectFileContent -match '<\s*Reference\s+Include\s*=\s*"monotouch"') {
        $platform = "ios"
        $isApplication = $projectFileContent -match '<\s*OutputType\s*>\s*Exe\s*</\s*OutputType\s*>'
    } elseif ($projectFileContent -match '<\s*ProjectTypeGuids\s*>[^<]*\{BC8A1FFA-BEE3-4634-8014-F334798102B3}[^<]*</\s*ProjectTypeGuids\s*>') {
        # TODO: check if ws guid is the same in visual studio 2013
        $platform = "ws"
        $isApplication = $projectFileContent -match '<\s*OutputType\s*>\s*AppContainerExe\s*</\s*OutputType\s*>'
    } elseif ($projectFileContent -match '<\s*ProjectTypeGuids\s*>[^<]*\{C089C8C0-30E0-4E22-80C0-CE093F111A43}[^<]*</\s*ProjectTypeGuids\s*>') {
        # TODO: check if wp guid is the same in visual studio 2013
        $platform = "wp"
        $isApplication = $projectFileContent -match '<\s*SilverlightApplication\s*>\s*true\s*</\s*SilverlightApplication\s*>'
    } else {
        throw "Cannot determine target platform of project file " + $project.FullName
    }

    
    $nameReplacements = @{
        "_APPNAME_" = $appName
    }

    $contentReplacements = @{
        "_APPNAME_" = $appName;
        "MvvmQuickCross.Templates" = $defaultNamespace
    }


    if (-not $isApplication) # Do the shared library file actions
    {
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
}

Install -project $project
