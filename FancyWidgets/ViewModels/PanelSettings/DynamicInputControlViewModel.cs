using FancyWidgets.Common.SettingProvider.Interfaces;
using ReactiveUI;

namespace FancyWidgets.ViewModels.PanelSettings;

public class DynamicInputControlViewModel : ReactiveObject
{
    public IEnumerable<ISettingsControl> SettingsControls { get; set; }
    
    public DynamicInputControlViewModel(IEnumerable<ISettingsControl> settingsControls)
    {
        SettingsControls = settingsControls;
    }
}