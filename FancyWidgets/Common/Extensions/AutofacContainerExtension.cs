using Autofac;
using FancyWidgets.Common.Locator;

namespace FancyWidgets.Common.Extensions;

public static class AutofacContainerExtension
{
    internal static object? ResolveByCondition(this IContainer container,
        Func<Type, bool> selector)
    {
        var registrations = container.ComponentRegistry.Registrations;

        var type = registrations
            .SelectMany(r => r.Services)
            .Select(s => Type.GetType(s.Description))
            .Where(t => t is not null)
            .FirstOrDefault(selector!);

        return type is null
            ? null
            : WidgetLocator.Current.ResolveOptional(type);
    }
}