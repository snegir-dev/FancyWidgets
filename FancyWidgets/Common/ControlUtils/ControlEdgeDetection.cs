using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;

namespace FancyWidgets.Common.ControlUtils;

public class ControlEdgeDetection
{
    protected readonly Control Control;
    protected readonly int EdgeTolerance;

    public ControlEdgeDetection(Control control, int edgeTolerance)
    {
        Control = control;
        EdgeTolerance = edgeTolerance;
    }

    public virtual bool IsEdge(Point position)
    {
        return position.X < EdgeTolerance ||
               position.Y < EdgeTolerance ||
               position.X > Control.Bounds.Width - EdgeTolerance ||
               position.Y > Control.Bounds.Height - EdgeTolerance;
    }

    public virtual WindowEdge DetermineEdge(Point position)
    {
        if (position.X < EdgeTolerance)
        {
            if (position.Y < EdgeTolerance)
                return WindowEdge.NorthWest;
            if (position.Y > Control.Bounds.Height - EdgeTolerance)
                return WindowEdge.SouthWest;
            return WindowEdge.West;
        }

        if (position.X > Control.Bounds.Width - EdgeTolerance)
        {
            if (position.Y < EdgeTolerance)
                return WindowEdge.NorthEast;
            if (position.Y > Control.Bounds.Height - EdgeTolerance)
                return WindowEdge.SouthEast;
            return WindowEdge.East;
        }

        if (position.Y < EdgeTolerance)
            return WindowEdge.North;
        return WindowEdge.South;
    }

    public virtual StandardCursorType GetCursorType(Point position)
    {
        var edge = DetermineEdge(position);
        return edge switch
        {
            WindowEdge.West => StandardCursorType.LeftSide,
            WindowEdge.East => StandardCursorType.RightSide,
            WindowEdge.South => StandardCursorType.TopSide,
            WindowEdge.North => StandardCursorType.BottomSide,
            WindowEdge.SouthWest => StandardCursorType.BottomLeftCorner,
            WindowEdge.NorthEast => StandardCursorType.TopRightCorner,
            WindowEdge.SouthEast => StandardCursorType.BottomRightCorner,
            WindowEdge.NorthWest => StandardCursorType.TopLeftCorner,
            _ => StandardCursorType.Arrow
        };
    }
}