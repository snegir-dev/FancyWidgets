using Autofac;
using FancyWidgets.Common.Locators;
using FancyWidgets.Common.SettingProvider;
using FancyWidgets.Common.SettingProvider.Interfaces;
using FancyWidgets.Common.SettingProvider.Models;
using ReactiveUI;

namespace FancyWidgets.Common.Domain;

public class WidgetReactiveObject : ReactiveObject, IDisposable
{
    private ReactiveObjectDataStatus _reactiveObjectDataStatus;

    protected WidgetReactiveObject() => InitialTrack();

    private void InitialTrack()
    {
        _reactiveObjectDataStatus = new ReactiveObjectDataStatus(this, false);
        ReactiveObjectDataStatusContainer.CurrentObjectDataStatuses.Add(_reactiveObjectDataStatus);
        WidgetLocator.Current.Resolve<ISettingsProvider>().LoadSettings();
    }

    void IDisposable.Dispose() => 
        ReactiveObjectDataStatusContainer.CurrentObjectDataStatuses.Remove(_reactiveObjectDataStatus);
}