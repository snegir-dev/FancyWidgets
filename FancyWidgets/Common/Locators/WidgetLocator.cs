using Autofac;

namespace FancyWidgets.Common.Locators;

public static class WidgetLocator
{
    public static IContainer Current { get; internal set; }
}