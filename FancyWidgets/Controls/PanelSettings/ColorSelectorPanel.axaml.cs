using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using FancyWidgets.Common.Controls.Types;

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


    private void TextBoxControl_OnKeyUp(object? sender, KeyEventArgs e)
    {
        if (sender is not TextBox textBox)
            return;

        if (BrushConvert.TryPars(textBox.Text, out var brush))
            ColorDisplayControl.Background = brush;
        else
            ColorDisplayControl.Background = Brushes.Transparent;
    }

    private void TextBoxControl_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        TextBoxControl_OnKeyUp(sender, new KeyEventArgs());
    }
}