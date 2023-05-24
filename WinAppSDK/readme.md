# WindowsAppSDK

Snippets related to the WindowsAppSDK go here.

## Version 1.3(+)

Starting with v1.3, the following features/functionality are available.

### Enable Mica, Mica (Alt) or Acrylic Window Backdrops

Version 1.3 dramatically reduced the amount of code required to enable custom backdrops on your windows.

```csharp
public MainWindow()
{
    this.InitializeComponent();

    this.SystemBackdrop = new MicaBackdrop(); // Mica
    this.SystemBackdrop = new MicaBackdrop({ MicaBackdrop.Style = SystemBackdropStyles.MicaAlt }); // Mica Alt
    this.SystemBackdrop = new AcrylicBackdrop(); // Acrylic
}
```

Version 1.3 also *dramatically* reduced the amount of boilerplate and duplicate code required to access and interact with the **AppWindow** for your app:

```csharp
// You're now able to use AppWindow APIs directly from an Window through Window.AppWindow. 
public static Microsoft.UI.Windowing.AppWindow m_AppWindow;

public MainWindow()
{
  this.InitializeComponent();
  
  this.AppWindow.Title = ""; // Perform actions directly on AppWindow
}
```

**AppWindow Information:**

- For the Windows App SDK version of Microsoft.UI.Windowing.AppWindow we're supporting only top-level HWNDs. There's a 1:1 mapping between an AppWindow and a top-level HWND.
- The lifetime of an AppWindow object and an HWND is the same—the AppWindow is available immediately after the window has been created; and it's destroyed when the window is closed.
- AppWindow is available only to desktop apps (both packaged and unpackaged); it's not available to UWP apps.
