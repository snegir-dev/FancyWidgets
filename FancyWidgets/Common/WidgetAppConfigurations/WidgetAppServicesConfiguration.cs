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
        _container.RegisterType<WidgetApplicationOptions>().AsSelf();
        _container.RegisterType<ContextMenuWindowViewModel>().AsSelf();
        _container.RegisterType<SettingsWindowViewModel>().AsSelf();
        _container.RegisterType<WidgetDisablingButton>().As<WidgetContextMenuButton>();
        _container.RegisterType<ChangingWindowButton>().As<WidgetContextMenuButton>();
        _container.RegisterType<ChangingSettingsButton>().As<WidgetContextMenuButton>();
        _container.RegisterType<DefaultWidgetDragger>().As<IWidgetDragger>().PreserveExistingDefaults();
        _container.RegisterType<WidgetJsonProvider>().As<IWidgetJsonProvider>();
        _container.RegisterType<SettingElementOperations>().As<ISettingElementOperations>();
        _container.AddSettingsProvider();
        NewtonsoftConfigurationInitializer.Initialize();
    }
}