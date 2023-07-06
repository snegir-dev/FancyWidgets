using Autofac;
using FancyWidgets.Common.Locators;
using FancyWidgets.Common.SettingProvider;
using FancyWidgets.Common.SettingProvider.Interfaces;
using ReactiveUI;

namespace FancyWidgets.Common.Domain;

public class WidgetReactiveObject : ReactiveObject, IDisposable
{
    protected WidgetReactiveObject()
    {
        InitialTrack();
    }

    private void InitialTrack()
    {
        ViewModelsContainer.CurrentViewModels.Add(this);
        WidgetLocator.Current.Resolve<ISettingsProvider>().LoadSettings();
    }

    void IDisposable.Dispose()
    {
        ViewModelsContainer.CurrentViewModels.Remove(this);
    }
}