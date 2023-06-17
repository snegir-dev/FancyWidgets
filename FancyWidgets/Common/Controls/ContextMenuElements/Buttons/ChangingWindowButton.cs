using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Platform;
using FancyWidgets.Common.Extensions;
using FancyWidgets.Common.Locators;
using ReactiveUI;
using Splat;

namespace FancyWidgets.Common.Controls.ContextMenuElements.Buttons;

public class ChangingWindowButton : WidgetContextMenu
{
    private readonly Window _widget;
    private const string EditContentButton = "Edit";
    private const string DisableEditingContentButton = "Disable editing";
    private bool _isPressed;
    private string _content = EditContentButton;

    public override int Order { get; protected set; } = 2;

    public override string Content
    {
        get => _content;
        set => this.RaiseAndSetIfChanged(ref _content, value);
    }

    public ChangingWindowButton()
    {
        _widget = (Window)WidgetLocator
            .Current.ResolveByCondition(t => t.GetGenericTypeDefinition() == typeof(Widget<>))!;
    }

    protected override void Execute()
    {
        if (_isPressed == false)
        {
            _widget.SystemDecorations = SystemDecorations.Full;
            Content = DisableEditingContentButton;
            _isPressed = true;
        }
        else
        {
            _widget.SystemDecorations = SystemDecorations.None;
            Content = EditContentButton;
            _isPressed = false;
        }
    }
}