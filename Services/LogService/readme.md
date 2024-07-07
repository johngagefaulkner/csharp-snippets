### Snippets // Services
# LogService

This folder contains two different approaches to logging. Currently, I'm using my custom logging implementation in a WinUI 3 Packaged Desktop App targeting `.NET 8`, `Windows 11 Build 10.0.22621.0` and the `Windows App SDK v1.5`.

The first is my own custom logger that's intended to use your project's root namespace so that it can be universally called by simply `Log`.
- The Logger is a static singleton instance that does not need to be manually initialized (it initializes itself.)
- Each log entry is automatically prepended with a sortable timestamp, log severity and caller info.
- The log file is automatically created if it doesn't exist at `C:\Users\%USERNAME%\AppData\Local\$AppName\Logs\$Date_Main.log`

For example:
```cs
try
{
  // Log some information
  Log.Info("Successfully performed task!");
}

catch (Exception ex)
{
  // Log an error message with exception, StackTrace, and caller information included automatically
  // Pass the thrown Exception object through as a parameter and, optionally, include a custom message string to provide context.
  Log.Error(ex, "Failed to perform task!");
}
```

---

`MicrosoftLogger.cs`─The second implementation─is an adaptation from a Microsoft Docs article which is similar, but not identical, to my (much more opinionated) implementation.
- This one does have to be initialized before use.
- By default, the log file is saved to `string logFilePath = @$"C:\Users\{Environment.GetEnvironmentVariable("%username%")}\AppData\Local\{AppInfo.Current.DisplayInfo.DisplayName}\Logs\{Assembly.Version}\Log_{DateTime.Now.ToString("yyyy-MM-dd")}.log";`

**Initialization Example:**
```cs
public App()
{
    this.UnhandledException += App_UnhandledException;
    this.InitializeComponent();

    // Initialize Logger
    bool isLoggerInitialized = Snippets.Services.LogService.MicrosoftLogger.InitializeLogger(@"\Logs\", false);
}
```

