using Autofac;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Controls.Mixins;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using FancyWidgets.Common.Locator;
using FancyWidgets.Common.SettingProvider.Models;
using FancyWidgets.Common.System;
using FancyWidgets.ViewModels;
using ReactiveUI;
using Splat;
using WinApi.User32;

namespace FancyWidgets.Views;

public partial class SettingsWindow : ReactiveWindow<SettingsWindowViewModel>
{
    private StackPanel _stackPanel;
    public List<SettingsElement> SettingElements { get; private set; }

    public SettingsWindow()
    {
        InitializeComponent();
        var windowSystemManager = new WindowSystemManager(TryGetPlatformHandle()!.Handle);
        windowSystemManager.HideMinimizeAndMaximizeButtons();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        _stackPanel = this.FindControl<StackPanel>("StackPanelInputControl")!;
        ViewModel = WidgetLocator.Current.Resolve<SettingsWindowViewModel>();
        DataContext = ViewModel;
        BindProperties();
        CreateInputControl();
    }
    
    private void BindProperties()
    {
        this.WhenActivated(_ =>
        {
            if (ViewModel == null)
                return;
            this.WhenAnyValue(x => x.SettingElements).BindTo(this, x => x.ViewModel!.SettingElements);
        });
    }

    private void CreateInputControl()
    {
        foreach (var settingsControl in ViewModel!.SettingsControls)
        {
            _stackPanel.Children.Add(settingsControl.Control);
        }
    }
}