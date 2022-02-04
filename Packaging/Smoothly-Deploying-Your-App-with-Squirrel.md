# Smoothly Deploying Your App with Squirrel
- Originally Written by: [Kevin Bost](https://web.archive.org/web/20201111214658/https://intellitect.com/author/kevin-bost/)
- Originally Hosted Here: [Intellitect.com](https://web.archive.org/web/20201111214658/https://intellitect.com/deploying-app-squirrel/)
- Originally Written on: July 18, 2017

## Application Deployment Made Easy
Many .NET developers are familiar with ClickOnce, Microsoft's simple deployment solution for quickly packaging and deploying software that is easy for end-users to install. However, these conveniences come with limitations.

- There are only a couple automatic update situations. To show custom UI when an update is available, requires disabling the automatic updates and handling everything in code.
- The generated installer is generated with a fixed deployment location. The installer must download all of the application files.
- All of the application files must be downloaded individually. This can cause issues on flaky internet connections when retrying may be necessary.
- No support for machine-wide installs.
- Supporting both online and offline installs requires is difficult.
- No support for custom actions during install/uninstall.

Squirrel offers an alternative that provides similar end-user install experience and uses NuGet to package and deploy your application. It provides an elegant balance between developer control and automatic updating.

## Integrating with your project
Let's take a look at deploying a simple Hello World (C# WPF) application.

```xaml
<Window x:Class="HellowWorld.WPF.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        mc:Ignorable="d" Loaded="MainWindow_OnLoaded"
        Title="MainWindow" Height="350" Width="525">
    <StackPanel VerticalAlignment="Center" HorizontalAlignment="Center" TextElement.FontSize="20">
        <TextBlock x:Name="CurrentVersion" Text="Loading..."/>
        <TextBlock x:Name="NewVersion" />
    </StackPanel>
</Window>
```

First, install the Squirrel.Windows NuGet package: `Install-Package Squirrel.Windows`

Next, we need to add in the code to check for updates. Unlike ClickOnce, there is no magical bootstrapping of your application. You have to put in the code to do the update checks. Fortunately, the Squirrel library can do this with just a couple lines of code.

For this application, we will just do a single update check when the window loads.

```csharp
private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
{
    using (var updateManager = new UpdateManager(@"C:\SquirrelReleases"))
    {
        CurrentVersion.Text = $"Current version: {updateManager.CurrentlyInstalledVersion()}";
        var releaseEntry = await updateManager.UpdateApp();
        NewVersion.Text = $"Update Version: {releaseEntry?.Version.ToString() ?? "No update"}";
    }
}
```

In this example, I am using a local folder at the location for the updates. For most situations, this path will likely be a URL.

The handy `UpdateApp` method is doing quite a bit for us:

- Checks for any available updates
- Downloads any new releases that it finds
- Applies the releases (so the latest version starts the next time the app is launched)

If you would rather have more control over the update process, the UpdateManager has other methods that give you immense control over the entire process. The UpdateManager also has support for re-starting the application into the latest version.

## Creating your first Release
Now that the app is written we need to release it. Squirrel uses NuGet packages to deploy your application, so we need to bundle it as a NuGet package. For details on creating NuGet packages see this [blog post](https://web.archive.org/web/20201111214658/https://intellitect.com/publishing-a-nuget/). For this example, I have downloaded the latest (v4.1.0) NuGet CLI and placed it alongside my project file and am using the Package Manager Console to run commands.

Create a simple nuspec file:

```xml
<?xml version="1.0"?>
<package >
  <metadata>
    <id>HelloWorld</id>
    <version>1.0.0</version>
    <authors>Me</authors>
    <owners>Me</owners>
    <description>HelloWorld - Squirrel</description>
    <releaseNotes>Initial release.</releaseNotes>
    <copyright>Copyright ©  2017</copyright>
  </metadata>
  <files>
    <file src="bin\Release\*.*" target="lib\net45\" exclude="*.pdb;*.vshost.*"/>
  </files>
</package>
```

Most of the values function exactly the same as a NuGet package, but there are a few things to note because this is being distributed with Squirrel:

- The id must not contain spaces
- The id should not contain any periods (contrary to standard NuGet recommendations).
- The description will appear as the program name when viewed in the list of installed programs within the Control Panel.
- Place the application, and all of its dependencies, in the lib\net45 folder within the NuGet package (It does not matter whether your application is a .Net 4.5 application, this is just the folder that Squirrel uses).
- Confirm that the NuGet package does not contain any dependencies or target multiple platforms.

To create the NuGet package, run: `.\nuget pack nuget\HelloWorld.nuspec`. This will generate a NuGet package `HelloWorld.1.0.0.nupkg` in our project directory. Finally, we need to run the NuGet package through Squirrel to create a release:

`Squirrel --releasify HelloWorld.1.0.0.nupkg --releaseDir "C:\SquirrelReleases"`

> **Note:** If this process fails, you may need to restart Visual Studio to get the Package Manager Console to reload the tools.

The output directory now contains four files:

> ![Squirrel-Output-Directory](https://i.imgur.com/XWCbNvA.png)

The Setup.exe contains a full copy of the NuGet package. So distributing this one file will be enough to install the application. The name of the file is not significant, and can be renamed to whatever you like.

The RELEASES file contains the list of releases that are available to be installed. This is just a simple text file that contains SHA1 hash, filename, and file size of all of the packages.

`77A4810CCFFF6772E21E4C499C696D909A9CE932 HelloWorld-1.0.0-full.nupkg 1165627`

The `HelloWorld.1.0.0.nupkg` file will not actually get used for the initial install. However, it is needed for performing updates.

There is also a `Setup.msi` that gets created for [doing machine wide installs](https://github.com/Squirrel/Squirrel.Windows/blob/develop/docs/using/machine-wide-installs.md). It can be ignored (or [disabled](https://github.com/Squirrel/Squirrel.Windows/blob/develop/docs/using/machine-wide-installs.md)) if this is not something you need to support.

> **Disabling MSI Generation:** `Squirrel --releasify MyApp.1.0.0.nupkg --no-msi`

Running the `Setup.exe` file allows the application to get installed and launched.

> ![First-Run](https://i.imgur.com/nhZ89v0.png)

## Updating your Application
Now let's simulate an update to see Squirrel at work!

- Update the version number in the nuspec file to be `1.0.1`
- Run `.\nuget pack nuget\HelloWorld.nuspec` to generate a new NuGet package
- Create an updated Squirrel release by running `Squirrel –releasify HelloWorld.1.0.1.nupkg –releaseDir "C:\SquirrelReleases"`

The output directory now contains a few additional files.

> ![First-Update](https://i.imgur.com/n3t4Qtr.png)

The `Setup.exe` is updated to contain the latest full NuGet package. Anyone who runs it will get the latest version (in this case 1.0.1) of the application. If you need to keep old Setup.exe files around to install older versions quickly, be sure to rename your existing Setup.exe file before running the Squirrel releasify command.

The `RELEASES` file has been updated to contain additional entries for the new packages.

- `77A4810CCFFF6772E21E4C499C696D909A9CE932 HelloWorld-1.0.0-full.nupkg 1165627`
- `8759B8C13E089CBE1A28EA3D6B0886B84D5B52C9 HelloWorld-1.0.1-delta.nupkg 9234`
- `9360C6F390BB38C5FDCC2FF2A3A9EA56919867F8 HelloWorld-1.0.1-full.nupkg 1165626`

You will notice that, in addition to the `HelloWorld.1.0.1-full.nupkg`, there is also a `HelloWorld-1.0.1-delta.nupkg` package created. Rather than containing the full application, it merely contains the delta between the binary files. These are typically much smaller and quicker to deploy. You can control if your application wants to use delta updates or only full updates ([documentation](https://github.com/Squirrel/Squirrel.Windows/blob/develop/docs/using/update-process.md)) by using the `UpdateManager`.

The `UpdateApp` extension method that we used in this application always downloads the full update releases. Re-running the application verifies that an update has been applied.

> ![Update-Has-Been-Applied](https://i.imgur.com/yr7iDNl.png)

Re-launching the application one more time shows that we are now running the latest version.

> ![New-Version-Is-Running](https://i.imgur.com/6zEpG10.png)

Squirrel also comes with lots of little features (many to alleviate some of the pain points on ClickOnce). You can read the [complete documentation](https://github.com/Squirrel/Squirrel.Windows/tree/develop/docs/), but here are some of the highlights.

- You can enable a "Splash screen" if the installer takes too much time. It won't be displayed if the app installs quick enough; a testament to how fast Squirrel truly is ([docs](https://github.com/Squirrel/Squirrel.Windows/blob/develop/docs/using/loading-gif.md)).
- Simple GitHub integration for easily deploying your app from GitHub ([docs](https://github.com/Squirrel/Squirrel.Windows/blob/develop/docs/using/github.md)).
- The ability to run code triggered by installer events ([docs](https://github.com/Squirrel/Squirrel.Windows/blob/develop/docs/using/custom-squirrel-events.md)).

"[Squirrel: It's like ClickOnce but Works™](https://github.com/Squirrel/Squirrel.Windows/)" is a fitting slogan for this application. It only takes a short amount of time to integrate Squirrel into your Windows Desktop applications.
