using Autofac;
using Autofac.Core;

namespace FancyWidgets.Common.Extensions;

public static class AutofacContainerExtensions
{
    internal static object? ResolveByCondition(this IContainer container,
        Func<IComponentRegistration, bool> selector)
    {
        var registrations = container.ComponentRegistry.Registrations;
        var type = registrations
            .FirstOrDefault(selector);
        
        return type is null
            ? null
            : container.ResolveService(type.Services.ElementAt(0));
    }
}