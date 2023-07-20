using Avalonia.Controls;
using Avalonia.VisualTree;
using FancyWidgets.Common.Constants;
using FancyWidgets.Common.Extensions;
using FancyWidgets.Common.Locators;
using ReactiveUI;

namespace FancyWidgets.Common.Controls.WidgetContextMenu.Buttons;

public class ChangingWindowButton : WidgetContextMenuButton
{
    private readonly Window _widget;
    private const string EditContentButton = "Edit";
    private const string DisableEditingContentButton = "Disable editing";
    private bool _isPressed;
    private string _content = EditContentButton;
    private readonly Control _draggerContainer;

    public override int Order { get; protected set; } = 2;

    public override string Content
    {
        get => _content;
        set => this.RaiseAndSetIfChanged(ref _content, value);
    }

    public ChangingWindowButton()
    {
        _widget = (Window)WidgetLocator
            .Current.ResolveByCondition(t => t.Activator.LimitType.BaseType?.IsGenericType == true &&
                                             t.Activator.LimitType.BaseType.GetGenericTypeDefinition() == typeof(Widget<>))!;

        _draggerContainer = GetDraggerContainer();
    }

    protected override void Execute()
    {
        if (_isPressed == false)
            WindowToEditableState();
        else
            WindowToNoEditableState();

        _isPressed = !_isPressed;
    }

    private void WindowToEditableState()
    {
        _draggerContainer.IsVisible = true;
        Content = DisableEditingContentButton;
    }

    private void WindowToNoEditableState()
    {
        _draggerContainer.IsVisible = false;
        Content = EditContentButton;
    }

    private Control GetDraggerContainer()
    {
        var t = _widget.GetVisualChildren();
        var draggerContainer = ((Panel)_widget.GetVisualChildren()
                .ToList()[0]).Children
            .FirstOrDefault(v => v.Name == UiElementNames.DraggerContainer);
        if (draggerContainer == null)
            throw new NullReferenceException(
                $"Control with the name '{UiElementNames.DraggerContainer}' not found ");

        return draggerContainer;
    }
}