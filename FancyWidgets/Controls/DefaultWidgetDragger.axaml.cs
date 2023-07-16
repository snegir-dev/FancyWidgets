using Avalonia.Controls;
using Avalonia.Input;
using FancyWidgets.Common.Controls.WidgetDragger;
using FancyWidgets.Common.ControlUtils;

namespace FancyWidgets.Controls;

public partial class DefaultWidgetDragger : UserControl, IWidgetDragger
{
    private readonly ControlEdgeDetection _controlEdgeDetection;
    private const int SizeEdge = 10;

    public DefaultWidgetDragger()
    {
        _controlEdgeDetection = new ControlEdgeDetection(this, SizeEdge);
        InitializeComponent();
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        var position = e.GetPosition(this);
        Cursor = _controlEdgeDetection.IsEdge(position)
            ? new Cursor(_controlEdgeDetection.GetCursorType(position))
            : new Cursor(StandardCursorType.Arrow);
        
        base.OnPointerMoved(e);
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        var position = e.GetPosition(this);
        if (VisualRoot is Window window)
        {
            if (_controlEdgeDetection.IsEdge(position))
            {
                if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
                {
                    var edge = _controlEdgeDetection.DetermineEdge(position);
                    window.BeginResizeDrag(edge, e);
                }
            }
            else
            {
                Cursor = new Cursor(StandardCursorType.SizeAll);
                window.BeginMoveDrag(e);
            }
        }

        base.OnPointerPressed(e);
        Cursor = new Cursor(StandardCursorType.Arrow);
    }
}