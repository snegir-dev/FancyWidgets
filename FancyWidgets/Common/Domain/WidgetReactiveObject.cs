using Autofac;
using FancyWidgets.Common.Locators;
using FancyWidgets.Common.SettingProvider;
using FancyWidgets.Common.SettingProvider.Interfaces;
using FancyWidgets.Common.SettingProvider.Models;
using ReactiveUI;

namespace FancyWidgets.Common.Domain;

public class WidgetReactiveObject : ReactiveObject, IDisposable
{
    private ObjectWithDataStatus _objectWithDataStatus;

    protected WidgetReactiveObject() => InitialTrack();

    private void InitialTrack()
    {
        _objectWithDataStatus = new ObjectWithDataStatus(this, false);
        ViewModelsContainer.CurrentViewModels.Add(_objectWithDataStatus);
        WidgetLocator.Current.Resolve<ISettingsProvider>().LoadSettings();
    }

    void IDisposable.Dispose() => 
        ViewModelsContainer.CurrentViewModels.Remove(_objectWithDataStatus);
}