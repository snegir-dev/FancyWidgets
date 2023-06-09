using Autofac;
using Avalonia.ReactiveUI;
using FancyWidgets.Common.Controls.ContextMenuElements;
using FancyWidgets.Common.Controls.ContextMenuElements.Buttons;
using FancyWidgets.Common.SettingProvider;
using FancyWidgets.Common.SettingProvider.Interfaces;
using ReactiveUI;
using Splat;
using Splat.Autofac;

namespace FancyWidgets;

public static class WidgetApplication
{
    public static ContainerBuilder CreateBuilder()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<DisableWidgetButton>().As<WidgetContextMenu>();
        builder.RegisterType<ChangeWindowButton>().As<WidgetContextMenu>();
        builder.RegisterType<ChangeStylesButton>().As<WidgetContextMenu>();
        builder.RegisterType<SettingProvider>().As<ISettingProvider>();
        
        Locator.CurrentMutable.InitializeSplat();
        RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;

        return builder;
    }

    public static void BuildContainer(this ContainerBuilder builder)
    {
        var autofacResolver = builder.UseAutofacDependencyResolver();
        builder.RegisterInstance(autofacResolver);
        autofacResolver.SetLifetimeScope(builder.Build());
    }
}