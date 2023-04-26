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
