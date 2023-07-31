namespace FancyWidgets.Common.Models.WidgetReactiveObject;

internal class ReactiveObjectDataStatus
{
    public WidgetReactiveObject WidgetReactiveObject { get; }
    public bool IsDataLoaded { get; set; }

    public ReactiveObjectDataStatus(WidgetReactiveObject widgetReactiveObject, bool isDataLoaded)
    {
        WidgetReactiveObject = widgetReactiveObject;
        IsDataLoaded = isDataLoaded;
    }
}