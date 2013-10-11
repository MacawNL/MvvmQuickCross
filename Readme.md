NuGet package: [http://nuget.org/packages/MvvmQuickCross](http://nuget.org/packages/mvvmquickcross)
> NOTE: This readme describes version 1.5.1, which is the current NuGet release.

# MvvmQuickCross #
Quickly build cross-platform apps in C# with the MVVM pattern and [Xamarin](http://xamarin.com/).

## Summary ##
MvvmQuickCross is a lightweight cross-platform MVVM pattern to quickly build native Xamarin.iOS, Xamarin.Android, Windows Phone and Windows Store Apps with shared C# code.

MvvmQuickCross accelerates development, also for a single platform app. For cross-platform apps MvvmQuickCross increases code sharing.

MvvmQuickCross aims to leave you in full control; it does not get in the way if you want to do some things differently, and you can simply extend it.

## News ##
**Coming up**: Guidance on using the Android specific features in release 1.5.1 (Android simple data binding). Next planned release: 2.0, which will add simple iOS data binding and an iOS example app.

**October 9, 2013**: Version 1.5.1 is published. This release adds navigation code generation, Windows Phone support and more documentation.

**October 5, 2013**: Vincent Hoogendoorn gave a presentation on MvvmQuickCross at the Windows Phone Developer Day in The Netherlands. See [the slides](http://www.slideshare.net/VincentHoogendoorn/mvvm-quickcross-windows-phone-devday-2013) for an overview of the MvvmQuickCross shared code pattern and Windows Phone specifics.

## Features ##

**Lightweight and easy to modify**.<br />No binaries, only adds a code snippets file and C# source files to your projects.

**Scaffolders**.<br />Quickly add viewmodels and views from within the Visual Studio package manager console with the New-ViewModel and New-View [commands](#commands).

**Viewmodel code snippets**.<br />Quickly add data-bindable properties and commands to viewmodels with [code snippets](#code-snippets).

**Application-Navigator pattern**.<br />Maximize code sharing, including navigation logic, across platforms. See [these slides](http://www.slideshare.net/VincentHoogendoorn/mvvm-quickcross-windows-phone-devday-2013) for an overview of the MvvmQuickCross shared code pattern.

**Simple Android data binding**.<br />Specify [data bindings in Android](#android) by using naming conventions, tag markup or code. Use observable collections. Create performant data-bound list views without writing an adapter.

Override virtual methods in your activity or fragment to handle specific property change events with custom code instead of with data binding. Or customize how the data binding sets a value to a specific control. Add a few lines of code to make new view types data bindable.

**Android lifecycle management**.<br />Prevent [memory leaks in Xamarin](http://docs.xamarin.com/guides/android/application_fundamentals/activity_lifecycle) by automatically removing and re-adding event handlers during the Android activity life-cycle.

**Simple iOS data binding**.<br />This will be added in the **upcoming v2.0 release**.

## Getting Started ##
**Note: for a detailed step-by-step guide on how to build the CloudAuction example app that can be found in this repository, see [here](http://vincenth.net/blog/archive/2013/08/30/creating-a-cross-platform-native-app-using-mvvmquickcross-and-xamarin-part-1-cross-platform-code-and-windows-8-app.aspx) (however, be aware that this post has not yet been updated to show the new NuGet package install procedure; follow the steps below for that).**

To create an app with MvvmQuickCross, follow these steps:

1. In Visual Studio, create a new solution with an application project for the platform (Windows Store, Windows Phone, Android, iOS) that you are most productive with. Add a class library project for that platform to the solution. Reference the class library from the application project.

	**Note for Android:** Set the API level to 12 (Android 3.1) or higher in the Project properties for both projects. This is needed to support the Fragment view type. You can target lower API versions by either using the [Android Support Library](http://developer.android.com/tools/support-library/index.html) (which is supported in Xamarin) or by removing the Fragment view base class and template from the MvvmQuickCross folder in your application project.

	**Note for Windows Phone:** Select Windows Phone OS 8.0 or higher.

2. Install the [MvvmQuickCross NuGet package](http://nuget.org/packages/mvvmquickcross)

	The available MvvmQuickCross commands are now displayed in the package manager console.
	Type "**Get-Help *command* -Online**" for details.

3. In the Visual Studio package manager console (*menu View | Other Windows*) enter:

	**[Install-Mvvm](#install-mvvm)**

	An MvvmQuickCross folder is now added to your library project and your application project, and a few application-specific projects items are generated and opened in Visual Studio.

	**Note** that the package installation uses the first part of the solution filename (before the first dot) as the **application name** for naming new project items and classes.

4. Import the C# code snippets from the MvvmQuickCross\Templates\MvvmQuickCross.snippet file in your class library project into Visual Studio with the Code Snippets Manager (see [how](http://msdn.microsoft.com/en-us/library/ms165394\(v=vs.110\).aspx)). If you get a "Snippet With Same Name Exists" dialog, select Overwrite. 
	
	**Note** do not select the MvvmQuickCross\Templates folder itself as the location to import snippets **to**; that may prevent the snippets to be imported correctly, as this would mean copying the snippets file over itself.

5. Add new views and viewmodels with the **[New-View](#new-view)** and **[New-ViewModel](#new-viewmodel)** commands.

	**Note** that currently New-View only supports Android and Windows Phone.

6. Add data-bindable properties and commands to your viewmodels with the [code snippets](#code-snippets).

7. Check the TODO comments in the Visual Studio Task List *(menu View | Tast List)* to find guidance on how to complete the viewmodel, application and navigator classes. You can also check out the CloudAuction example app in this GitHub repository. More guidance will be added here shortly.

## Adding platforms ##
To code your app for more platforms:


1. Create a new solution for each platform, with a class library project for that platform and an application project for that platform, just like you did for the first platform.

2. Add all code files from the existing class library project to the class library project for the new platform.

3. Install the MvvmQuickCross NuGet package and execute the Install-Mvvm command again (it won't overwrite existing files).

4. Code the views, navigator and any platform specific service implementations in the application project.

## Commands ##
After installing the MvvmQuickCross NuGet package, the below commands are available in the Visual Studio **Package Manager Console**.

**Note** that except for Install-Mvvm, anything that these commands do can also be done by hand; the manual steps are documented inline in the files that you add to your projects with Install-Mvvm. This makes it possible to create your initial solutions in (a [free version](http://go.microsoft.com/fwlink/?linkid=244366) of) Visual Studio, and then continue working in [Xamarin Studio](http://xamarin.com/studio) for Android or iOS, if you prefer that.

### Install-Mvvm ###
    Install-Mvvm
Installs the MvvmQuickCross support files in both your library project and your application project, in a subfolder MvvmQuickCross. The files in the MvvmQuickCross folders are not application specific; unless you want to modify the standard MvvmQuickCross templates, code snippets and/or functionality you don't need to edit these.

Install-Mvvm also generates a few application-specific project items for you. The generated project items are opened in the Visual Studio editor for your inspection.

**Note** that Install-Mvvm uses the first part of the solution filename (before the first dot) as the **application name** for naming generated project items, classes, properties and methods.

Check the **TODO comments** in the Visual Studio **Task List** to find guidance on how to complete the generated project items.

Install-Mvvm will not overwrite existing files or code. If you want to recreate the default files, remove the files that you want to recreate before running Install-Mvvm.

### New-View ###
    New-View [-ViewName] <string> [[-ViewType] <string>] [[-ViewModelName] <string>] [-WithoutNavigation]
Generates a new view. Currently only supports Android and Windows Phone.

The specified **ViewName** will be suffixed with "View", and the specified **ViewModelName** will be suffixed with "ViewModel". If no ViewModelName is specified, it will be the same as the ViewName. If the view model does not exist, it will be generated with the **New-ViewModel** command.

On Windows Phone, the **ViewType** can be **Page** (default) or **UserControl**. On Android, it can be **MainLauncher**, **Activity** (default) or **Fragment**. The specified view type determines which view templates are used. You can find these templates in the MvvmQuickCross\Templates folder of your application project. You can simply modify these templates or add your own (which is better) by adding similar named files there.

Unless the **-WithoutNavigation** switch is specified, New-View will also add basic navigation code to the navigator and application classes. The -WithoutNavigation switch is useful when creating views such as list item views, that do not need to navigated to directly from the application class.

E.g. this command:

    New-View Person
will generate:

- A PersonView view markup file + class
- A PersonViewModel viewmodel class
- A PersonViewModelDesign viewmodel class
- A PersonViewModel property in the application class
- A NavigateToPersonView() method signature in the navigator interface
- A NavigateToPersonView() method implementation in the navigator class
- A ContinueToPerson() method in the application class

Now the only thing needed to display the view, bound to the view model, is to call the ContinueToPerson() method on the application.

Check the **TODO comments** in the Visual Studio **Task List** to find guidance on how to complete the generated project items.

New-View will not overwrite existing files or code. If you want to recreate files or code fragments, remove the existing one(s) first.
  
### New-ViewModel ###
    New-ViewModel [-ViewModelName] <string> [-NotInApplication]
Generates a new viewmodel. You can use this command to create viewmodels without creating any corresponding views (yet).

The specified **ViewModelName** will be suffixed with "ViewModel".

Unless the **-NotInApplication** switch is specified, New-ViewModel will also add a property to contain the instance of the viewmodel to the application class. The application class will then be responsible for providing an initialized viewmodel instance before navigating to the corresponding view. The -NotInApplication switch is useful when creating viewmodels such as list item viewmodels, that do not need to be instantiated and initialized directly by the application class.

E.g. this command:

    New-ViewModel Person
will generate:

- A PersonViewModel viewmodel class
- A PersonViewModelDesign viewmodel class
- A PersonViewModel property in the application class

Check the **TODO comments** in the Visual Studio **Task List** to find guidance on how to complete the generated project items.

New-ViewModel will not overwrite existing files or code. If you want to recreate files or code fragments, remove the existing one(s) first.

## Code Snippets ##
When you run the Install-Mvvm command, the C# code snippets file **MvvmQuickCross\Templates\MvvmQuickCross.snippet** is added to your class library project. When you import this snippets file into Visual Studio with the Code Snippets Manager (see [how](http://msdn.microsoft.com/en-us/library/ms165394\(v=vs.110\).aspx)), the code snippets described below become available for coding viewmodels.

Note that the code snippets and their parameters have intellisense when you invoke them in the Visual Studio C# editor.

To instantiate a code snippet, place your cursor on an empty line in the "**Data-bindable properties and commands**" region of a viewmodel class .cs file, type the code snippet shortcut (e.g. propdb1), and press Tab twice. Now you can enter the parameters of the code snippet (press Tab to cycle through all parameters). Press Enter to complete the snippet instance.

### propdb1 ###
Adds a one-way data-bindable property to a Viewmodel. You can specify the property type and name.
### propdbcol ###
Adds a one-way data-bindable collection property, a corresponding **(name)HasItems** property and an **Update(name)HasItems()** method to a Viewmodel. You can specify the generic collection type (e.g. **ObservableCollection** or **List**), the collection element type and the property name.

If you want specific UI elements in your view to only be visible when the collection has elements (e.g. a list of error messages), you can use the HasItems property to bind to the visibility of those UI elements. In that case, you should also call the UpdateHasItems() method after you have added or removed items (this is necessary even if this is an ObservableCollection).

**Note** that if you suffix a collection property name with **"List"**, you can benefit from data binding naming conventions in Android.

### propdb2 ###
Adds a two-way data-bindable property to a Viewmodel. You can specify the property type and name.
### propdb2c ###
Adds a two-way data-bindable property and a **On(name)Changed()** method for custom setter code to a Viewmodel. You can specify the property type and name.
### cmd ###
Adds a data-bindable command to a Viewmodel. You can specify the command name, which will be suffixed with **"Command"**.
### cmdp ###
Adds a data-bindable command with a parameter to a Viewmodel. You can specify the command name, which will be suffixed with **"Command"**, the parameter type and the parameter name.

## Android ##
Below is an overview of using MvvmQuickCross with Xamarin.Android. For a more detailed example, see the Android version of the [CloudAuction example application](http://github.com/MacawNL/MvvmQuickCross/tree/master/Examples/CloudAuction) in this repository.

### Create an Android App ###
Here is how to create an Android Twitter app that demonstrates simple data binding:
> WORK IN PROGRESS: this example is in writing; eta is October 11, 2013.

1.  Create a working Android app by following steps 1 though 4 of [Getting Started](#getting-started) above - choose an **Android Application** project named "Twitter" and an **Android Class Library** project named "Twitter.Shared".

2.  Remove the **Activity1.cs** and **Class1.cs** files that were included by the Xamarin.Android project templates

3.  Now you can run the app on your device and test the example MainView that was generated by the Install-Mvvm command.

4.  Create a **Models** folder in your Twitter.Shared project and create this **Tweet.cs** class in it:

    namespace Twitter.Shared.Models
	{
	    public class Tweet
	    {
	        public string Text { get; set; }
	        public string UserName { get; set; }
	        public int RetweetCount { get; set; }
	    }
	}

5.  In **ViewModels\MainViewModel.cs** in your library project, in the **region Data-bindable properties and commands**, add these properties and commands with the indicated [code snippets](#code-snippets):

	<table>
	<tr><td><b>Snippet</b></td><td><b>Parameters</b></td><td><b>Generated code</b></td></tr>
	<tr><td><a href="#propdb2c">propdb2c</a></td><td>string Tweet</td><td>public string Tweet ...<br />private void OnTweetChanged() { ... }</td></tr>
	<tr><td><a href="#propdb">propdb</a></td><td>int CharactersLeft</td><td>public int CharactersLeft ...</td></tr>
	<tr><td><a href="#cmd">cmd</a></td><td>Send</td><td>public RelayCommand SendCommand ...<br />private void Send() { ... }</td></tr>
	</table>


TODO: Document how to use the Android specific MvvmQuickCross features (Android Simple Data Binding).

