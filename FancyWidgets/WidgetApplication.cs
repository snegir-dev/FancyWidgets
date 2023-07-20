using Autofac;
using Avalonia.ReactiveUI;
using FancyWidgets.Common.Controls.WidgetContextMenu;
using FancyWidgets.Common.Controls.WidgetContextMenu.Buttons;
using FancyWidgets.Common.Controls.WidgetDragger;
using FancyWidgets.Common.Convertors.Json;
using FancyWidgets.Common.Convertors.NewtonsoftJson;
using FancyWidgets.Common.Locators;
using FancyWidgets.Common.SettingProvider;
using FancyWidgets.Common.SettingProvider.Interfaces;
using FancyWidgets.Common.System.IO;
using FancyWidgets.Controls;
using FancyWidgets.Models;
using FancyWidgets.ViewModels;
using Microsoft.Extensions.Configuration;
using ReactiveUI;
using Splat.Autofac;

namespace FancyWidgets;

public sealed class WidgetApplication
{
    private readonly ContainerBuilder _container;
    private Type? _widgetImplementationType;
    public ConfigurationManager Configuration { get; }

    public WidgetApplication()
    {
        _container = new ContainerBuilder();
        Configuration = new ConfigurationManager();
        _container.RegisterInstance<IConfiguration>(Configuration);
        _container.RegisterType<WidgetApplicationOptions>().AsSelf();
        _container.RegisterType<ContextMenuWindowViewModel>().AsSelf();
        _container.RegisterType<SettingsWindowViewModel>().AsSelf();
        _container.RegisterType<WidgetDisablingButton>().As<WidgetContextMenuButton>();
        _container.RegisterType<ChangingWindowButton>().As<WidgetContextMenuButton>();
        _container.RegisterType<ChangingSettingsButton>().As<WidgetContextMenuButton>();
        _container.RegisterType<DefaultWidgetDragger>().As<IWidgetDragger>().PreserveExistingDefaults();
        _container.RegisterType<WidgetJsonProvider>().As<IWidgetJsonProvider>();
        _container.AddSettingsProvider();
        NewtonsoftConfigurationInitializer.Initialize();
    }

    public WidgetApplication ConfigureServices(Action<ContainerBuilder> configureContainer)
    {
        configureContainer.Invoke(_container);
        return this;
    }

    public WidgetApplication ConfigureOptions(Action<WidgetApplicationOptions> configureOptions)
    {
        var options = new WidgetApplicationOptions();
        configureOptions.Invoke(options);
        _container.RegisterInstance(options).AsSelf();
        return this;
    }

    public WidgetApplication InitializeSettings()
    {
        using var container = _container.Build();
        var settingsInitializer = container.Resolve<ISettingsInitializer>();
        settingsInitializer.InitializeSettings();
        return this;
    }

    public WidgetApplication UseWidget<TService, TImplementation>()
        where TService : notnull
        where TImplementation : notnull
    {
        if (typeof(TService).BaseType?.GetGenericTypeDefinition() != typeof(Widget<>))
            throw new ArgumentException("The type is not a widget");

        _widgetImplementationType = typeof(TImplementation);
        _container.RegisterType<TService>()
            .As<TImplementation>()
            .SingleInstance();

        return this;
    }

    public TWidget Build<TWidget>()
        where TWidget : class
    {
        var autofacResolver = _container.UseAutofacDependencyResolver();
        _container.RegisterInstance(autofacResolver);
        WidgetLocator.Current = _container.Build();
        RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;

        if (_widgetImplementationType == null)
            throw new InvalidOperationException("You must call UseWidget before calling Build.");

        var t = WidgetLocator.Current.Resolve(_widgetImplementationType);
        if (t is not TWidget widget)
            throw new NullReferenceException("Widget not found.");

        return widget;
    }
}