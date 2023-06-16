using FancyWidgets.Common.SettingProvider.Models;
using FancyWidgets.Models;
using FancyWidgets.Views;
using Newtonsoft.Json;

namespace FancyWidgets.Common.Controls.ContextMenuElements.Buttons;

public class ChangingSettingsButton : WidgetContextMenu
{
    public override string Content { get; set; } = "Settings";

    protected override void Execute()
    {
        var stylesChangeWindow = new SettingsWindow();
        stylesChangeWindow.Show();
    }
}