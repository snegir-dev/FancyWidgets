namespace FancyWidgets.Models;

public class WidgetMetadata
{
    public WidgetInfo WidgetInfo { get; set; }
}

public class WidgetInfo
{
    public string WidgetName { get; set; }
    public string Uuid { get; set; }
    public string Description { get; set; }
    public string Version { get; set; }
    public string Url { get; set; }
    public string StartupDll { get; set; }
}