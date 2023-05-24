using System.Reactive;
using ReactiveUI;

namespace FancyWidgets.Common.Controls.ContextMenuElements;

public abstract class WidgetContextMenu : ReactiveObject
{
    public abstract string Content { get; set; }
    public ReactiveCommand<Widget, Unit> ExecuteCommand { get; protected set; }

    protected WidgetContextMenu()
    {
        ExecuteCommand = ReactiveCommand.Create<Widget>(Execute);
    }

    protected abstract void Execute(Widget widget);
}