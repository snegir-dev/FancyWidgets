using System.Reactive;
using ReactiveUI;

namespace FancyWidgets.Common.Controls.WidgetContextMenu;

public abstract class WidgetContextMenuButton : ReactiveObject, IComparable<WidgetContextMenuButton>
{
    public abstract int Order { get; protected set; }
    public abstract string Content { get; set; }
    protected abstract void Execute();

    public int CompareTo(WidgetContextMenuButton? other)
    {
        if (other == null)
            return 1;

        var orderComparison = Order.CompareTo(other.Order);
        if (orderComparison == 0)
        {
            var currentAssemblyName = GetType().Assembly.FullName;
            if (currentAssemblyName == typeof(WidgetContextMenuButton).Assembly.FullName)
            {
                return 1;
            }

            return -1;
        }

        return orderComparison;
    }
}