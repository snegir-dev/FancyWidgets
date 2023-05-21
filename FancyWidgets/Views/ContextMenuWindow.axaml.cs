using Avalonia;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using FancyWidgets.ViewModels;
using WinApi.User32;

namespace FancyWidgets.Views;

public class ContextMenuWindow : ReactiveWindow<ContextMenuWindowViewModel>
{
    public Widget Widget { get; set; }

    public ContextMenuWindow()
    {
        InitializeComponent();
        ViewModel = new ContextMenuWindowViewModel();
        DataContext = ViewModel;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        Opened += OnOpened;
        PointerLeave += (_, _) => Hide();
    }

    public void SetSenderWidget(Widget widget)
    {
        Widget = widget;
        if (ViewModel != null) ViewModel.SenderWidget = Widget;
    }

    private void OnOpened(object? sender, EventArgs eventArgs)
    {
        User32Methods.GetCursorPos(out var point);
        Position =
            new PixelPoint(point.X + 10, (int)(point.Y - Height));
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        Hide();
    }
}