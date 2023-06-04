using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Mixins;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using FancyWidgets.Common.SettingProvider.Models;
using FancyWidgets.ViewModels;
using ReactiveUI;

namespace FancyWidgets.Views;

public class SettingWindow : ReactiveWindow<StylesChangeWindowViewModel>
{
    public required List<SettingElement> SettingElements { get; init; } = new();

    public SettingWindow()
    {
        InitializeComponent();
        ViewModel = new StylesChangeWindowViewModel();
        DataContext = ViewModel;
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        ViewModel!.SettingElements = SettingElements;
        base.OnApplyTemplate(e);
    }
}