# WinUI 3

Snippets related to WinUI 3 go here.

## Fix Caption Buttons with Custom Titlebar

```xml
<Application
    x:Class="Snippets.App"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:local="using:Snippets">
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <XamlControlsResources xmlns="using:Microsoft.UI.Xaml.Controls" />
                <!--  Other merged dictionaries here  -->
            </ResourceDictionary.MergedDictionaries>
          
            <!-- Fixes Caption Buttons when ExtendsContentIntoTitlebar = true and SetTitleBar() has been called. -->
            <SolidColorBrush x:Key="WindowCaptionBackground">Transparent</SolidColorBrush>
            <SolidColorBrush x:Key="WindowCaptionBackgroundDisabled">Transparent</SolidColorBrush>
        </ResourceDictionary>
    </Application.Resources>
</Application>
```
