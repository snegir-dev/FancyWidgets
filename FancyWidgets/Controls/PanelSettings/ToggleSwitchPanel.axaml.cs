using Avalonia;
using Avalonia.Controls;

namespace FancyWidgets.Controls.PanelSettings;

public partial class ToggleSwitchPanel : UserControl
{
    public ToggleSwitchPanel()
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