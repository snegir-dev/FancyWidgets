using Avalonia;
using Avalonia.Controls;

namespace FancyWidgets.Controls.PanelSettings;

public partial class TextBoxPanel : UserControl
{
    public static readonly DirectProperty<TextBoxPanel, string> TitleProperty =
        AvaloniaProperty.RegisterDirect<TextBoxPanel, string>(
            nameof(Title),
            o => o.Title,
            (o, v) => o.Title = v);

    private string _title = string.Empty;

    public string Title
    {
        get => _title;
        set => SetAndRaise(TitleProperty, ref _title, value);
    }

    public TextBox TextBoxControl { get; private set; }

    public TextBoxPanel()
    {
        InitializeComponent();
        DataContext = this;
        TextBoxControl = TextBox;
    }
}