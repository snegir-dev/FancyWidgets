using FancyWidgets.Common.SettingProvider.Interfaces;
using FancyWidgets.Views;

namespace FancyWidgets.Common.Controls.WidgetContextMenu.Buttons;

public class ChangingSettingsButton : WidgetContextMenuButton
{
    private SettingsWindow? _settingsWindow;

    public override int Order { get; protected set; } = 3;
    public override string Content { get; set; } = "Settings";

    protected override void Execute()
    {
        _settingsWindow = new SettingsWindow();
        _settingsWindow.Closed += SettingsWindow_Closed;
        _settingsWindow.Show();
    }
    
    private void SettingsWindow_Closed(object? sender, EventArgs e)
    {
        if (_settingsWindow != null)
        {
            _settingsWindow.Closed -= SettingsWindow_Closed;
            _settingsWindow = null;
        }
    }
}