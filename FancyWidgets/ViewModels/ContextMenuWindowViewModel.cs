using FancyWidgets.Common.Controls.ContextMenuElements;
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