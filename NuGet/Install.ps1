param($installPath, $toolsPath, $package, $project)

function ReplaceStringsInString([string]$text, [Hashtable]$replacements)
{
    foreach ($replacement in $replacements.GetEnumerator())
    {
        $text = $text.Replace($replacement.Name, $replacement.Value);
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

    $nameReplacements = @{
        "_APPNAME_" = $appName
    }

    $contentReplacements = @{
        "_APPNAME_" = $appName;
        "MvvmQuickCross.Templates" = $defaultNamespace
    }

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

Install -project $project
