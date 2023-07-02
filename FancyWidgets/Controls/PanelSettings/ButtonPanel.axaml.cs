using Avalonia;
using Avalonia.Controls;

namespace FancyWidgets.Controls.PanelSettings;

public partial class ButtonPanel : UserControl
{
    public static readonly DirectProperty<ButtonPanel, string> TitleProperty =
        AvaloniaProperty.RegisterDirect<ButtonPanel, string>(
            nameof(Title),
            o => o.Title,
            (o, v) => o.Title = v);

    public static readonly DirectProperty<ButtonPanel, string> ButtonContentProperty =
        AvaloniaProperty.RegisterDirect<ButtonPanel, string>(
            nameof(ButtonContent),
            o => o.ButtonContent,
            (o, v) => o.ButtonContent = v);

    private string _title = string.Empty;
    private string _buttonContent = string.Empty;

    public string Title
    {
        get => _title;
        set => SetAndRaise(TitleProperty, ref _title, value);
    }

    public string ButtonContent
    {
        get => _buttonContent;
        set
        {
            SetAndRaise(ButtonContentProperty, ref _buttonContent, value);
            Button.Content = value;
        }
    }

    public Button ButtonControl { get; private set; }

    public ButtonPanel()
    {
        InitializeComponent();
        DataContext = this;
        ButtonControl = Button;
    }
}