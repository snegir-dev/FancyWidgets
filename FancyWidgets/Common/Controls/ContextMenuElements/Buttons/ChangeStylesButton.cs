using FancyWidgets.Common.SettingProvider.Models;
using FancyWidgets.Models;
using FancyWidgets.Views;

namespace FancyWidgets.Common.Controls.ContextMenuElements.Buttons;

public class ChangeStylesButton : WidgetContextMenu
{
    private readonly JsonFileManager _jsonFileManager = new();
    public override string Content { get; set; } = "Change styles";

    protected override void Execute(Widget widget)
    {
        var settingElements = _jsonFileManager.GetModelFromJson<List<SettingElement>>(AppSettings.SettingFile);
        var stylesChangeWindow = new SettingWindow
        {
            SettingElements = settingElements
        };
        stylesChangeWindow.Show();
    }
}