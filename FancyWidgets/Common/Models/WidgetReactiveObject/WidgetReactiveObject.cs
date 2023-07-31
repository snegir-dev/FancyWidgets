using System.Reactive.Disposables;
using Autofac;
using FancyWidgets.Common.Locators;
using FancyWidgets.Common.SettingProvider;
using FancyWidgets.Common.SettingProvider.Interfaces;
using FancyWidgets.Common.SettingProvider.Models;
using ReactiveUI;

namespace FancyWidgets.Common.Models.WidgetReactiveObject;

public class WidgetReactiveObject : ReactiveObject, IActivatableViewModel
{
    protected WidgetReactiveObject()
    {
        var reactiveObjectDataStatus = new ReactiveObjectDataStatus(this, false);
        this.WhenActivated(disposables =>
        {
            ReactiveObjectDataStatusContainer.Add(reactiveObjectDataStatus);
            using var scope = WidgetLocator.Context.BeginLifetimeScope();
            scope.Resolve<ISettingsProvider>().LoadSettings();

            Disposable.Create(() =>
                {
                    ReactiveObjectDataStatusContainer.Remove(reactiveObjectDataStatus);
                    reactiveObjectDataStatus = null!;
                })
                .DisposeWith(disposables);
        });
    }

    public ViewModelActivator Activator { get; } = new();
}