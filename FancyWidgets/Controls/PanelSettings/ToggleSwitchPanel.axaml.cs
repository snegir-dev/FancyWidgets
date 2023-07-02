using Avalonia;
using Avalonia.Controls;

namespace FancyWidgets.Controls.PanelSettings;

public partial class ToggleSwitchPanel : UserControl
{
    public static readonly DirectProperty<ToggleSwitchPanel, string> TitleProperty =
        AvaloniaProperty.RegisterDirect<ToggleSwitchPanel, string>(
            nameof(Title), o => o.Title, (o, v) => o.Title = v);

    private string _title = string.Empty;

    public string Title
    {
        get => _title;
        set => SetAndRaise(TitleProperty, ref _title, value);
    }

    public ToggleSwitch ToggleSwitchControl { get; set; }

    public ToggleSwitchPanel()
    {
        InitializeComponent();
        DataContext = this;
        ToggleSwitchControl = ToggleSwitch;
    }
}