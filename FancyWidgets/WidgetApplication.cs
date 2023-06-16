using Autofac;
using Avalonia.ReactiveUI;
using FancyWidgets.Common.Controls.ContextMenuElements;
using FancyWidgets.Common.Controls.ContextMenuElements.Buttons;
using FancyWidgets.Common.Convertors.Json;
using FancyWidgets.Common.Locator;
using FancyWidgets.Common.SettingProvider;
using FancyWidgets.Common.SettingProvider.Interfaces;
using FancyWidgets.ViewModels;
using ReactiveUI;
using Splat.Autofac;

namespace FancyWidgets;

public static class WidgetApplication
{
    public static ContainerBuilder CreateBuilder()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<ContextMenuWindowViewModel>().AsSelf();
        builder.RegisterType<SettingsWindowViewModel>().AsSelf();
        builder.RegisterType<WidgetDisablingButton>().As<WidgetContextMenu>();
        builder.RegisterType<ChangingWindowButton>().As<WidgetContextMenu>();
        builder.RegisterType<ChangingSettingsButton>().As<WidgetContextMenu>();
        builder.RegisterType<WidgetJsonProvider>().As<IWidgetJsonProvider>();
        builder.RegisterType<SettingsProvider>().As<ISettingsProvider>();

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