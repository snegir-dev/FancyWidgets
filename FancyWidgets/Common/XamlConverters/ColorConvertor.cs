using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace FancyWidgets.Common.XamlConverters;

public class ColorConvertor : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (!targetType.IsAssignableTo(typeof(IBrush)))
            return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);

        try
        {
            if (value != null)
                return Brush.Parse(value.ToString()!);

            return null;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}