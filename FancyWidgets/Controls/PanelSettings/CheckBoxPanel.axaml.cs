using Avalonia;
using Avalonia.Controls;

namespace FancyWidgets.Controls.PanelSettings;

public partial class CheckBoxPanel : UserControl
{
    public static readonly DirectProperty<CheckBoxPanel, string> TitleProperty =
        AvaloniaProperty.RegisterDirect<CheckBoxPanel, string>(
            nameof(Title),
            o => o.Title,
            (o, v) => o.Title = v);

    private string _title = string.Empty;

    public string Title
    {
        get => _title;
        set => SetAndRaise(TitleProperty, ref _title, value);
    }

    public CheckBox CheckBoxControl { get; set; }

    public CheckBoxPanel()
    {
        InitializeComponent();
        DataContext = this;
        CheckBoxControl = CheckBox;
    }
}