using System;
using Autofac;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.ReactiveUI;
using FancyWidgets.Common.Convertors.Json;
using FancyWidgets.Common.Locators;
using FancyWidgets.Common.System;
using FancyWidgets.Models;
using FancyWidgets.Views;
using ReactiveUI;

namespace FancyWidgets;

public abstract class Widget<TViewModel> : ReactiveWindow<TViewModel>
    where TViewModel : ReactiveObject
{
    public string Uuid { get; private set; }

    private readonly IntPtr _windowHandler;
    private readonly IWidgetJsonProvider _widgetJsonProvider;
    public WidgetSettings? WidgetSettings;
    public readonly WidgetMetadata WidgetMetadata;
    private ContextMenuWindow _contextMenuWindow;
    private readonly WindowSystemManager _windowSystemManager;
    private int _currentCountStartCallingPositionChanges;

    private const int CountStartCallingPositionChanges = 2;

    protected Widget()
    {
        _windowHandler = TryGetPlatformHandle()!.Handle;
        _windowSystemManager = new WindowSystemManager(_windowHandler);
        _widgetJsonProvider = WidgetLocator.Current.Resolve<IWidgetJsonProvider>();
        WidgetMetadata = _widgetJsonProvider.GetModel<WidgetMetadata>(AppSettings.WidgetMetadataFile)
                         ?? new WidgetMetadata();
        Uuid = WidgetMetadata.Uuid ?? throw new NullReferenceException("Uuid must not be null.");
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        _contextMenuWindow = new ContextMenuWindow();
        _windowSystemManager.HideFromAltTab();
        _windowSystemManager.WidgetToBottom();
        _windowSystemManager.HideMinimizeAndMaximizeButtons();
        InitializeEvents();
        base.OnApplyTemplate(e);
    }

    protected override void OnInitialized()
    {
        LoadDefaultStyles();
        LoadWidgetData();
        base.OnInitialized();
    }

    private void InitializeEvents()
    {
        LayoutUpdated += OnLayoutUpdated;
        PositionChanged += OnPositionChanged;
        Initialized += OnStarted;
    }

    private void LoadWidgetData()
    {
        WidgetSettings = _widgetJsonProvider.GetModel<WidgetSettings>(AppSettings.WidgetSettingsFile);
        if (WidgetSettings == null)
            return;
        Title = WidgetMetadata.WidgetName;
        Width = WidgetSettings.Width == 0 ? Width : WidgetSettings.Width;
        Height = WidgetSettings.Height == 0 ? Height : WidgetSettings.Height;
        if (WidgetSettings is not { XPosition: 0, YPosition: 0 })
            Position = new PixelPoint((int)WidgetSettings.XPosition, (int)WidgetSettings.YPosition);
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

    protected override void OnLoaded()
    {
        _windowSystemManager.SetWindowChildToDesktop();
        base.OnLoaded();
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

    protected virtual void OnLayoutUpdated(object? sender, EventArgs e)
    {
        SaveLayoutSize();
    }

    protected virtual void OnPositionChanged(object? sender, EventArgs e)
    {
        SavePosition();
    }

    private void SaveLayoutSize()
    {
        _widgetJsonProvider.UpdateModel<WidgetSettings>(widgetSettings =>
        {
            widgetSettings.Width = Width;
            widgetSettings.Height = Height;
        }, AppSettings.WidgetSettingsFile, true);
    }

    private void SavePosition()
    {
        if (_currentCountStartCallingPositionChanges >= CountStartCallingPositionChanges)
        {
            _widgetJsonProvider.UpdateModel<WidgetSettings>(widgetSettings =>
            {
                widgetSettings.XPosition = Position.X;
                widgetSettings.YPosition = Position.Y;
            }, AppSettings.WidgetSettingsFile, true);
        }
        else
        {
            _currentCountStartCallingPositionChanges++;
        }
    }

    private void OnStarted(object? sender, EventArgs eventArgs)
    {
        _widgetJsonProvider.UpdateModel<WidgetSettings>(widgetSettings => { widgetSettings.IsEnable = true; },
            AppSettings.WidgetSettingsFile, true);
    }
}