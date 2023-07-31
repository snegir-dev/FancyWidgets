using Autofac;
using Avalonia;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using FancyWidgets.Common.Locators;
using FancyWidgets.ViewModels;
using WinApi.User32;

namespace FancyWidgets.Views;

public partial class ContextMenuWindow : ReactiveWindow<ContextMenuWindowViewModel>
{
    public ContextMenuWindow()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        ViewModel = WidgetLocator.Context.Resolve<ContextMenuWindowViewModel>();
        PointerExited += Hide;
    }

    private void Hide(object? sender, PointerEventArgs e)
    {
        base.Hide();
    }

    public override void Show()
    {
        Topmost = true;
        User32Methods.GetCursorPos(out var point);
        Position =
            new PixelPoint(point.X - 5, (int)(point.Y - Height));
        base.Show();
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        Hide();
    }
}