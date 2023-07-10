using FancyWidgets.Common.Domain;

namespace FancyWidgets.Common.SettingProvider.Models;

public class ReactiveObjectDataStatus
{
    public WidgetReactiveObject WidgetReactiveObject { get; set; }
    public bool IsDataLoaded { get; set; }

    public ReactiveObjectDataStatus(WidgetReactiveObject widgetReactiveObject, bool isDataLoaded)
    {
        WidgetReactiveObject = widgetReactiveObject;
        IsDataLoaded = isDataLoaded;
    }
}