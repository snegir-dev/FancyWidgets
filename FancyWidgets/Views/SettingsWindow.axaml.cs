using Autofac;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using FancyWidgets.Common.Locators;
using FancyWidgets.Common.SettingProvider.Models;
using FancyWidgets.Common.System;
using FancyWidgets.ViewModels;
using ReactiveUI;

namespace FancyWidgets.Views;

public partial class SettingsWindow : ReactiveWindow<SettingsWindowViewModel>
{
    private StackPanel _stackPanel;
    public List<SettingsElement> SettingElements { get; private set; }

    public SettingsWindow()
    {
        var handle = TryGetPlatformHandle()!.Handle;
        InitializeComponent();
        new WindowSystemManager(handle)
            .HideMinimizeAndMaximizeButtons();
        BindProperties();
        CreateInputControl();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        _stackPanel = this.FindControl<StackPanel>("StackPanelInputControl")!;
        ViewModel = WidgetLocator.Current.Resolve<SettingsWindowViewModel>();
        DataContext = ViewModel;
#if DEBUG
        this.AttachDevTools();
#endif
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
            var border = new Border()
            {
                Child = settingsControl.Control,
                Margin = new Thickness(5, 5)
            };
            _stackPanel.Children.Add(border);
        }
    }
}