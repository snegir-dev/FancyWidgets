using System.Reactive;
using ReactiveUI;

namespace FancyWidgets.Common.Controls.ContextMenuElements;

public abstract class WidgetContextMenu : ReactiveObject
{
    public abstract string Content { get; set; }
    public ReactiveCommand<Unit, Unit> ExecuteCommand { get; protected set; }

    protected WidgetContextMenu()
    {
        ExecuteCommand = ReactiveCommand.Create(Execute);
    }

    protected abstract void Execute();
}