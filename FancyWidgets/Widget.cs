using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Platform;
using FancyWidgets.Models;
using WinApi.User32;
using Window = Avalonia.Controls.Window;

namespace FancyWidgets;

public class Widget : Window
{
    private readonly IntPtr _windowHandler;
    private readonly JsonFileManager _jsonFileManager = new();
    private readonly WidgetSetting _widgetSettings;
    private readonly WidgetMetadata _widgetMetadata;

    protected Widget()
    {
        _windowHandler = PlatformImpl.Handle.Handle;
        _widgetSettings = _jsonFileManager.GetModelFromJson<WidgetSetting>(AppSettings.WidgetSettingsFile);
        _widgetMetadata = _jsonFileManager.GetModelFromJson<WidgetMetadata>(AppSettings.WidgetMetadataFile);
        LayoutUpdated += OnLayoutUpdated;
        PositionChanged += OnPositionChanged;
        Closed += OnClosed;
        Initialized += OnStarted;
    }

    protected sealed override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        HideFromAltTab();
        WidgetToBottom();
        LoadWidgetData();
        LoadDefaultStyles();
        base.OnApplyTemplate(e);
    }

    public void AllowToEdit()
    {
        SystemDecorations = SystemDecorations.Full;
    }

    public void DenyToEdit()
    {
        SystemDecorations = SystemDecorations.None;
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
        IsHitTestVisible = false;
        SystemDecorations = SystemDecorations.None;
        Topmost = false;
    }

    private void WidgetToBottom()
    {
        var handle = PlatformImpl.Handle.Handle;
        User32Methods.SetWindowPos(handle, 
            (IntPtr)HwndZOrder.HWND_BOTTOM, Position.X, Position.Y, (int)Width, (int)Height, WindowPositionFlags.SWP_NOACTIVATE);
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
        _jsonFileManager.UpdateModel<WidgetSetting>(widgetSettings =>
        {
            widgetSettings.XPosition = Position.X;
            widgetSettings.YPosition = Position.Y;
        }, AppSettings.WidgetSettingsFile);
    }

    private void OnStarted(object? sender, EventArgs eventArgs)
    {
        _jsonFileManager.UpdateModel<WidgetSetting>(widgetSettings =>
        {
            widgetSettings.IsEnable = true;
        }, AppSettings.WidgetSettingsFile);
    }

    private void OnClosed(object? sender, EventArgs eventArgs)
    {
        _jsonFileManager.UpdateModel<WidgetSetting>(widgetSettings =>
        {
            widgetSettings.IsEnable = false;
        }, AppSettings.WidgetSettingsFile);
    }
}