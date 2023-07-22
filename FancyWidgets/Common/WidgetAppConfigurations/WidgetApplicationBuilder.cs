using Autofac;
using Avalonia.ReactiveUI;
using FancyWidgets.Common.Locators;
using FancyWidgets.Common.SettingProvider.Interfaces;
using FancyWidgets.Common.WidgetAppConfigurations.Interfaces;
using Microsoft.Extensions.Configuration;
using ReactiveUI;
using Splat.Autofac;

namespace FancyWidgets.Common.WidgetAppConfigurations;

public class WidgetApplicationBuilder
{
    private Type? _widgetImplementationType;
    public ContainerBuilder Services { get; }
    public ConfigurationManager Configuration { get; }

    public WidgetApplicationBuilder(ContainerBuilder servicesBuilder,
        IWidgetAppConfiguration appConfiguration, IWidgetAppServicesConfiguration servicesConfiguration)
    {
        Services = servicesBuilder;
        Configuration = appConfiguration.Configuration;
        
        servicesConfiguration.Configure();
        appConfiguration.LoadConfig();
    }

    public static WidgetApplicationBuilder CreateBuilder()
    {
        var containerBuilder = new ContainerBuilder();
        var configuration = new ConfigurationManager();
        var appConfig = new WidgetAppConfiguration(configuration);
        var servicesConfig = new WidgetAppServicesConfiguration(containerBuilder);

        return new WidgetApplicationBuilder(containerBuilder, appConfig, servicesConfig);
    }

    public WidgetApplicationBuilder ConfigureOptions(Action<WidgetApplicationOptions> configureOptions)
    {
        var options = new WidgetApplicationOptions();
        configureOptions.Invoke(options);
        Services.RegisterInstance(options).AsSelf();
        return this;
    }

    public void InitializeSettings()
    {
        Services.RegisterBuildCallback(scope =>
            scope.Resolve<ISettingsInitializer>().InitializeSettings());
    }

    public void UseWidget<TService, TImplementation>()
        where TService : notnull
        where TImplementation : notnull
    {
        if (typeof(TService).BaseType?.GetGenericTypeDefinition() != typeof(Widget<>))
            throw new ArgumentException("The type is not a widget");

        _widgetImplementationType = typeof(TImplementation);
        Services.RegisterType<TService>()
            .As<TImplementation>()
            .SingleInstance();
    }

    public TWidget Build<TWidget>()
        where TWidget : class
    {
        Services.RegisterInstance(Configuration).As<IConfiguration>();
        var autofacResolver = Services.UseAutofacDependencyResolver();
        Services.RegisterInstance(autofacResolver);
        WidgetLocator.Context = Services.Build();
        RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;

        if (_widgetImplementationType == null)
            throw new InvalidOperationException("You must call UseWidget before calling Build.");

        var widgetObj = WidgetLocator.Context.Resolve(_widgetImplementationType);
        if (widgetObj is not TWidget widget)
            throw new NullReferenceException("Widget not found.");

        return widget;
    }
}