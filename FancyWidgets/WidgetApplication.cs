using Autofac;
using Avalonia.Platform;
using Avalonia.ReactiveUI;
using FancyWidgets.Common.Controls.WidgetContextMenu;
using FancyWidgets.Common.Controls.WidgetContextMenu.Buttons;
using FancyWidgets.Common.Controls.WidgetDragger;
using FancyWidgets.Common.Convertors.Json;
using FancyWidgets.Common.Convertors.NewtonsoftJson;
using FancyWidgets.Common.Locators;
using FancyWidgets.Common.SettingProvider;
using FancyWidgets.Controls;
using FancyWidgets.ViewModels;
using ReactiveUI;
using Splat;
using Splat.Autofac;

namespace FancyWidgets;

public static class WidgetApplication
{
    public static ContainerBuilder CreateBuilder()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<ContextMenuWindowViewModel>().AsSelf();
        builder.RegisterType<SettingsWindowViewModel>().AsSelf();
        builder.RegisterType<WidgetDisablingButton>().As<WidgetContextMenuButton>();
        builder.RegisterType<ChangingWindowButton>().As<WidgetContextMenuButton>();
        builder.RegisterType<ChangingSettingsButton>().As<WidgetContextMenuButton>();
        builder.RegisterType<DefaultWidgetDragger>().As<IWidgetDragger>().PreserveExistingDefaults();
        builder.RegisterType<WidgetJsonProvider>().As<IWidgetJsonProvider>();
        builder.AddSettingsProvider();
        NewtonsoftConfigurationInitializer.Initialize();
        
        return builder;
    }

    public static void BuildContainer(this ContainerBuilder builder)
    {
        var autofacResolver = builder.UseAutofacDependencyResolver();
        builder.RegisterInstance(autofacResolver);
        WidgetLocator.Current = builder.Build();
        RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;
    }
}