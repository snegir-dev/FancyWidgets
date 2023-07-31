using FancyWidgets.Common.SettingProvider.Interfaces;
using ReactiveUI;

namespace FancyWidgets.ViewModels;

public class SettingsWindowViewModel : ReactiveObject
{
    public SettingsWindowViewModel(IEnumerable<ISettingsControl> settingsControls)
    {
        SettingsControls = settingsControls.OrderBy(c => c.Order).ToList();
    }

    public IEnumerable<ISettingsControl> SettingsControls { get; }
}