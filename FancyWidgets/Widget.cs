using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.ReactiveUI;
using FancyWidgets.Common.SettingProvider;
using FancyWidgets.Common.SettingProvider.Interfaces;
using FancyWidgets.Models;
using FancyWidgets.Views;
using ReactiveUI;
using Splat;
using WinApi.User32;

namespace FancyWidgets;

public abstract class Widget : ReactiveWindow<ReactiveObject>
{
    private readonly IntPtr _windowHandler;
    private readonly JsonFileManager _jsonFileManager = new();
    private readonly WidgetSetting _widgetSettings;
    private readonly WidgetMetadata _widgetMetadata;
    private const int CountStartCallingPositionChanges = 2;
    private int _currentCountStartCallingPositionChanges;
    protected readonly ContextMenuWindow ContextMenuWindow;
    public bool IsStyleRegenerate { get; set; }

    protected Widget()
    {
        FancyDependency.RegisterDependency();
        ContextMenuWindow = new ContextMenuWindow();
        ContextMenuWindow.SetSenderWidget(this);
        _windowHandler = PlatformImpl.Handle.Handle;
        _widgetSettings = _jsonFileManager.GetModelFromJson<WidgetSetting>(AppSettings.WidgetSettingsFile);
        _widgetMetadata = _jsonFileManager.GetModelFromJson<WidgetMetadata>(AppSettings.WidgetMetadataFile);
        LayoutUpdated += OnLayoutUpdated;
        PositionChanged += OnPositionChanged;
        Closed += OnClosed;
        Initialized += OnStarted;
        DataContextChanged += OnDataContextChanged;
    }

    protected sealed override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        HideFromAltTab();
        // WidgetToBottom();
        Topmost = true;
        LoadWidgetData();
        LoadDefaultStyles();
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
        TransparencyLevelHint = WindowTransparencyLevel.Transparent;
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

        // if (e.KeyModifiers != KeyModifiers.Control)
        //     return;
        
        ContextMenuWindow.Show();
        base.OnPointerPressed(e);
    }

    private void WidgetToBottom()
    {
        var handle = PlatformImpl.Handle.Handle;
        User32Methods.SetWindowPos(handle,
            (IntPtr)HwndZOrder.HWND_BOTTOM, Position.X, Position.Y, (int)Width, (int)Height,
            WindowPositionFlags.SWP_NOACTIVATE);
    }

    private void HideFromAltTab()
    {
        var currentStyle = User32Helpers.GetWindowLongPtr(_windowHandler, WindowLongFlags.GWL_EXSTYLE);
        User32Helpers.SetWindowLongPtr(_windowHandler, WindowLongFlags.GWL_EXSTYLE,
            currentStyle | (int)WindowExStyles.WS_EX_NOACTIVATE);
    }

    private void OnLayoutUpdated(object? sender, EventArgs e)
    {
        _jsonFileManager.UpdateModel<WidgetSetting>(widgetSettings =>
        {
            widgetSettings.Width = Width;
            widgetSettings.Height = Height;
        }, AppSettings.WidgetSettingsFile);
    }

    private void OnPositionChanged(object? sender, EventArgs e)
    {
        if (_currentCountStartCallingPositionChanges >= CountStartCallingPositionChanges)
        {
            _jsonFileManager.UpdateModel<WidgetSetting>(widgetSettings =>
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
        _jsonFileManager.UpdateModel<WidgetSetting>(widgetSettings => { widgetSettings.IsEnable = true; },
            AppSettings.WidgetSettingsFile);
    }

    private void OnClosed(object? sender, EventArgs eventArgs)
    {
        _jsonFileManager.UpdateModel<WidgetSetting>(widgetSettings => { widgetSettings.IsEnable = false; },
            AppSettings.WidgetSettingsFile);
    }
}