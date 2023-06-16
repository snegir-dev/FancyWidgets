using FancyWidgets.Common.Convertors.Json;
using FancyWidgets.Common.SettingProvider.Interfaces;
using FancyWidgets.Common.SettingProvider.Models;
using FancyWidgets.Models;
using ReactiveUI;

namespace FancyWidgets.ViewModels;

public class SettingsWindowViewModel : ReactiveObject
{
    private List<SettingsElement> _settingElements = new();
    public IEnumerable<ISettingsControl> SettingsControls { get; set; }

    public List<SettingsElement> SettingElements
    {
        get => _settingElements;
        set => this.RaiseAndSetIfChanged(ref _settingElements, value);
    }
    
    public SettingsWindowViewModel(IWidgetJsonProvider widgetJsonProvider, 
        IEnumerable<ISettingsControl> settingsControls)
    {
        SettingElements = widgetJsonProvider.GetModel<List<SettingsElement>>(AppSettings.SettingsFile);
        SettingsControls = settingsControls;
    }
}