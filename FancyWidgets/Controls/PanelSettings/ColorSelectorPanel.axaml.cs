using Avalonia;
using Avalonia.Controls;

namespace FancyWidgets.Controls.PanelSettings;

public partial class ColorSelectorPanel : UserControl
{
    public static readonly DirectProperty<ColorSelectorPanel, string> TitleProperty =
        AvaloniaProperty.RegisterDirect<ColorSelectorPanel, string>(
            nameof(Title),
            o => o.Title,
            (o, v) => o.Title = v);

    private string _title = string.Empty;

    public string Title
    {
        get => _title;
        set => SetAndRaise(TitleProperty, ref _title, value);
    }

    public TextBox TextBoxControl { get; set; }

    public ColorSelectorPanel()
    {
        InitializeComponent();
        DataContext = this;
        TextBoxControl = TextBox;
    }
}