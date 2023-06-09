using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.ReactiveUI;
using FancyWidgets.Common.SettingProvider;
using FancyWidgets.Common.System;
using FancyWidgets.Models;
using FancyWidgets.Views;
using ReactiveUI;
using WinApi.User32;
using Path = System.IO.Path;

namespace FancyWidgets;

public abstract class Widget : ReactiveWindow<ReactiveObject>
{
    private readonly IntPtr _windowHandler;
    private readonly WidgetJsonProvider _widgetJsonProvider = new();
    private readonly WidgetSetting _widgetSettings;
    private readonly WidgetMetadata _widgetMetadata;
    private const int CountStartCallingPositionChanges = 2;
    private int _currentCountStartCallingPositionChanges;
    protected readonly ContextMenuWindow ContextMenuWindow;
    private readonly WindowSystemManager _windowSystemManager;

    protected Widget()
    {
        WidgetApplication.CreateBuilder();
        ContextMenuWindow = new ContextMenuWindow();
        ContextMenuWindow.SetSenderWidget(this);
        _windowHandler = TryGetPlatformHandle()!.Handle;
        _windowSystemManager = new WindowSystemManager(_windowHandler);
        _widgetSettings = _widgetJsonProvider.GetModel<WidgetSetting>(AppSettings.WidgetSettingsFile);
        _widgetMetadata = _widgetJsonProvider.GetModel<WidgetMetadata>(AppSettings.WidgetMetadataFile);
        LayoutUpdated += OnLayoutUpdated;
        PositionChanged += OnPositionChanged;
        Closed += OnClosed;
        Initialized += OnStarted;
        DataContextChanged += OnDataContextChanged;
    }

    protected sealed override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        Topmost = true;
        LoadWidgetData();
        LoadDefaultStyles();
        _windowSystemManager.HideFromAltTab();
        _windowSystemManager.HideMinimizeAndMaximizeButtons();
        _windowSystemManager.WidgetToBottom(Position, (int)Width, (int)Height);
        base.OnApplyTemplate(e);
    }

    private void LoadWidgetData()
    {
        Title = _widgetMetadata.WidgetName;
        Width = _widgetSettings.Width == 0 ? Width : _widgetSettings.Width;
        Height = _widgetSettings.Height == 0 ? Height : _widgetSettings.Height;
        Position = new PixelPoint((int)_widgetSettings.XPosition, (int)_widgetSettings.YPosition);
    }

    private void LoadDefaultStyles()
    {
        TransparencyLevelHint = new[] { WindowTransparencyLevel.Transparent };
        Background = Brushes.Transparent;
        ExtendClientAreaToDecorationsHint = false;
        ExtendClientAreaChromeHints = ExtendClientAreaChromeHints.NoChrome;
        ExtendClientAreaTitleBarHeightHint = -1;
        ShowInTaskbar = false;
        SystemDecorations = SystemDecorations.None;
        Topmost = false;
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.PointerUpdateKind != PointerUpdateKind.RightButtonPressed)
            return;

        if (e.KeyModifiers != KeyModifiers.Control)
            return;

        ContextMenuWindow.Show();
        base.OnPointerPressed(e);
    }

    private void OnLayoutUpdated(object? sender, EventArgs e)
    {
        _widgetJsonProvider.UpdateModel<WidgetSetting>(widgetSettings =>
        {
            widgetSettings.Width = Width;
            widgetSettings.Height = Height;
        }, AppSettings.WidgetSettingsFile);
    }

    private void OnPositionChanged(object? sender, EventArgs e)
    {
        if (_currentCountStartCallingPositionChanges >= CountStartCallingPositionChanges)
        {
            _widgetJsonProvider.UpdateModel<WidgetSetting>(widgetSettings =>
            {
                widgetSettings.XPosition = Position.X;
                widgetSettings.YPosition = Position.Y;
            }, AppSettings.WidgetSettingsFile);
        }
        else
        {
            _currentCountStartCallingPositionChanges++;
        }
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext == null)
            return;
        var settingProvider = new SettingProvider(DataContext);
        settingProvider.LoadSettings();
    }

    private void OnStarted(object? sender, EventArgs eventArgs)
    {
        _widgetJsonProvider.UpdateModel<WidgetSetting>(widgetSettings => { widgetSettings.IsEnable = true; },
            AppSettings.WidgetSettingsFile);
    }

    private void OnClosed(object? sender, EventArgs eventArgs)
    {
        _widgetJsonProvider.UpdateModel<WidgetSetting>(widgetSettings => { widgetSettings.IsEnable = false; },
            AppSettings.WidgetSettingsFile);
    }
}