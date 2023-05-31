using ReactiveUI;

namespace FancyWidgets.Common.Controls.ContextMenuElements.Buttons;

public class DisableWidgetButton : WidgetContextMenu
{
    public override string Content { get; set; } = "Disable";

    public DisableWidgetButton()
    {
        ExecuteCommand = ReactiveCommand.Create<Widget>(Execute);
    }

    protected override void Execute(Widget widget)
    {
        widget.Close();
    }
}