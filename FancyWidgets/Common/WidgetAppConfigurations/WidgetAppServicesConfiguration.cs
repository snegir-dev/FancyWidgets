using Autofac;
using FancyWidgets.Common.Controls.WidgetContextMenu;
using FancyWidgets.Common.Controls.WidgetContextMenu.Buttons;
using FancyWidgets.Common.Controls.WidgetDragger;
using FancyWidgets.Common.Convertors.Json;
using FancyWidgets.Common.Convertors.NewtonsoftJson;
using FancyWidgets.Common.SettingProvider;
using FancyWidgets.Common.SettingProvider.Interfaces;
using FancyWidgets.Common.WidgetAppConfigurations.Interfaces;
using FancyWidgets.Controls;
using FancyWidgets.ViewModels;

namespace FancyWidgets.Common.WidgetAppConfigurations;

public class WidgetAppServicesConfiguration : IWidgetAppServicesConfiguration
{
    private readonly ContainerBuilder _container;

    public WidgetAppServicesConfiguration(ContainerBuilder container)
    {
        _container = container;
    }

    public void Configure()
    {
        _container.AddSettingsProvider();
        _container.RegisterType<WidgetApplicationOptions>().AsSelf().SingleInstance();
        _container.RegisterType<ContextMenuWindowViewModel>().AsSelf().SingleInstance();
        _container.RegisterType<SettingsWindowViewModel>().AsSelf().InstancePerDependency();
        _container.RegisterType<WidgetDisablingButton>().As<WidgetContextMenuButton>().InstancePerDependency();
        _container.RegisterType<ChangingWindowButton>().As<WidgetContextMenuButton>().InstancePerDependency();
        _container.RegisterType<ChangingSettingsButton>().As<WidgetContextMenuButton>().InstancePerDependency();
        _container.RegisterType<DefaultWidgetDragger>().As<IWidgetDragger>().InstancePerDependency();
        _container.RegisterType<WidgetJsonProvider>().As<IWidgetJsonProvider>().InstancePerDependency();
        _container.RegisterType<SettingElementOperations>().As<ISettingElementOperations>().InstancePerDependency();
        NewtonsoftConfigurationInitializer.Initialize();
    }
}