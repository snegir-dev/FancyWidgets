using Avalonia.Controls;
using ReactiveUI;

namespace FancyWidgets.Common.Controls.ContextMenuElements.Buttons;

public class ChangeWindow : WidgetContextMenu
{
    private const string EditContentButton = "Edit";
    private const string DisableEditingContentButton = "Disable editing";
    private bool _isPressed;
    private string _content = EditContentButton;

    public override string Content
    {
        get => _content;
        set => this.RaiseAndSetIfChanged(ref _content, value);
    }

    protected override void Execute(Widget widget)
    {
        if (_isPressed == false)
        {
            widget.SystemDecorations = SystemDecorations.Full;
            Content = DisableEditingContentButton;
            _isPressed = true;
        }
        else
        {
            widget.SystemDecorations = SystemDecorations.None;
            Content = EditContentButton;
            _isPressed = false;
        }
    }
}