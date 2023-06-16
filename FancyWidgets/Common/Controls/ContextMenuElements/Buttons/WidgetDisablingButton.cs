using FancyWidgets.Common.Convertors.Json;
using FancyWidgets.Common.Extensions;
using FancyWidgets.Common.Locator;
using FancyWidgets.Models;
using Splat;

namespace FancyWidgets.Common.Controls.ContextMenuElements.Buttons;

public class WidgetDisablingButton : WidgetContextMenu
{
    private readonly Window _window;
    private readonly IWidgetJsonProvider _widgetJsonProvider;
    public override string Content { get; set; } = "Disable";

    public WidgetDisablingButton(IWidgetJsonProvider widgetJsonProvider)
    {
        _widgetJsonProvider = widgetJsonProvider;
        _window = (Window)WidgetLocator.Current
            .ResolveByCondition(t => t.GetGenericTypeDefinition() == typeof(Widget<>))!;
    }

    protected override void Execute()
    {
        _widgetJsonProvider.UpdateModel<WidgetSettings>(widgetSettings => { widgetSettings.IsEnable = false; },
            AppSettings.WidgetSettingsFile);
        _window.Close();
    }
}