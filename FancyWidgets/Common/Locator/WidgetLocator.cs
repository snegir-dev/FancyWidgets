using Autofac;

namespace FancyWidgets.Common.Locator;

public static class WidgetLocator
{
    public static IContainer Current { get; internal set; }
}