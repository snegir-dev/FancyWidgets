using Avalonia.Controls;
using FancyWidgets.Common.Convertors.Json;
using FancyWidgets.Common.Extensions;
using FancyWidgets.Common.Locators;
using FancyWidgets.Models;

namespace FancyWidgets.Common.Controls.ContextMenuElements.Buttons;

public class WidgetDisablingButton : WidgetContextMenuButton
{
    private readonly Window _window;
    private readonly IWidgetJsonProvider _widgetJsonProvider;
    public override string Content { get; set; } = "Disable";
    public override int Order { get; protected set; } = 1;

    public WidgetDisablingButton(IWidgetJsonProvider widgetJsonProvider)
    {
        _widgetJsonProvider = widgetJsonProvider;
        _window = (Window)WidgetLocator.Current
            .ResolveByCondition(t => t.IsGenericType &&
                                     t.GetGenericTypeDefinition() == typeof(Widget<>))!;
    }

    protected override void Execute()
    {
        _widgetJsonProvider.UpdateModel<WidgetSettings>(widgetSettings => { widgetSettings.IsEnable = false; },
            AppSettings.WidgetSettingsFile, true);
        _window.Close();
    }
}