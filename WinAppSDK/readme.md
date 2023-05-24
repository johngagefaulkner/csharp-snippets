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
using Microsoft.UI.Xaml.Windowing;

public MainWindow()
{
  this.InitializeComponent();
  
  this.AppWindow.Title = ""; // Perform actions directly on AppWindow
}
```
