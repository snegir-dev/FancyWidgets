using Avalonia.Controls;
using ReactiveUI;

namespace FancyWidgets.ViewModels;

public class FancyToggleSwitchViewModel : ReactiveObject
{
    private ToggleSwitch _toggleSwitch;
    private string? _title;

    public ToggleSwitch ToggleSwitch
    {
        get => _toggleSwitch;
        set => this.RaiseAndSetIfChanged(ref _toggleSwitch, value);
    }

    public string? Title
    {
        get => _title;
        set => this.RaiseAndSetIfChanged(ref _title, value);
    }
}