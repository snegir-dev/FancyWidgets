using FancyWidgets.Common.Controls.ContextMenuElements;
using FancyWidgets.Common.Controls.ContextMenuElements.Buttons;
using FancyWidgets.Common.SettingProvider;
using FancyWidgets.Common.SettingProvider.Interfaces;
using Splat;

namespace FancyWidgets;

public static class FancyDependency
{
    public static void RegisterDependency()
    {
        Locator.CurrentMutable.Register<WidgetContextMenu, DisableWidgetButton>();
        Locator.CurrentMutable.Register<WidgetContextMenu, ChangeWindowButton>();
        Locator.CurrentMutable.Register<WidgetContextMenu, ChangeStylesButton>();
        Locator.CurrentMutable.Register<ISettingProvider, SettingProvider>();
    }
}