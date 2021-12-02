# C# Snippets • Terminals
Snippets for running, calling, or otherwise interacting with, **CMD** and/or **PowerShell** from .NET 5 and .NET 6.

---

## Basic Usage
- Example using the `PowerShell-Update.cs` file.
- Add a new class to your project and completely remove all the code that's automatically generated.
- Open [PowerShell-Update.cs](https://github.com/johngagefaulkner/csharp-snippets/blob/main/Terminals/PowerShell-Update.cs) and copy the entire contents of the file.
- Paste the code into your newly-created class, hit "Save All" in Visual Studio, then build your project.
- From any area within your project, you can run a PowerShell command using the following code:

```csharp
using Snippets.Terminals;

// Returns a list of all AppxPackages installed on the machine, for all users, exported as JSON
public static string GetInstalledAppxPackages()
{
  string appxResult = PowerShell.ExecuteCommand("Get-AppxPackage -AllUsers |Select-Object Name,Publisher,InstallLocation |ConvertTo-Json");
  return appxResult.Trim();
}
```

### Async Usage
You can run a single command, or a full `.ps1` PowerShell Script, asynchronously as to not freeze the thread you're calling from.

```csharp
using Snippets.Terminals;

// Returns a list of all AppxPackages installed on the machine, for all users, exported as JSON
public static async Task<string> GetAppxPackagesAsync()
{
  var _result = await PowerShell.ExecuteCommandAsync("Get-AppxPackage -AllUsers |Select-Object Name,Publisher,InstallLocation |ConvertTo-Json");
  return _result.Trim();
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
  var _result = PowerShell.ExecutePS1(_scriptPath);
  return _result.Trim();
}
```

### Specifying a Different Version of PowerShell
Let's say you wanted to run a command that's only available in PowerShell 7 ─ you can specify to run it with that version.

```csharp
using Snippets.Terminals;

// Returns a list of all AppxPackages installed on the machine, for all users, exported as JSON
public static string GetStorageDisks()
{
  string diskResult = PowerShell.ExecuteCommand("Get-PhysicalDisk |Select-Object * -ExcludeProperty 'CIM*' |ConvertTo-Json -EnumsAsStrings", PowerShellVersion.PowerShell7);
  return diskResult.Trim();
}
```
