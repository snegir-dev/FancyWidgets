using FancyWidgets.Common.Domain;

namespace FancyWidgets.Common.SettingProvider.Models;

public class ObjectWithDataStatus
{
    public WidgetReactiveObject WidgetReactiveObject { get; set; }
    public bool IsDataLoaded { get; set; }

    public ObjectWithDataStatus(WidgetReactiveObject widgetReactiveObject, bool isDataLoaded)
    {
        WidgetReactiveObject = widgetReactiveObject;
        IsDataLoaded = isDataLoaded;
    }
}