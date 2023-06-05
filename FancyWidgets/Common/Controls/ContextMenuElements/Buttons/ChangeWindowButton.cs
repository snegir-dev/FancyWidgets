using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Platform;
using ReactiveUI;

namespace FancyWidgets.Common.Controls.ContextMenuElements.Buttons;

public class ChangeWindowButton : WidgetContextMenu
{
    private const string EditContentButton = "Edit";
    private const string DisableEditingContentButton = "Disable editing";
    private bool _isPressed;
    private string _content = EditContentButton;
    private Point _startPosition;
    private Widget _widget;

    public override string Content
    {
        get => _content;
        set => this.RaiseAndSetIfChanged(ref _content, value);
    }

    protected override void Execute(Widget widget)
    {
        _widget = widget;
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