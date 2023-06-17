using Avalonia.Controls;
using ReactiveUI;

namespace FancyWidgets.ViewModels.PanelSettings;

public class FancyTextBoxViewModel : ReactiveObject
{
    private TextBox _textBox;
    private string? _title;

    public string? Title
    {
        get => _title;
        set => this.RaiseAndSetIfChanged(ref _title, value);
    }

    public TextBox TextBox
    {
        get => _textBox;
        set => this.RaiseAndSetIfChanged(ref _textBox, value);
    }
}