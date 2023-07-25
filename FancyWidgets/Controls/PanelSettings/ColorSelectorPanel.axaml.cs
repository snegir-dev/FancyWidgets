using Avalonia;
using Avalonia.Controls;

namespace FancyWidgets.Controls.PanelSettings;

public partial class ColorSelectorPanel : UserControl
{
    public ColorSelectorPanel()
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