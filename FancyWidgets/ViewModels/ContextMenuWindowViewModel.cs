using Avalonia.Controls;
using FancyWidgets.Common.Controls.ContextMenuElements;
using FancyWidgets.Common.Domain;
using ReactiveUI;
using Splat;

namespace FancyWidgets.ViewModels;

public class ContextMenuWindowViewModel : ReactiveObject
{
    public IEnumerable<WidgetContextMenu> ContextMenuButtons { get; }

    public ContextMenuWindowViewModel(IEnumerable<WidgetContextMenu> widgetContextMenus)
    {
        ContextMenuButtons = widgetContextMenus;
    }
}