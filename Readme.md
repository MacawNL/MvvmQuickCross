NuGet package: [http://nuget.org/packages/MvvmQuickCross](http://nuget.org/packages/mvvmquickcross)

# MvvmQuickCross
Quickly build cross-platform apps in C# with the MVVM pattern and [Xamarin](http://xamarin.com/).

## Summary
MvvmQuickCross is a lightweight cross-platform MVVM pattern to quickly build native Xamarin.iOS, Xamarin.Android, Windows Phone and Windows Store Apps with shared C# code.

MvvmQuickCross accelerates development, also for a single platform app. For cross-platform apps MvvmQuickCross increases code sharing.

MvvmQuickCross aims to leave you in full control; it does not get in the way if you want to do some things differently, and you can simply extend it.

##Features

**Lightweight and easy to modify**.<br />No binaries, only adds a code snippets file and C# source files to your projects.

**Scaffolders**.<br />Quickly add viewmodels and views from within the Visual Studio package manager console with the New-ViewModel and New-View commands.

**Viewmodel code snippets**.<br />Quickly add data-bindable properties and commands to viewmodels.

**Application-Navigator pattern**.<br />Maximize code sharing, including navigation logic, across platforms.

**Simple Android data binding**.<br />Specify bindings by naming conventions, in tag markup or in code. Use observable collections. Create performant data-bound list views without writing an adapter.

Override virtual methods in your activity or fragment to handle specific property change events with custom code instead of with data binding. Or customize how the data binding sets a value to a specific control. Add a few lines of code to make new view types data bindable.

**Android lifecycle management**.<br />Prevent [memory leaks in Xamarin](http://docs.xamarin.com/guides/android/application_fundamentals/activity_lifecycle) by automatically removing and re-adding event handlers during the Android activity life-cycle.

**Simple iOS data binding**.<br />This will be added in the **upcoming v2.0 release**.

## Getting Started
**Note: for a detailed step-by-step guide on how to build the CloudAuction example app that can be found in this repository, see [here](http://vincenth.net/blog/archive/2013/08/30/creating-a-cross-platform-native-app-using-mvvmquickcross-and-xamarin-part-1-cross-platform-code-and-windows-8-app.aspx) (however, be aware that this post has not yet been updated to show the new NuGet package install procedure; follow the steps below for that).**

**Coming up next: updated and detailed guidance (will be posted and linked to from here shortly), simple iOS data binding and an iOS example app.**

To create an app with MvvmQuickCross, follow these steps:

1. In Visual Studio, create a new solution with an application project for the platform (Windows Store, Windows Phone, Android, iOS) that you are most productive with. Add a class library project for that platform to the solution. Reference the class library from the application project.

2. Install the [MvvmQuickCross NuGet package](http://nuget.org/packages/mvvmquickcross)

	The available MvvmQuickCross commands are now displayed in the package manager console.
	Type Get-Help followed by a command for details.

3. In the Visual Studio package manager console (*menu View | Other Windows*) enter:

	**Install-Mvvm**

	An MvvmQuickCross folder is now added to your library project and your application project, and a few application-specific projects items are generated and opened in Visual Studio.

	**Note** that the package installation uses the first part of the solution filename (before the first dot) as the application name for naming new project items and classes.

4. Import the C# code snippets from the MvvmQuickCross\Templates\MvvmQuickCross.snippet file in your class library project into Visual Studio with the Code Snippets Manager (see [how](http://msdn.microsoft.com/en-us/library/ms165394\(v=vs.110\).aspx)).
	
	**Note** do not select the MvvmQuickCross\Templates folder itself as the location to import snippets **to**; that may prevent the snippets to be imported correctly, as this would mean copying the snippets file over itself.

5. Add new viewmodels with the **New-ViewModel** command.
	For android, you can also create new views with the **New-View** command.

6. Build the solution and check the TODO comments in the Visual Studio Task List *(menu View | Tast List)* to find guidance on how to complete the viewmodel, application and navigator classes. You can also check out the SampleApp and CloudAuction example apps in this GitHub repository. Detailed guidance will be added shortly.

## Adding platforms
To code your app for more platforms:


1. Create a new solution for each platform, with a class library project for that platform and an application project for that platform, just like you did for the first platform.

2. Add all code files from the existing class library project to the class library project for the new platform.

3. Install the MvvmQuickCross NuGet package and execute the Install-Mvvm command again (it won't overwrite existing files).

4. Code the views, navigator and any platform specific service implementations in the application project.
