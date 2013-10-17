NuGet package: [http://nuget.org/packages/MvvmQuickCross](http://nuget.org/packages/mvvmquickcross)
> NOTE: This readme describes version 1.5.2, which is the current NuGet release.

# MvvmQuickCross #
Quickly build cross-platform apps in C# with the MVVM pattern and [Xamarin](http://xamarin.com/).

## Summary ##
MvvmQuickCross is a lightweight cross-platform MVVM pattern to quickly build native Xamarin.iOS, Xamarin.Android, Windows Phone and Windows Store Apps with shared C# code.

MvvmQuickCross accelerates development, also for a single platform app. For cross-platform apps MvvmQuickCross increases code sharing.

MvvmQuickCross aims to leave you in full control; it does not get in the way if you want to do some things differently, and you can simply extend it.

## News ##
**Coming up**: A blogpost on building the CloudAuction application for Android; Next planned MvvmQuickCross release: 2.0, which will add simple iOS data binding and an iOS example app.

**October 17, 2013**: The Cross-Platform features and the Android-specific features are now fully documented in this Readme.

**October 14, 2013**: Version 1.5.2 is published. This release adds support for command enabling/disabling and for list highlighting in Android, and more documentation.

**October 9, 2013**: Version 1.5.1 is published. This release adds navigation code generation, Windows Phone support and more documentation.

**October 5, 2013**: Vincent Hoogendoorn gave a presentation on MvvmQuickCross at the Windows Phone Developer Day in The Netherlands. See [the slides](http://www.slideshare.net/VincentHoogendoorn/mvvm-quickcross-windows-phone-devday-2013) for an overview of the MvvmQuickCross shared code pattern and Windows Phone specifics.

## Features ##

**Lightweight and easy to modify**.<br />No binaries, only adds a code snippets file and C# source files to your projects.

**Scaffolders**.<br />Quickly add viewmodels and views from within the Visual Studio package manager console with the New-ViewModel and New-View [commands](#commands).

**Viewmodel code snippets**.<br />Quickly add data-bindable properties and commands to viewmodels with [code snippets](#code-snippets).

**Application-Navigator pattern**.<br />Maximize code sharing, including navigation logic, across platforms. See [these slides](http://www.slideshare.net/VincentHoogendoorn/mvvm-quickcross-windows-phone-devday-2013) for an overview of the MvvmQuickCross shared code pattern.

**Simple Android data binding**.<br />Specify [data bindings in Android](#android-data-binding) by using naming conventions, tag markup or code. Use observable collections. Create performant data-bound list views without writing an adapter.

Override virtual methods in your activity or fragment to handle specific property change events with custom code instead of with data binding. Or customize how the data binding sets a value to a specific control. Add a few lines of code to make new view types data bindable.

**Android lifecycle management**.<br />Prevent [memory leaks in Xamarin](http://docs.xamarin.com/guides/android/application_fundamentals/activity_lifecycle) by [automatically removing and re-adding event handlers](#android-view-lifecycle-support) during the Android activity life-cycle.

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

5. Add new views and viewmodels with the [`New-View`](#new-view) and [`New-ViewModel`](#new-viewmodel) commands.

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

```posh
Install-Mvvm 
```
Installs the MvvmQuickCross support files in both your library project and your application project, in a subfolder MvvmQuickCross. The files in the MvvmQuickCross folders are not application specific; unless you want to modify the standard MvvmQuickCross templates, code snippets and/or functionality you don't need to edit these.

Install-Mvvm also generates a few application-specific project items for you. The generated project items are opened in the Visual Studio editor for your inspection.

**Note** that Install-Mvvm uses the first part of the solution filename (before the first dot) as the **application name** for naming generated project items, classes, properties and methods.

Check the **TODO comments** in the Visual Studio **Task List** to find guidance on how to complete the generated project items.

Install-Mvvm will not overwrite existing files or code. If you want to recreate the default files, remove the files that you want to recreate before running Install-Mvvm.

### New-View ###

```posh
New-View [-ViewName] <string> [[-ViewType] <string>] [[-ViewModelName] <string>] [-WithoutNavigation]
```
Generates a new view. Currently only supports Android and Windows Phone.

The specified `ViewName` will be suffixed with "View", and the specified `ViewModelName` will be suffixed with "ViewModel". If no ViewModelName is specified, it will be the same as the ViewName. If the view model does not exist, it will be generated with the `New-ViewModel` command.

On Windows Phone, the `ViewType` can be `Page` (default) or `UserControl`. On Android, it can be `MainLauncher`, `Activity` (default) or `Fragment`. The specified view type determines which view templates are used. You can find these templates in the MvvmQuickCross\Templates folder of your application project. You can simply modify these templates or add your own (which is better) by adding similar named files there.

Unless the `-WithoutNavigation` switch is specified, New-View will also add basic navigation code to the navigator and application classes. The -WithoutNavigation switch is useful when creating views such as list item views, that do not need to navigated to directly from the application class.

E.g. this command:

```posh
New-View Person
```
will generate:

- A `PersonView` view markup file + class
- A `PersonViewModel` viewmodel class
- A `PersonViewModelDesign` viewmodel class
- A `PersonViewModel` property in the application class
- A `NavigateToPersonView()` method signature in the navigator interface
- A `NavigateToPersonView()` method implementation in the navigator class
- A `ContinueToPerson()` method in the application class

Now the only thing needed to display the view, bound to the view model, is to call the `ContinueToPerson()` method on the application.

Check the **TODO comments** in the Visual Studio **Task List** to find guidance on how to complete the generated project items.

New-View will not overwrite existing files or code. If you want to recreate files or code fragments, remove the existing one(s) first.
  
### New-ViewModel ###

```posh
New-ViewModel [-ViewModelName] <string> [-NotInApplication]
```
Generates a new viewmodel. You can use this command to create viewmodels without creating any corresponding views (yet).

The specified `ViewModelName` will be suffixed with "ViewModel".

Unless the `-NotInApplication` switch is specified, New-ViewModel will also add a property to contain the instance of the viewmodel to the application class. The application class will then be responsible for providing an initialized viewmodel instance before navigating to the corresponding view. The -NotInApplication switch is useful when creating viewmodels such as list item viewmodels, that do not need to be instantiated and initialized directly by the application class.

E.g. this command:

```posh
New-ViewModel Person
```
will generate:

- A PersonViewModel viewmodel class
- A PersonViewModelDesign viewmodel class
- A PersonViewModel property in the application class

Check the **TODO comments** in the Visual Studio **Task List** to find guidance on how to complete the generated project items.

New-ViewModel will not overwrite existing files or code. If you want to recreate files or code fragments, remove the existing one(s) first.

### Customizing and Extending Command Templates ###
As Described above in the usage of the ViewType parameter of the [New-View command](#new-view), you can add your own code and markup templates for custom view types by adding properly named files in the `MvvmQuickCross\Templates` folder of your application project.

The code template files are named:   `_VIEWNAME_<view type name>View.cs`
The markup template files are named: `_VIEWNAME_<view type name>View.axml.template`
If no markup file for the specified view type is found, this file is used: `_VIEWNAME_View.axml.template`, so you do not need to have multiple copies of the same markup template.

Of course you can also modify existing templates in the MvvmQuickCross\Templates folder.

The library project also contains a viewmodel template that can be customized, at `MvvmQuickCross\Templates\_VIEWNAME_ViewModel.cs`.

In addition to the template files, you can also modify the inline code templates in the Application, INavigator and Navigator. Inline templates are used to add properties and methods in these files when a new viewmodel or view is added. An example of an inline template in the Application class for a `view` is:

```csharp
public sealed class _APPNAME_Application : ApplicationBase
{
	...

    /* TODO: For each view, add a method (with any parameters needed) to initialize its viewmodel
     * and then navigate to the view using the navigator, like this:

    public void ContinueTo_VIEWNAME_(bool skipNavigation = false)
    {
        if (_VIEWNAME_ViewModel == null) _VIEWNAME_ViewModel = new _VIEWNAME_ViewModelDesign();
        if (!skipNavigation) RunOnUIThread(
			() => _navigator.NavigateTo_VIEWNAME_View(CurrentNavigationContext)
		);
    }
     * The skipNavigation parameter is needed in cases where the OS has already navigated
     * to the view for you; in that case you only need to initialize the viewmodel.
     * Note that the New-View command adds the above code automatically 
     * (see http://github.com/MacawNL/MvvmQuickCross#new-view). */
}
```
The format of an inline template is:

1. An empty line. The instantiated code will be placed directly before this line
2. A line starting with `/* TODO: For each <template name>,`
3. Optionally some lines starting with ` * `
4. Lines of actual template code
5. Optionally some lines starting with ` * `
6. A line ending with  ` */`

Currently the template name can be `view` or `viewmodel`.

## Code Snippets ##
When you run the Install-Mvvm command, the C# code snippets file `MvvmQuickCross\Templates\MvvmQuickCross.snippet` is added to your class library project. When you import this snippets file into Visual Studio with the Code Snippets Manager (see [how](http://msdn.microsoft.com/en-us/library/ms165394\(v=vs.110\).aspx)), the code snippets described below become available for coding viewmodels.

Note that the code snippets and their parameters have intellisense when you invoke them in the Visual Studio C# editor.

To instantiate a code snippet, place your cursor on an empty line in the `#region Data-bindable properties and commands` of a viewmodel class .cs file, type the code snippet shortcut (e.g. `propdb1`), and press Tab twice. Now you can enter the parameters of the code snippet (press Tab to cycle through all parameters). Press Enter to complete the snippet instance.

### propdb1 ###
Adds a one-way data-bindable property to a Viewmodel. You can specify the property type and name.
### propdbcol ###
Adds a one-way data-bindable collection property, a corresponding `(name)HasItems` property and an `Update(name)HasItems()` method to a Viewmodel. You can specify the generic collection type (e.g. `ObservableCollection` or `List`), the collection element type and the property name.

If you want specific UI elements in your view to only be visible when the collection has elements (e.g. a list of error messages), you can use the HasItems property to bind to the visibility of those UI elements. In that case, you should also call the UpdateHasItems() method after you have added or removed items (this is necessary even if this is an ObservableCollection).

**Note** that if you suffix a collection property name with **"List"**, you can benefit from the [list data binding naming convention in Android](#list-itemssource).

### propdb2 ###
Adds a two-way data-bindable property to a Viewmodel. You can specify the property type and name.
### propdb2c ###
Adds a two-way data-bindable property and a `On(name)Changed()` method for custom setter code to a Viewmodel. You can specify the property type and name.
### cmd ###
Adds a data-bindable command to a Viewmodel. You can specify the command name, which will be suffixed with **"Command"**.
### cmdp ###
Adds a data-bindable command with a parameter to a Viewmodel. You can specify the command name, which will be suffixed with **"Command"**, the parameter type and the parameter name.

## Android ##
Below is an overview of using MvvmQuickCross with Xamarin.Android. For a more detailed example, see the Android version of the [CloudAuction example application](http://github.com/MacawNL/MvvmQuickCross/tree/master/Examples/CloudAuction) in this repository.

### Create an Android App ###
Here is how to create an Android Twitter app that demonstrates simple data binding:
> Note that the complete source for this example is available in this repository, [here](http://github.com/MacawNL/MvvmQuickCross/tree/master/Examples/Twitter).

1.  Create a working Android app by following steps 1 though 4 of [Getting Started](#getting-started) above - choose an `Android Application` project named "Twitter" and an `Android Class Library` project named "Twitter.Shared".

2.  Remove the `Activity1.cs`, `Resources\Layout\Main.axml` and `Class1.cs` files that were included by the Xamarin.Android project templates

3.  Now you can run the app on your device and test the example MainView that was generated by the Install-Mvvm command.

4.  Create a `Models` folder in your Twitter.Shared project and create this `Tweet.cs` class in it:

	```csharp
	using System;
	namespace Twitter.Shared.Models
	{
	    public class Tweet
	    {
	        public string Text { get; set; }
	        public string UserName { get; set; }
	        public DateTime CreatedAt { get; set; }
	    }
	}
	```

5.  In `ViewModels\MainViewModel.cs` in your library project, in the `#region Data-bindable properties and commands`, remove the example property and command add these properties and commands with the indicated [code snippets](#code-snippets):
	
    <table>
        <tr>
            <td><b>Snippet</b></td><td><b>Parameters</b></td><td><b>Generated code</b></td>
        </tr>
        <tr>
            <td><a href="#propdb2c">propdb2c</a></td><td>Tweet Tweet</td><td>
				public Tweet Tweet { ... }<br />
				private void OnTweetChanged() { ... }
            </td>
        </tr>
        <tr>
            <td><a href="#propdbcol">propdbcol</a></td><td>ObservableCollection Tweet TweetList</td><td>
				public ObservableCollection<Tweet> TweetList  { ... }<br />
				public bool TweetListHasItems { ... }<br />
				protected void UpdateTweetListHasItems() { ... }
            </td>
        </tr>
        <tr>
            <td><a href="#cmd">cmd</a></td><td>Delete</td><td>
				public RelayCommand DeleteCommand { ... }<br />
				private void Delete() { ... }
            </td>
        </tr>
        <tr>
            <td><a href="#propdb2c">propdb2c</a></td><td>string Text</td><td>
				public string Text { ... }<br />
				private void OnTextChanged() { ... }
            </td>
        </tr>
        <tr>
            <td><a href="#propdb">propdb</a></td><td>int CharactersLeft</td><td>
				public int CharactersLeft { ... }
            </td>
        </tr>
        <tr>
            <td><a href="#cmd">cmd</a></td><td>Send</td><td>
				public RelayCommand SendCommand { ... }<br />
				private void Send() { ... }
           </td>
        </tr>
    </table>
    	
6.  Add this code in the generated `MainModel` methods:
		
	```csharp
    public MainViewModel()
    {
        TweetList = new ObservableCollection<Tweet>();
        OnTweetChanged();
        Text = "";
    }
	
    private void OnTweetChanged()
    {
        DeleteCommand.IsEnabled = (Tweet != null);
    }

    private void Delete()
    {
        TweetList.Remove(Tweet);
        Tweet = null;
    }

    private void OnTextChanged()
    {
        CharactersLeft = 140 - Text.Length;
        SendCommand.IsEnabled = (Text.Length > 0 && CharactersLeft >= 0);
    }

    private void Send()
    {
        var newTweet = new Tweet { Text = this.Text, CreatedAt = DateTime.Now, UserName="Me" };
        TweetList.Insert(0, newTweet);
        Tweet = newTweet;
        Text = "";
    }
	```

7.  The `MainViewModel.cs` file also contains a `MainViewModelDesign` class where you can put some hardcoded viewmodel data. Put this code in the MainViewModelDesign constructor:
		
	```csharp
    public MainViewModelDesign()
    {
        Text = "Text for a new tweet";
        var now = DateTime.Now;
        TweetList.Insert(0, new Tweet { 
			Text = "Creating a simple Twitter app for Android with MvvmQuickCross", 
			UserName = "Me", CreatedAt = now.AddSeconds(-115) });
        TweetList.Insert(0, new Tweet { 
			Text = "Created an Android solution with an application and a library project", 
			UserName = "Me", CreatedAt = now.AddSeconds(-63) });
        TweetList.Insert(0, new Tweet { 
			Text = "Added Tweet model class", 
			UserName = "Me", CreatedAt = now.AddSeconds(-45) });
        TweetList.Insert(0, new Tweet { 
			Text = "Added viewmodel properties and commands with code snippets", 
			UserName = "Me", CreatedAt = now.AddSeconds(-25) });
        TweetList.Insert(0, new Tweet { 
			Text = "Added some hardcoded design data fot the viewmodel", 
			UserName = "Me", CreatedAt = now.AddSeconds(-1) });
    }
	```

8.  In the application project, in the file `Resources\Layout\MainView.axml`, replace the existing markup with this:

	```xml
	<?xml version="1.0" encoding="utf-8"?>
	<LinearLayout xmlns:android="http://schemas.android.com/apk/res/android"
	    android:id="@+id/MainView"
	    android:orientation="vertical"
	    android:layout_width="fill_parent"
	    android:layout_height="fill_parent">
	    <ListView
	        android:layout_width="fill_parent"
	        android:layout_height="0dp"
	        android:layout_weight="1"
	        android:cacheColorHint="#FFDAFF7F"
	        android:choiceMode="singleChoice"
	        android:id="@+id/MainView_Tweet"
	        android:tag="{Binding Mode=TwoWay}" />
	    <EditText
	        android:id="@+id/MainView_Text"
	        android:tag="{Binding Mode=TwoWay}"
	        android:text="*"
	        android:textAppearance="?android:attr/textAppearanceLarge"
	        android:layout_width="fill_parent"
	        android:layout_height="wrap_content" />
	    <LinearLayout
	        android:orientation="horizontal"
	        android:layout_width="fill_parent"
	        android:layout_height="wrap_content">
	        <TextView
	            android:text="Characters left: "
	            android:textAppearance="?android:attr/textAppearanceSmall"
	            android:layout_width="wrap_content"
	            android:layout_height="wrap_content" />
	        <TextView
	            android:id="@+id/MainView_CharactersLeft"
	            android:text="14*"
	            android:textAppearance="?android:attr/textAppearanceSmall"
	            android:layout_width="0dp"
	            android:layout_weight="1"
	            android:layout_height="wrap_content" />
	        <Button
	            android:id="@+id/MainView_SendCommand"
	            android:layout_width="wrap_content"
	            android:layout_height="wrap_content"
	            android:text="Send" />
	        <Button
	            android:id="@+id/MainView_DeleteCommand"
	            android:layout_width="wrap_content"
	            android:layout_height="wrap_content"
	            android:layout_gravity="right"
	            android:text="Delete" />
	    </LinearLayout>
	</LinearLayout>
	```
8.  Add a new Android Layout named `Resources\Layout\TweetListItem.axml`, with this markup:

	```xml
	<?xml version="1.0" encoding="utf-8"?>
	<mvvmquickcross.CheckableLinearLayout 
		xmlns:android="http://schemas.android.com/apk/res/android"
	    android:id="@+id/TweetListItem"
	    android:orientation="vertical"
	    android:background="@drawable/CustomSelector"
	    android:addStatesFromChildren="true"
	    android:layout_width="fill_parent"
	    android:layout_height="wrap_content">
	    <LinearLayout
	        android:orientation="horizontal"
	        android:layout_width="fill_parent"
	        android:layout_height="wrap_content">
	        <TextView
	            android:id="@+id/TweetListItem_UserName"
	            android:text="Me*"
	            android:textAppearance="?android:attr/textAppearanceSmall"
	            android:layout_width="40dp"
	            android:layout_height="wrap_content" />
	        <TextView
	            android:id="@+id/TweetListItem_CreatedAt"
	            android:text="friday october 11, 2013 14:08:2*"
	            android:textAppearance="?android:attr/textAppearanceSmall"
	            android:gravity="right"
	            android:layout_width="0dp"
	            android:layout_weight="1"
	            android:layout_height="wrap_content" />
	    </LinearLayout>
	    <TextView
	        android:id="@+id/TweetListItem_Text"
	        android:text="Some example text*"
	        android:textAppearance="?android:attr/textAppearanceMedium"
	        android:layout_width="wrap_content"
	        android:layout_height="wrap_content" />
	</mvvmquickcross.CheckableLinearLayout>
	```
    Note that the CheckableLinearLayout view is a simple extension of the standard LinearLayout view that implements the ICheckable to better support highlighting checked list items; this view does not add anything specific for data-binding. If you dont care about highlighting checked items, you can use the standard LinearLayout (or any other layout view) for data binding as well.

9.  Add a new XML File named `Resources\Drawable\CustomSelector.xml`, with this markup:

	```xml
	<?xml version="1.0" encoding="utf-8"?>
	<selector xmlns:android="http://schemas.android.com/apk/res/android">
	  <item android:state_checked="true" android:drawable="@color/cellchecked" />
	  <item android:drawable="@color/cellback" />
	</selector>
	```

10. Add a new XML File named `Resources\Values\Colors.xml`, with this markup:

	```xml
	<?xml version="1.0" encoding="utf-8"?>
	<resources>
	  <color name="cellback">#00000000</color>
	  <color name="cellchecked">#FF0000FF</color>
	</resources>
	```

11. Run the app and test the MainView. Notice how the Send and Delete buttons are enabled and disabled based on the text length and selected item state, and how the characters remaining count is updated as you type. Also note how the selected list item is highlighted both from the UI when tapped, and from code when adding a new tweet.

You have created a working app with MvvmQuickCross. Note that the only code that you needed to write is in the viewmodel; no Android view or list adapter code is needed. To get data binding working, the markup follows some naming conventions and specifies some binding parameters in the Tag. 

### Android Data Binding ###
An Android data binding is a one-on-one binding between an Android view, such as a TextBox or Button, and a viewmodel property or command. You can specify bindings with (a combination of):

1. Code in the activity or fragment that creates the containing view
2. Id naming convention in the view markup
3. Tag binding parameters in the view markup

#### Android Id Naming Convention ####
To bind a view to a viewmodel property without using code, name the view `id` like this:

```xml
android:id="@+id/<activity-or-fragment-class-name>_<viewmodel-property-or-command-name>"
```
E.g, the MainView in the Twitter example above is created by this class:

```csharp
public class MainView : ActivityViewBase<MainViewModel> { ... }
```
And in the markup this is how a child view is bound to the CharactersLeft property on the view model:

```xml
<TextView android:id="@+id/MainView_CharactersLeft"	... />
```
Note that instead of using this id naming convention, you can specify the view in code. You can also change the default name prefix.

#### Android Binding Parameters in Tag ####
These are the binding parameters that you can specify in the view tag (linebreaks added for readability):

```xml
<ViewType android:tag="...
 {Binding propertyName, Mode=OneWay|TwoWay|Command} 
 {CommandParameter ListId=<view-Id>} 
 {List ItemsSource=listPropertyName, ItemIsValue=false|true, 
       ItemTemplate=listItemTemplateName, ItemValueId=listItemValueId}
 ..." />
```

All of these parameters are optional. You can also put any additional text outside the { } in the tag if you want to. Note that you can also specify binding parameters through code instead of in the tag attribute.

##### Binding propertyName #####
Is known by default from the naming convention for the view `id` = &lt;rootview prefix&gt;&lt;propertyName&gt;; the default for the rootview prefix is the rootview class name + "_". Note that viewmodel commands are just a special type of viewmodel property, so you can use the propertyName to specify a command name as well.

##### Binding Mode #####
Is `OneWay` by default. The mode specifies:

- OneWay data binding where the viewmodel property updates the view - e.g. a display-only TextView. The bound property can be generated with the propdb1 or propdbcol code snippet.
- TwoWay data binding where the viewmodel property updates the view and vice versa, e.g. an editable EditText. The bound property can be generated with the propdb2, propdb2c or propdbcol code snippet.
- Command binding (e.g. a Button). The bound command can be generated with the cmd or cmdp code snippet.

Note that you can also use the binding mode Command in a view that derives from AdapterView (ListView, Spinner etc). When an item in the list is selected or checked, the bound command is invoked with the selected item as the command parameter. E.g. in below markup, selecting an item in the list navigates to a detail view of the item:

```xml
<ListView
    android:id="@+id/ProductsView_ProductList"
    android:tag="{Binding SelectProductCommand, Mode=Command} {List ItemsSource=ProductList}"
	... />
```

##### CommandParameter ListId #####
Passes the selected item of the specified adapter view as the command parameter. The specified view can be any view type that is derived from AdapterView (ListView, Spinner etc). E.g. this Remove button passes the selected item from the view with id=SampleItemListView_Items as the command parameter, when the button is tapped:

```xml
<ListView
    android:id="@+id/SampleItemListView_Items"
    android:tag="{List ItemsSource=Items, ItemTemplate=ListItem}"
    android:choiceMode="singleChoice"
    android:listSelector="@android:color/holo_blue_dark"
	... />
<Button
    android:id="@+id/SampleItemListView_RemoveItemCommand"
    android:text="Remove"
    android:tag="{Binding Mode=Command} {CommandParameter ListId=SampleItemListView_Items}"
	... />
```

The passed command parameter is the selected item object from the bound list, e.g. see the corresponding viewmodel code:

```csharp
public ObservableCollection<SampleItem> Items { ... }
public RelayCommand RemoveItemCommand { ... }

private void RemoveItem(object parameter)
{
    var item = (SampleItem)parameter;
	...
}

```

The `List` binding parameters are for use with views derived from `AdapterView` (`ListView`, `Spinner` etc):

##### List ItemsSource #####
Specifies the name of the viewmodel collection property that contains the list items. The property must implement the standard .NET `IList` interface. If the property also implements the standard .NET `INotifyCollectionChanged` interface (e.g an `ObservableCollection`), the view will automatically reflect added, replaced or removed items. The default value of ItemsSource is `propertyName` + "**List**".

The items in an ItemsSource viewmodel collection property can be:

- An object with fields or properties (e.g. a POCO model object)
- An 'ValueItem' object, meaning an object that implements `ToString()` to present the value of the entire object as a human-readable text
- A viewmodel object that has data-bindable properties and/or commands. This is also called **composite viewmodels**, which makes it possible to e.g. automatically display changes of individual fields within existing list item objects.

##### List ItemTemplate #####
Specifies the name of the Android layout that represents a list item. E.g. the value "TweetListItem" corresponds to the view markup in the file Resources\Layout\TweetListItem.axml. The default value of ItemTemplate is the value of `ItemsSource` + "**Item**".

##### List ItemIsValue #####
Is a boolean flag indicating whether the list item should be displayed as a single text string, by calling the `ToString()` method on the object. If this flag is set to `true`, the `ItemValueId` binding parameter is also used. The default for ItemIsValue is `false`.

##### List ItemValueId #####
If `ItemIsValue` is `true`, this parameter specifies the id of the child view within the item template view that should be used to display the object text. The default value of ItemValueId is the value of `ItemTemplate`. 

#### Android Binding Parameters in Code ####
As an alternative to using the Id naming convention and tag texts in markup, you can also specify bindings in code in an optional parameter of the **Initialize()** method. The Initialize method is implemented in the view base classes, and it is called in your view code from the `OnCreate()` (for an activity view) or `OnCreateView()` (for a fragment view) method. Here is an example of specifying binding parameters in code:

```csharp
var spinner = FindViewById<Android.Widget.Spinner>(Resource.Id.OrderView_DeliveryLocation);
spinner.Adapter = new DataBindableListAdapter<string>(LayoutInflater, 
    itemTemplateResourceId: Resource.Layout.TextListItem, 
    idPrefix:               "TextListItem_", 
    itemValueResourceId:    Resource.Id.TextListItem);

var bindingsParameters = new BindingParameters[] {
   new BindingParameters {
       Mode                                    = BindingMode.TwoWay,
       View                                    = spinner,
       PropertyName                            = OrderViewModel.PROPERTYNAME_DeliveryLocation,
       ListPropertyName                        = OrderViewModel.PROPERTYNAME_DeliveryLocationList,
       CommandParameterSelectedItemAdapterView = null
   }
};

Initialize(
	FindViewById(Resource.Id.OrderView),
	CloudAuctionApplication.Instance.OrderViewModel, 
	bindingsParameters, 
	idPrefix: "OrderView_"
);
```
You can specify some or all of the bindings, and in each binding some or all of the binding parameters. If you also specify parameters for a binding in a markup tag, those will override or supplement any parameters that you specify in code.

Note that you do not need to use the [view id naming convention](#android-id-naming-convention) for normal views, since you specify a view instance in the `View` parameter. You could create the view entirely in code and not even give it an id. If you do use the id naming convention, you can specify a custom id name prefix in the `idPrefix` parameter of the `Initialize()` method (for normal views) and the `DataBindableListAdapter()` constructor (for list item views).

Most binding parameters are specified in the Initialize call, while some List binding parameters are specified when constructing a data bindable list adapter for an AdapterView. When you use markup for data bindings and an AdapterView does not have an adapter assigned to it, a data bindable adapter is created and assigned to the AdapterView automatically.

Here is how the code binding parameters correspond to the tag binding parameters:
<table>
<tr><td colspan="2"><b>Code</b></td><td colspan="2"><b>Markup</b></td></tr>
<tr><td>BindingParameters</td><td>Mode</td><td>Binding</td><td><a href = "#binding-mode">Mode</a></td></tr>
<tr><td></td><td>PropertyName</td><td>Binding</td><td><a href = "#binding-propertyname">property name</a></td></tr>
<tr><td></td><td>CommandParameter<br />SelectedItemAdapterView</td><td>CommandParameter</td><td><a href = "#commandparameter-listid">ListId</a></td></tr>
<tr><td></td><td>ListPropertyName</td><td>List</td><td><a href = "#list-itemssource">ItemsSource</a></td></tr>
<tr><td>DataBindableListAdapter</td><td>itemTemplateResourceId</td><td>List</td><td><a href = "#list-itemtemplate">ItemTemplate</a></td></tr>
<tr><td></td><td>itemValueResourceId</td><td>List</td><td><a href = "#list-itemvalueid">ItemValueId</a></td></tr>
</table>

#### Customizing or Extending Android Data Binding ####
MvvmQuickCross allows you to simply modify or extend it with overrides in the view base classes, and in the ViewDataBindings class. You can also add more view base classes if you need to derive your views from other classes besides Activity and Fragment.

##### Customizing Data Binding in Android Views #####
By overriding the `OnPropertyChanged()` method in your view class, you can handle changes for specific properties yourself instead of with the standard data binding. E.g:

```csharp
// Example of how to handle specific viewmodel property changes in code instead of with (or in addition to) data binding:
protected override void OnPropertyChanged(string propertyName)
{
    switch (propertyName)
    {
        case OrderViewModel.PROPERTYNAME_DeliveryLocationListHasItems:
            var hasItems = ViewModel.GetPropertyValue<bool>(
							  OrderViewModel.PROPERTYNAME_DeliveryLocationListHasItems);
            var spinner = FindViewById<Android.Widget.Spinner>(
							  Resource.Id.OrderView_DeliveryLocation);
            spinner.Visibility = hasItems ? ViewStates.Visible : ViewStates.Invisible;
            break;
        default:
            base.OnPropertyChanged(propertyName); break;
    }
}
```

You can also override the `UpdateView()` method to change how the data binding mechanism sets a property value to a specific view (or modify multiple views based on the value etc.). E.g., an alternative to the `OnPropertyChanged()` code above is to add a containing `FrameLayout` around the spinner, data-bind the FrameLayout to the `DeliveryLocationListHasItems` property, and then override `UpdateView()` like this:

```xml
<FrameLayout
    android:id="@+id/OrderView_DeliveryLocationListHasItems"
    android:layout_width="fill_parent"
    android:layout_height="wrap_content">
    <Spinner
        android:layout_width="fill_parent"
        android:layout_height="wrap_content"
        android:id="@+id/OrderView_DeliveryLocation"
        android:tag="{Binding Mode=TwoWay} {List ItemIsValue=true, ItemTemplate=TextListItem}" />
</FrameLayout>
```

```csharp
// Example of how to handle specific viewmodel property changes in code instead of with (or in addition to) data binding:
public override void UpdateView(Android.Views.View view, object value)
{
    switch (view.Id)
    {
        case Resource.Id.OrderView_DeliveryLocationListHasItems:
            view.Visibility = (bool)value ? ViewStates.Visible : ViewStates.Invisible; break;
        default:
            base.UpdateView(view, value); break;
    }
}
```
> Note that `UpdateView()` is also called for **data bindings in all list items** for all data-bound lists in your view. This makes it possible to customize data binding within list items with code in a normal view, instead of writing a custom data bindable adapter to put that customization code in.

Finally, you can react to changes in lists that implement INotifyCollectionChanged (e.g. ObservableCollections) by overriding `OnCollectionChanged()` in your view:

```csharp
public override void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
{
    ...
}
```
The sender parameter is the collection. You could use this e.g. if you want to do some animation when items are added or removed in a list.

##### Adding New Data-Bindable Android View Types #####
Out of the box MvvmQuickCross has default data binding support for these view types:

One-Way binding:

- `Android.Widget.ProgressBar`
- `Android.Webkit.WebView`
- `TextView` and derived types
- `AbsListView` and derived types
- `AdapterView` and derived types

Two-way binding:

- `AbsSpinner` and derived types
- `AbsListView` and derived types
- `EditText` and derived types

Command binding:

- `AbsSpinner` and derived types
- `AdapterView` and derived types
- `View` and derived types

To make more control types data bindable, you can simply add a case to the appropriate switch statements in the `MvvmQuickCross\ViewDataBindings.UI.cs` file in your application project, as indicated by the `// TODO: ` comments. E.g.:

```csharp
public static void UpdateView(View view, object value)
{
    if (view != null)
    {
        string viewTypeName = view.GetType().FullName;
        switch (viewTypeName)
        {
            // TODO: Add cases here for specialized view types, as needed
            case "Macaw.UIComponents.MultiImageView":
                {
                    if (value is Uri) value = ((Uri)value).AbsoluteUri;
                    var multiImageView = (Macaw.UIComponents.MultiImageView)view;
                    multiImageView.LoadImageList(value == null ? null : new[] { (string)value });
                }
                break;
			...
		}
	}
}
```
To prevent memory leaks, be sure that if you register an event handler on a view in `AddCommandHandler()` or `AddTwoWayHandler()`, you also unregister that handler in `RemoveCommandHandler()` resp. `RemoveTwoWayHandler()`.

##### Adding New Android View Base Classes #####
If you need to derive your views from other classes besides Activity and Fragment, you can simply copy the existing `MvvmQuickCross\ActivityViewBase.cs` or `MvvmQuickCross\FragmentViewBase.cs` class and change the class that it derives from to the one that you need. 

E.g. to support data bindable classes derived from ListActivity, you would copy ActivityViewBase.cs to ListActivityViewBase.cs and just change "Activity" to "ListActivity" in these lines:

```csharp
public abstract class ListActivityViewBase : ListActivity
{
	...
}

public class ListActivityViewBase<ViewModelType> :
			     ListActivityViewBase, 
                 ViewDataBindings.ViewExtensionPoints
                 where ViewModelType : ViewModelBase
{
	...
}
```
Now you can use the new base class in your view:

```csharp
[Activity(Label = "Order")]
public class OrderView : ListActivityViewBase<OrderViewModel>
{
	...
}
```
It should take no more than a minute. To complete this, you could also [add a custom view template](#customizing-and-extending-command-templates) for this new view type.

### Android View Lifecycle Support ###
As described by Xamarin, under "**3.1. Removing Event Handlers in Activities**" [here](http://docs.xamarin.com/guides/cross-platform/application_fundamentals/memory_perf_best_practices), you need to remove and re-add any external event handlers that you register in your view code to prevent memory leaks (external meaning not tied to the view itself or to an object contained within that view). The MvvmQuickCross view base classes do this removing and re-adding automatically at the appropriate [lifecycle events](http://docs.xamarin.com/guides/android/application_fundamentals/activity_lifecycle). To have your own event handlers added, removed and re-added at the appropriate times, you only need to override the `AddHandlers()` and `RemoveHandlers()` methods and add/remove your handlers there. E.g.:

```csharp
protected override void AddHandlers()
{
    base.AddHandlers();
    foreach (var tab in tabs) tab.TabSelected += Tab_TabSelected;
}

protected override void RemoveHandlers()
{
    foreach (var tab in tabs) tab.TabSelected -= Tab_TabSelected;
    base.RemoveHandlers();
}
```
Note: Be sure to always call the base class method as well!

You should not call `AddHandlers()` yourself - that would mess up the base class tracking of added handlers. `AddHandlers()` is initially called by the base class in the `Initialize()` method, which you call in your view code in the `OnCreate()` or `OnCreateView()` method.

### Android Helpers ###
In the `MvvmQuickCross\AndroidHelpers.cs` file in your application project, you will find a few simple helpers that are of general use in Android development. Noteable helpers are:

- `CurrentActivity` sometimes you need to code against the current activity from code that is not part of that activity. This static property is kept up to date by the Activity view base class, in such a way that no memory leaks can occur.
- `Wrapper<T>` sometimes you need to provide a `Java.Lang.Object` to a Xamarin.Android method or property, but what you actually have is a `System.Object`. This wrapper allows you to cast between any .NET Object and Java Object, e.g.:

	```csharp
	rootView.Tag = (Wrapper<ListDictionary>)viewHolder;
	...
	var viewHolder = (ListDictionary)rootView.Tag;
	```
	Note that the Tag property of an Android View in Xamarin is a `Java.Lang.Object`. 