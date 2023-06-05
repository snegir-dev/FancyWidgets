using Avalonia;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using FancyWidgets.ViewModels;
using WinApi.User32;

namespace FancyWidgets.Views;

public partial class ContextMenuWindow : ReactiveWindow<ContextMenuWindowViewModel>
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
        PointerExited += (_, _) => Hide();
    }

    public void SetSenderWidget(Widget widget)
    {
        Widget = widget;
        if (ViewModel != null) ViewModel.SenderWidget = Widget;
    }

    public override void Show()
    {
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