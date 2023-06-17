using Avalonia.Controls;
using ReactiveUI;

namespace FancyWidgets.ViewModels.PanelSettings;

public class FancyButtonViewModel : ReactiveObject
{
    private Button _button;
    private string? _title;

    public Button Button
    {
        get => _button;
        set => this.RaiseAndSetIfChanged(ref _button, value);
    }

    public string? Title
    {
        get => _title;
        set => this.RaiseAndSetIfChanged(ref _title, value);
    }
}