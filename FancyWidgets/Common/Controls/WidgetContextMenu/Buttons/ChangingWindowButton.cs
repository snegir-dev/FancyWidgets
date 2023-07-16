using Avalonia.Controls;
using FancyWidgets.Common.Controls.WidgetDragger;
using FancyWidgets.Common.Extensions;
using FancyWidgets.Common.Locators;
using ReactiveUI;

namespace FancyWidgets.Common.Controls.WidgetContextMenu.Buttons;

public class ChangingWindowButton : WidgetContextMenuButton
{
    private readonly Window _widget;
    private const string EditContentButton = "Edit";
    private const string DisableEditingContentButton = "Disable editing";
    private const string NameDraggerContainer = "DraggerContainer";
    private bool _isPressed;
    private string _content = EditContentButton;
    private Control _control;
    private readonly UserControl _defaultWidgetDragger;
    private Grid _draggerContainer;

    public override int Order { get; protected set; } = 2;

    public override string Content
    {
        get => _content;
        set => this.RaiseAndSetIfChanged(ref _content, value);
    }

    public ChangingWindowButton(IWidgetDragger widgetDragger)
    {
        _widget = (Window)WidgetLocator
            .Current.ResolveByCondition(t => t.IsGenericType &&
                                             t.GetGenericTypeDefinition() == typeof(Widget<>))!;
        _defaultWidgetDragger = (UserControl)widgetDragger;
    }

    protected override void Execute()
    {
        _draggerContainer = _defaultWidgetDragger.FindControl<Grid>(NameDraggerContainer)
            ?? throw new NullReferenceException("WidgetDragger must have a main Grid container named 'DraggerContainer'");
        
        if (_isPressed == false)
            WindowToEditableState();
        else
            WindowToNoEditableState();
        
        _isPressed = !_isPressed;
    }
    
    private void WindowToEditableState()
    {
        _control = (Control)_widget.Content!;
        _widget.Content = null;
        _draggerContainer.Children.Insert(0, _control);
        _widget.Content = _defaultWidgetDragger;
        Content = DisableEditingContentButton;
    }

    private void WindowToNoEditableState()
    {
        _draggerContainer.Children.Remove(_control);
        _widget.Content = null;
        _widget.Content = _control;
        Content = EditContentButton;
    }
}