using FancyWidgets.Common.SettingProvider;
using FancyWidgets.Common.SettingProvider.Interfaces;
using ReactiveUI;
using Splat;

namespace FancyWidgets.Common.Domain;

public class TrackedReactiveObject : ReactiveObject
{
    public TrackedReactiveObject()
    {
        InitialTracks();
    }

    private void InitialTracks()
    {
        ViewModelContainer.CurrentViewModel = this;
        Locator.Current.GetService<ISettingProvider>()?.LoadSettings();
    }
}