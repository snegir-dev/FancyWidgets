using Autofac;
using Autofac.Core;
using FancyWidgets.Common.Convertors.Json;
using FancyWidgets.Common.SettingProvider.Interfaces;
using FancyWidgets.Common.SettingProvider.Models;
using FancyWidgets.Models;

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
            .As<ISettingsReader>()
            .InstancePerDependency();
        builder.AddSettingsElements();
    }

    private static void AddSettingsElements(this ContainerBuilder builder)
    {
        builder.Register(context =>
        {
            var widgetJsonProvider = context.Resolve<IWidgetJsonProvider>();
            return widgetJsonProvider.GetModel<List<SettingsElement>>(AppSettings.SettingsFile)
                   ?? new List<SettingsElement>();
        }).AsSelf().InstancePerDependency();
    }
}