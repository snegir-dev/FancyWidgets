using ReactiveUI;

namespace FancyWidgets.ViewModels.SettingPanel;

public class FancyComboBoxViewModel : ReactiveObject
{
    private ComboBox _comboBox;
    private string? _title;

    public ComboBox ComboBox
    {
        get => _comboBox;
        set => this.RaiseAndSetIfChanged(ref _comboBox, value);
    }

    public string? Title
    {
        get => _title;
        set => this.RaiseAndSetIfChanged(ref _title, value);
    }
}