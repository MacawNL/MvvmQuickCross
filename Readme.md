NuGet package: [http://nuget.org/packages/MvvmQuickCross](http://nuget.org/packages/mvvmquickcross)

# MvvmQuickCross
Quickly build cross-platform apps in C# with the MVVM pattern and Xamarin.

## Summary
MvvmQuickCross is a cross-platform MVVM pattern to quickly build native iOS, Android, Windows Phone and Windows Store Apps with shared C# code and Xamarin.

MvvmQuickCross is lightweight and easy to modify: it adds no binaries, just one Visual Studio C# code snippets file and a few C# source files.

## Getting Started
**Note: detailed step-by-step guidance will be posted and linked to from here when version 1.0 is published (eta Sept 1, 2013)**

1. In Visual Studio, create a solution for the platform (Windows Store, Windows Phone, Android, iOS) that you are most productive with. Add a class library project for that platform and an application project for that platform. Reference the class library from the application project.

2. Install the [MvvmQuickCross NuGet package](http://nuget.org/packages/mvvmquickcross) in the class library project.
	
	**Note** that the package installation uses the first part of the solution file (before the first dot) as the application name for naming new project items and classes.

3. Import the C# code snippets from the MvvmQuickCross\MvvmQuickCross.snippet file into Visual Studio with the Code Snippets Manager (see [how](http://msdn.microsoft.com/en-us/library/ms165394(v=vs.110).aspx)).
	
	**Note** do not select the MvvmQuickCross folder itself as the location to import snippets **to**; that may prevent the snippets to be imported correctly, as this would mean copying the snippets file over itself.

4. Start coding, following this guidance:

	- The files ViewModels\\\_VIEWNAME\_ViewModel.cs, *\_APPNAME\_*Application.cs and I*\_APPNAME\_*Navigator.cs in your class library project
	- The example solution in this GitHub repository

To add more platforms, create a new solution for each platform, with a class library project and an application project, just like the first platform. Then add all existing shared code files to the class library for the new platform, and code the views, navigator and other platform specific service implementations in the application project.