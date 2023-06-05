using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Controls.Mixins;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using FancyWidgets.Common.SettingProvider.Models;
using FancyWidgets.ViewModels;
using ReactiveUI;
using WinApi.User32;

namespace FancyWidgets.Views;

public partial class SettingWindow : ReactiveWindow<StylesChangeWindowViewModel>
{
    public required List<SettingElement> SettingElements { get; init; } = new();

    public SettingWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        ViewModel = new StylesChangeWindowViewModel();
        DataContext = ViewModel;
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        ViewModel!.SettingElements = SettingElements;
        base.OnApplyTemplate(e);
    }
}