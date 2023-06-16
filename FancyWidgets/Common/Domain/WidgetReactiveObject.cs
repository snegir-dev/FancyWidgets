using Autofac;
using FancyWidgets.Common.Locator;
using FancyWidgets.Common.SettingProvider;
using FancyWidgets.Common.SettingProvider.Interfaces;
using ReactiveUI;

namespace FancyWidgets.Common.Domain;

public class WidgetReactiveObject : ReactiveObject
{
    public WidgetReactiveObject()
    {
        InitialTrack();
    }

    private void InitialTrack()
    {
        ViewModelContainer.CurrentViewModel = this;
        WidgetLocator.Current.Resolve<ISettingsProvider>().LoadSettings();
    }
}