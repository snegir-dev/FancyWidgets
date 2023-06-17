using FancyWidgets.Common.Controls.ContextMenuElements;
using ReactiveUI;

namespace FancyWidgets.ViewModels;

public class ContextMenuWindowViewModel : ReactiveObject
{
    public IEnumerable<WidgetContextMenu> ContextMenuButtons { get; }

    public ContextMenuWindowViewModel(IEnumerable<WidgetContextMenu> widgetContextMenus)
    {
        ContextMenuButtons = widgetContextMenus.Order();
    }
}