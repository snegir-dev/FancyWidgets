using Autofac;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.ReactiveUI;
using FancyWidgets.Common.Constants;
using FancyWidgets.Common.Controls.WidgetDragger;
using FancyWidgets.Common.Convertors.Json;
using FancyWidgets.Common.Locators;
using FancyWidgets.Common.Systems;
using FancyWidgets.Common.WidgetAppConfigurations;
using FancyWidgets.Models;
using FancyWidgets.Views;
using ReactiveUI;

namespace FancyWidgets;

public abstract class Widget<TViewModel> : ReactiveWindow<TViewModel>
    where TViewModel : ReactiveObject
{
    private readonly IntPtr _windowHandler;
    private readonly IWidgetJsonProvider _widgetJsonProvider;
    public WidgetSettings? WidgetSettings;
    public readonly WidgetMetadata WidgetMetadata;
    private ContextMenuWindow _contextMenuWindow;
    private readonly WindowSystemManager _windowSystemManager;
    private int _currentCountStartCallingPositionChanges;
    private const int CountStartCallingPositionChanges = 2;
    public string? Uuid { get; private set; }

    protected Widget()
    {
        _windowHandler = TryGetPlatformHandle()!.Handle;
        var applicationOptions = WidgetLocator.Context.Resolve<WidgetApplicationOptions>();
        _windowSystemManager = new WindowSystemManager(_windowHandler);
        _widgetJsonProvider = WidgetLocator.Context.Resolve<IWidgetJsonProvider>();
        WidgetMetadata = _widgetJsonProvider.GetModel<WidgetMetadata>(AppSettings.WidgetMetadataFile)
                         ?? new WidgetMetadata();

        if (applicationOptions.IsDebug)
            this.AttachDevTools();
        else
            Uuid = WidgetMetadata.WidgetInfo.Uuid
                   ?? throw new NullReferenceException("Uuid must not be null.");
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        LoadWidgetDragger();
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

    protected virtual void LoadWidgetDragger()
    {
        if (VisualChildren[0] is Panel panel)
        {
            panel.Children.Add(new Border
            {
                Name = UiElementNames.DraggerContainer,
                Child = (UserControl)WidgetLocator.Context.Resolve<IWidgetDragger>(),
                IsVisible = false
            });
        }
    }

    protected virtual void LoadWidgetData()
    {
        WidgetSettings = _widgetJsonProvider.GetModel<WidgetSettings>(AppSettings.WidgetSettingsFile);
        if (WidgetSettings == null)
            return;
        Title = WidgetMetadata.WidgetInfo.WidgetName;
        Width = WidgetSettings.Width == 0 ? Width : WidgetSettings.Width;
        Height = WidgetSettings.Height == 0 ? Height : WidgetSettings.Height;
        if (WidgetSettings is not { XPosition: 0, YPosition: 0 })
            Position = new PixelPoint((int)WidgetSettings.XPosition, (int)WidgetSettings.YPosition);
    }

    protected virtual void LoadDefaultStyles()
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

    protected override void OnLoaded(RoutedEventArgs e)
    {
        _windowSystemManager.SetWindowChildToDesktop();
        base.OnLoaded(e);
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

    protected virtual void SaveLayoutSize()
    {
        _widgetJsonProvider.UpdateModel<WidgetSettings>(widgetSettings =>
        {
            widgetSettings.Width = Width;
            widgetSettings.Height = Height;
        }, AppSettings.WidgetSettingsFile, true);
    }

    protected virtual void SavePosition()
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