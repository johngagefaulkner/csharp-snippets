# C# Snippets • Terminals
###### Updated: February 18, 2024
Snippets used to execute scripts from a file (like a batch file `*.bat` or PowerShell script `*.ps1`) or, more commonly, directly interacting with one of the supported shells/terminals.

## Currently Supported
- **Command Prompt (CMD):** `C:\WINDOWS\System32\cmd.exe`
- **Windows PowerShell:** `%SystemRoot%\system32\WindowsPowerShell\v1.0\powershell.exe`
- **PowerShell 7:** `%PROGRAMFILES%\PowerShell\7\pwsh.exe`

---

## Getting Started

### Adding the `Terminals` Snippets to your Project
> **Note:** The example(s) below are using the `PowerShell-Update.cs` file.

- Add a new class to your project and completely remove all the code that's automatically generated.
- Open [PowerShell-Update.cs](https://github.com/johngagefaulkner/csharp-snippets/blob/main/Terminals/PowerShell-Update.cs) in a new tab and copy the entire contents of the file.
- Paste the copied code into your newly-created class, hit "Save All" in Visual Studio, then build your project.
- From any area within your project, you now have the following functionality available:
    - **Supported Shells:** `CMD`, `Windows PowerShell`, and `PowerShell 7` 
    - **Supported Options:** Single-line commands, Script blocks (PowerShell) and script files (usually `.bat` or `.ps1` files.)
    - **Supported Execution:** Synchronous and Asynchronous execution supported for any/all combinations of shells & commands/scripts.
    - **Supported Parameters:**
        - **Command Prompt (CMD):**
            - **RunAsAdmin:** True/False (Default: False)
        - **PowerShell:**
            - **RunAsAdmin:** True/False (Default: False)
            - **ExecutionPolicy:** AllSigned, Bypass, Default, RemoteSigned, Restricted, Undefined, Unrestricted (Default: Default)
            - **PowerShellVersion:** Windows PowerShell or PowerShell 7

---

## Examples

### Synchronous Usage

> **Windows PowerShell:** Executing a basic command.

```csharp
using Snippets; // Add to the top of your file.

// Obtain a list of all files in your Downloads folder.
public static string GetFilesInDownloadsFolder()
{
    string downloadsPath = $"C:\\Users\\{Environment.UserName}\\Downloads\\";
    return Terminals.PowerShell.ExecuteCommand($"Get-ChildItem -Path '{downloadsPath}' | Select-Object Name");
}
```

> **Note:** In the following example, we're choosing to provide values for all parameters:
> - **Command:** `appxCommand`
> - **Run As Admin:** `true` (optional)
> - **Execution Policy:** `Bypass` (optional)
> - **PowerShell Version:** `WindowsPowerShell` (optional)


```csharp
using Snippets; // Add to the top of your file.

// Obtain a list of all AppxPackages installed on the machine, for all users, exported as JSON
public static string GetInstalledAppxPackages()
{
    string appxCommand = "Get-AppxPackage -AllUsers | Select-Object Name,Publisher,InstallLocation | ConvertTo-Json";
    string _appxPackages = Terminals.PowerShell.ExecuteCommand(appxCommand, true, Terminals.ExecutionPolicy.Bypass, Terminals.PSVersion.WindowsPowerShell);
    return _appxPackages.Trim();
}
```

### Async Usage
You can run a single command, or a full `.ps1` PowerShell Script, asynchronously. This can be useful when you're calling the method from your UI thread or if the targeted command is long-running.

```csharp
using Snippets.Terminals;

// Returns a list of all AppxPackages installed on the machine, for all users, exported as JSON
public static async Task<string> GetAppxPackagesAsync()
{
    string appxCommand = "Get-AppxPackage -AllUsers | Select-Object Name,Publisher,InstallLocation | ConvertTo-Json";
    string _appxPackages = await Terminals.PowerShell.ExecuteCommandAsync(appxCommand, true, Terminals.ExecutionPolicy.Bypass, Terminals.PSVersion.WindowsPowerShell);
    return _appxPackages.Trim();
}
```

### Invoking a `.ps1` Script
You can also launch a full-blown script (as a `.ps1` file) using this library.

```csharp
using Snippets.Terminals;

// Launches a Toast Notification on Windows 10
public static string LaunchToastNotification()
{
    string _scriptPath = @"C:\Users\Public\New-ToastNotification.ps1";
    string _result = Terminals.PowerShell.ExecutePS1(_scriptPath);
    return _result.Trim();
}
```
## Full Example

> **Note:** The following example is copied directly from a tested-and-working `WPF, .NET 6, C#` project using `Visual Studio 2022 Community Edition`.

### Specifying a Different Version of PowerShell
Let's say you wanted to run a command that's only available in PowerShell 7 ─ you can specify to run it with that version.

```csharp
using System;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Snippets.Terminals;

namespace SnippetsWPF
{
    /// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        // The event method that's automatically generated for "clicking" a button will NOT be 'async' - be sure to add it as seen below.
        private async void button1_click(object sender, RoutedEventArgs e)
        {
            string storageDisks = await DeviceInformation.Storage.GetStorageDisks();
            MessageBox.Show(storageDisks, "Storage Disks", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
    
    public class DeviceInformation
    {
        public class Storage
        {
            // Returns information (in JSON format) about all Physical Storage Disks connected to the current system.
            // Targets PowerShell 7 and runs asynchronously.
            public static async Task<string> GetStorageDisks()
            {
                // PowerShell script to obtain the information. I believe this uses CIM_Instance to obtain the data.
                string diskQuery = "Get-PhysicalDisk | Select-Object BusType,MediaType,FriendlyName,Model,Manufacturer,FirmwareVersion |ConvertTo-Json -EnumsAsStrings";

                // Run the command as an Administrator with ExecutionPolicy set to 'Bypass'
                string diskResult = await Terminals.PowerShell.ExecuteCommandAsync(diskQuery, true, Terminals.ExecutionPolicy.Bypass, Terminals.PSVersion.PowerShell7);

                // Return the result(s) from the script.
                return diskResult;
            }
        }
    }
}
```

### Example Output
The results from the previous code sample would've shown something like the example below in a MessageBox:

```json
[
  {
    "BusType": "NVMe",
    "MediaType": "SSD",
    "FriendlyName": "INTEL SSDPEKNW010T8",
    "Model": "INTEL SSDPEKNW010T8",
    "Manufacturer": null,
    "FirmwareVersion": "004C"
  },
  {
    "BusType": "NVMe",
    "MediaType": "SSD",
    "FriendlyName": "NVMe Samsung SSD 970",
    "Model": "Samsung SSD 970",
    "Manufacturer": "NVMe",
    "FirmwareVersion": "1B2QEXP7"
  }
]
```

---

# Advanced Usage

## Custom Profiles
Some more advanced users, or users who plan to run a large number of PowerShell commands/scripts, may find it tedious being required to define the `Execution Policy` and `PowerShell Version` with each and every command. As an alternative, you can define `Custom Profiles` which already have those variables/parameters/arguments defined resulting in much cleaner code. You can create as many Custom Profiles as you'd like and re-use them as often as necessary.

### Creating (and using) Custom Config Profiles

Creating a Custom Profile is a simple, straight-forward, process. In the example below, I'm creating a Custom Profile for PowerShell 7 targeting the following settings:
- **Executable Path:** `C:\Program Files\PowerShell\7\pwsh.exe`
- **Run as Administrator:** `True`
- **Execution Policy:** `Bypass`
- **PowerShell Version:** `PowerShell7`

**Example Code:**
```csharp
var ps7AdminProfile = Terminals.Config.CustomProfiles.PowerShellProfile.Create(@"C:\Program Files\PowerShell\7\pwsh.exe", true, Terminals.ExecutionPolicy.Bypass, Terminals.PSVersion.PowerShell7);
```

Creation of this profile enables us to now reduce code written to run a command/script using all the settings we defined above. For an easier comparison, we'll use the same "Storage Disks" code from the previous section. See the code example below:

```csharp

public static async Task<string> GetStorageDiskInformation()
{
	// Build the PowerShell command
	string diskQuery = "Get-PhysicalDisk | Select-Object BusType,MediaType,FriendlyName,Model,Manufacturer,FirmwareVersion |ConvertTo-Json -EnumsAsStrings";
	
	// Normal method of running a PowerShell Command
	string diskResult = await Terminals.PowerShell.ExecuteCommandAsync(diskQuery, true, Terminals.ExecutionPolicy.Bypass, Terminals.PSVersion.PowerShell7);

	// Shortened method using a Custom Profile with predefined settings
	string diskResult2 = await Terminals.PowerShell.ExecuteCommandAsync(diskQuery, ps7AdminProfile);	
}
```
