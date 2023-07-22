using Autofac;
using Autofac.Core;

namespace FancyWidgets.Common.Extensions;

public static class AutofacContainerExtensions
{
    internal static object? ResolveByCondition(this IComponentContext context,
        Func<IComponentRegistration, bool> selector)
    {
        var registrations = context.ComponentRegistry.Registrations;
        var type = registrations
            .FirstOrDefault(selector);

        return type is null
            ? null
            : context.ResolveService(type.Services.ElementAt(0));
    }
}