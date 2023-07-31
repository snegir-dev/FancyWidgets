using Avalonia.Media;

namespace FancyWidgets.Common.Controls.Types;

public class BrushConvert
{
    public static bool TryPars(string? value, out IBrush? brush)
    {
        if (value == null)
        {
            brush = null;
            return false;
        }

        try
        {
            brush = Brush.Parse(value);
            return true;
        }
        catch (Exception e)
        {
            brush = null;
            return false;
        }
    }
}