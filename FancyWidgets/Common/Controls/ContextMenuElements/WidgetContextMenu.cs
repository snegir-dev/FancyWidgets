using System.Reactive;
using ReactiveUI;

namespace FancyWidgets.Common.Controls.ContextMenuElements;

public abstract class WidgetContextMenu : ReactiveObject, IComparable<WidgetContextMenu>
{
    public abstract int Order { get; protected set; }
    public abstract string Content { get; set; }
    public ReactiveCommand<Unit, Unit> ExecuteCommand { get; protected set; }

    protected WidgetContextMenu()
    {
        ExecuteCommand = ReactiveCommand.Create(Execute);
    }

    protected abstract void Execute();

    public int CompareTo(WidgetContextMenu? other)
    {
        if (other == null)
            return 1;

        var orderComparison = Order.CompareTo(other.Order);
        if (orderComparison == 0)
        {
            var currentAssemblyName = GetType().Assembly.FullName;
            if (currentAssemblyName == typeof(WidgetContextMenu).Assembly.FullName)
            {
                return 1;
            }

            return -1;
        }

        return orderComparison;
    }
}