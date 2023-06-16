using ReactiveUI;

namespace FancyWidgets.ViewModels.SettingPanel;

public class FancyCheckBoxViewModel : ReactiveObject
{
    private CheckBox _checkBox;
    private string? _title;

    public CheckBox CheckBox
    {
        get => _checkBox;
        set => this.RaiseAndSetIfChanged(ref _checkBox, value);
    }
    
    public string? Title
    {
        get => _title;
        set => this.RaiseAndSetIfChanged(ref _title, value);
    }
}