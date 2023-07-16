using FancyWidgets.Views;

namespace FancyWidgets.Common.Controls.WidgetContextMenu.Buttons;

public class ChangingSettingsButton : WidgetContextMenuButton
{
    public override int Order { get; protected set; } = 3;
    public override string Content { get; set; } = "Settings";

    protected override void Execute()
    {
        var stylesChangeWindow = new SettingsWindow();
        stylesChangeWindow.Show();
    }
}