# Sample App - CustomTitleBar

This sample app displays how to create and implement a <kbd>Custom Titlebar</kbd> that displays the App Icon and App Title on the left-inset (like any WinForms, WPF, or Win32 app would) but then also includes an interactive Searchbox (AutoSuggestBox control) in the center of the Window and, finally, an interactive <kbd>PersonPicture</kbd> control that's right-aligned but with enough padding that it still leaves room for the control box (minimize, maximize, close) buttons to be properly displayed.

> April 26th, 2023
Unfortunately, as of writing, this requires a mixed-bag combination of:
- Some WinAppSDK-specific code to access the `AppWindow` for our `MainWindow` via HWND.
- Some WinUI 3 code that accesses the `Window` property of our `MainWindow`.
- Some leftover code that only currently exists because Microsoft has migrated only *half* of the workload from WinAppSDK to WinUI 3 and this leftover code serves as a bridge between the two methods. 
    - Ideally, Microsoft will complete this transition sooner rather than later (here's hoping for a fix in WinAppSDK v1.4's release) and I'll be able to update this sample removing ~55% of the code required to make this possible.

# Overview

To make this whole thing work the way you want (and expect) it to, we're required to edit (at least) four files:
- App.xaml
- App.xaml.cs
- MainWindow.xaml
- MainWindow.xaml.cs

Let's get started.

## MainWindow.xaml

```xml
<Grid x:Name="AppTitleBar" Background="{ThemeResource SystemControlBackgroundChromeMediumLowBrush}">
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="Auto"/>
        <ColumnDefinition Width="*"/>
        <ColumnDefinition Width="Auto"/>
    </Grid.ColumnDefinitions>
    <StackPanel Orientation="Horizontal" Margin="12,0">
        <Image Source="Assets/StoreLogo.png" Height="20"/>
        <TextBlock Text="My App" Margin="12,0" VerticalAlignment="Center"/>
    </StackPanel>
    <AutoSuggestBox Grid.Column="1" PlaceholderText="Search..." Margin="12,0"/>
    <PersonPicture Grid.Column="2" Initials="JD" Margin="12,0">
        <PersonPicture.Flyout>
            <MenuFlyout>
                <MenuFlyoutItem Text="Sign in with your Microsoft Account"/>
            </MenuFlyout>
        </PersonPicture.Flyout>
  </PersonPicture>
</Grid>
```
      
    </PersonPicture>
</Grid>
