using FancyWidgets.Common.Controls.WidgetContextMenu;
using ReactiveUI;

namespace FancyWidgets.ViewModels;

public class ContextMenuWindowViewModel : ReactiveObject
{
    public IEnumerable<WidgetContextMenuButton> ContextMenuButtons { get; }

    public ContextMenuWindowViewModel(IEnumerable<WidgetContextMenuButton> widgetContextMenus)
    {
        ContextMenuButtons = widgetContextMenus.Order();
    }
}