using Autofac;

namespace FancyWidgets.Common.Locators;

public static class WidgetLocator
{
    internal static IContainer Context { get; set; }

    public static ILifetimeScope BeginLifetimeScope()
    {
        return Context.BeginLifetimeScope();
    }
}