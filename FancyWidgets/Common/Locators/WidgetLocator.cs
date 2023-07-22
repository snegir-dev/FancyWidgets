using Autofac;

namespace FancyWidgets.Common.Locators;

public static class WidgetLocator
{
    public static IComponentContext Context { get; internal set; }
}