using Avalonia.Controls;

namespace FancyWidgets.Controls.PanelSettings;

public partial class CheckBoxPanel : UserControl
{
    public CheckBoxPanel()
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