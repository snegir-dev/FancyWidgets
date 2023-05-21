using ReactiveUI;

namespace FancyWidgets.ContextMenuElements.Buttons;

public class Disable : WidgetContextMenu
{
    public override string Content { get; set; } = "Disable";

    public Disable()
    {
        ExecuteCommand = ReactiveCommand.Create<Widget>(Execute);
    }

    protected override void Execute(Widget widget)
    {
        widget.Close();
    }
}