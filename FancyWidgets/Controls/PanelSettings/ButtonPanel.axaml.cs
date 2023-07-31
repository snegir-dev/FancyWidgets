using Avalonia;
using Avalonia.Controls;

namespace FancyWidgets.Controls.PanelSettings;

public partial class ButtonPanel : UserControl
{
    public string? Title
    {
        get => TitleControl.Text;
        set => TitleControl.Text = value;
    }

    public object? ButtonContent
    {
        get => Button.Content;
        set => Button.Content = value;
    }

    public ButtonPanel()
    {
        InitializeComponent();
        DataContext = this;
    }
}