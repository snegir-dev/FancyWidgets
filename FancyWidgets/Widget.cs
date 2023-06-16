using Autofac;
using Avalonia.ReactiveUI;
using FancyWidgets.Common.Convertors.Json;
using FancyWidgets.Common.Locator;
using FancyWidgets.Common.System;
using FancyWidgets.Models;
using FancyWidgets.Views;
using ReactiveUI;

namespace FancyWidgets;

public abstract class Widget<TViewModel> : ReactiveWindow<TViewModel>
    where TViewModel : ReactiveObject
{
    public string Uuid { get; private set; }

    private readonly IWidgetJsonProvider _widgetJsonProvider;
    private readonly WidgetSettings _widgetSettings;
    private readonly WidgetMetadata _widgetMetadata;
    private  ContextMenuWindow _contextMenuWindow;
    private readonly WindowSystemManager _windowSystemManager;
    private int _currentCountStartCallingPositionChanges;
    
    private const int CountStartCallingPositionChanges = 2;

    protected Widget()
    {
        var windowHandler = TryGetPlatformHandle()!.Handle;
        _windowSystemManager = new WindowSystemManager(windowHandler);
        _widgetJsonProvider = WidgetLocator.Current.Resolve<IWidgetJsonProvider>();
        _widgetSettings = _widgetJsonProvider.GetModel<WidgetSettings>(AppSettings.WidgetSettingsFile);
        _widgetMetadata = _widgetJsonProvider.GetModel<WidgetMetadata>(AppSettings.WidgetMetadataFile);
        Uuid = _widgetMetadata.Uuid ?? throw new NullReferenceException("Uuid must not be null.");
        LayoutUpdated += OnLayoutUpdated;
        PositionChanged += OnPositionChanged;
        Initialized += OnStarted;
    }

    protected sealed override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        _contextMenuWindow = new ContextMenuWindow();
        _windowSystemManager.HideFromAltTab();
        _windowSystemManager.WidgetToBottom(Position, (int)Width, (int)Height);
        _windowSystemManager.HideMinimizeAndMaximizeButtons();
        LoadDefaultStyles();
        LoadWidgetData();
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
        SystemDecorations = SystemDecorations.Full;
        Topmost = false;
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.PointerUpdateKind != PointerUpdateKind.RightButtonPressed)
            return;

        if (e.KeyModifiers != KeyModifiers.Control)
            return;

        _contextMenuWindow.Show();
        base.OnPointerPressed(e);
    }

    private void OnLayoutUpdated(object? sender, EventArgs e)
    {
        _widgetJsonProvider.UpdateModel<WidgetSettings>(widgetSettings =>
        {
            widgetSettings.Width = Width;
            widgetSettings.Height = Height;
        }, AppSettings.WidgetSettingsFile);
    }

    private void OnPositionChanged(object? sender, EventArgs e)
    {
        if (_currentCountStartCallingPositionChanges >= CountStartCallingPositionChanges)
        {
            _widgetJsonProvider.UpdateModel<WidgetSettings>(widgetSettings =>
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

    private void OnStarted(object? sender, EventArgs eventArgs)
    {
        _widgetJsonProvider.UpdateModel<WidgetSettings>(widgetSettings => { widgetSettings.IsEnable = true; },
            AppSettings.WidgetSettingsFile);
    }
}