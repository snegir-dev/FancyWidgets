using Autofac;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using FancyWidgets.Common.Locators;
using FancyWidgets.Common.Systems;
using FancyWidgets.ViewModels;

namespace FancyWidgets.Views;

public partial class SettingsWindow : ReactiveWindow<SettingsWindowViewModel>
{
    public SettingsWindow()
    {
        var handle = TryGetPlatformHandle()!.Handle;
        InitializeComponent(true);
        InitializeComponent();
        new WindowSystemManager(handle).HideMinimizeAndMaximizeButtons();
    }

    private void InitializeComponent()
    {
        using (var scope = WidgetLocator.Context.BeginLifetimeScope())
            ViewModel = scope.Resolve<SettingsWindowViewModel>();

        CreateInputControl();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void CreateInputControl()
    {
        foreach (var settingsControl in ViewModel!.SettingsControls)
        {
            var border = new Border
            {
                Child = (Control)settingsControl,
                Margin = new Thickness(5, 5)
            };
            StackPanelInputControl.Children.Add(border);
        }
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        StopRendering();
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }
}