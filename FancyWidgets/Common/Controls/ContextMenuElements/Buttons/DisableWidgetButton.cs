using ReactiveUI;

namespace FancyWidgets.Common.Controls.ContextMenuElements.Buttons;

public class DisableWidgetButton : WidgetContextMenu
{
    public override string Content { get; set; } = "Disable";

    protected override void Execute(Widget widget)
    {
        widget.Close();
    }
}