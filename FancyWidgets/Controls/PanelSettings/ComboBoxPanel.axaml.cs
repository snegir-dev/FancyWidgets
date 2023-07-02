using Avalonia;
using Avalonia.Controls;

namespace FancyWidgets.Controls.PanelSettings;

public partial class ComboBoxPanel : UserControl
{
    public static readonly DirectProperty<ComboBoxPanel, string> TitleProperty =
        AvaloniaProperty.RegisterDirect<ComboBoxPanel, string>(
            nameof(Title), o => o.Title, (o, v) => o.Title = v);
    
    private string _title = string.Empty;
    public string Title
    {
        get => _title;
        set => SetAndRaise(TitleProperty, ref _title, value);
    }
    public ComboBox ComboBoxControl { get; set; }
    
    public ComboBoxPanel()
    {
        InitializeComponent();
        DataContext = this;
        ComboBoxControl = ComboBox;
    }
}