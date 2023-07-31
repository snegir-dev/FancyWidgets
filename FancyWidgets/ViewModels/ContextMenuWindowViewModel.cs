using FancyWidgets.Common.Controls.WidgetContextMenu;
using ReactiveUI;

namespace FancyWidgets.ViewModels;

public class ContextMenuWindowViewModel : ReactiveObject
{
    public ContextMenuWindowViewModel(IEnumerable<WidgetContextMenuButton> widgetContextMenus)
    {
        ContextMenuButtons = widgetContextMenus.Order();
    }

    public IEnumerable<WidgetContextMenuButton> ContextMenuButtons { get; }
}