using FancyWidgets.Common.SettingProvider.Models;
using ReactiveUI;

namespace FancyWidgets.ViewModels;

public class StylesChangeWindowViewModel : ReactiveObject
{
    private List<SettingElement> _settingElements = new();
    
    public List<SettingElement> SettingElements
    {
        get => _settingElements;
        set => this.RaiseAndSetIfChanged(ref _settingElements, value);
    }
}