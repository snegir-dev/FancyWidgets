using FancyWidgets.Common.Controls.ContextMenuElements;
using FancyWidgets.Common.Controls.ContextMenuElements.Buttons;
using Splat;

namespace FancyWidgets;

public static class FancyDependency
{
    internal static void RegisterDependency()
    {
        Locator.CurrentMutable.Register<WidgetContextMenu, Disable>();
        Locator.CurrentMutable.Register<WidgetContextMenu, ChangeWindow>();
    }
}