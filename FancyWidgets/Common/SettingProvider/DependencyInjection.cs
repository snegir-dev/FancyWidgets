using Autofac;
using Autofac.Core;
using FancyWidgets.Common.SettingProvider.Interfaces;

namespace FancyWidgets.Common.SettingProvider;

internal static class DependencyInjection
{
    internal static void AddSettingsProvider(this ContainerBuilder builder)
    {
        builder.RegisterType<SettingsProvider>()
            .As<ISettingsProvider>()
            .As<ISettingsInitializer>()
            .As<ISettingsLoader>()
            .As<ISettingsModifier>()
            .As<ISettingsReader>();
    }
}