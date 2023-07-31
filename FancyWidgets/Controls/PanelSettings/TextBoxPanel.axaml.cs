using Avalonia.Controls;

namespace FancyWidgets.Controls.PanelSettings;

public partial class TextBoxPanel : UserControl
{
    public TextBoxPanel()
    {
        InitializeComponent();
        DataContext = this;
    }

    public string? Title
    {
        get => TitleControl.Text;
        set => TitleControl.Text = value;
    }
}