using FancyWidgets.Common.Controls.ContextMenuElements;
using ReactiveUI;
using Splat;

namespace FancyWidgets.ViewModels;

public class ContextMenuWindowViewModel : ReactiveObject
{
    private Widget _senderWidget;

    public Widget SenderWidget
    {
        get => _senderWidget;
        set => this.RaiseAndSetIfChanged(ref _senderWidget, value);
    }
    
    public IEnumerable<WidgetContextMenu> ContextMenuButtons { get; }

    public ContextMenuWindowViewModel()
    {
        ContextMenuButtons = Locator.Current.GetServices<WidgetContextMenu>();
    }
}