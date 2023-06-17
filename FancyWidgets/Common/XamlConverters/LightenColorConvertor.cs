using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace FancyWidgets.Common.XamlConverters;

public class LightenColorConvertor : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is ISolidColorBrush color && parameter
                                                is string parameterString
                                            && double.TryParse(parameterString, NumberStyles.Any,
                                                CultureInfo.InvariantCulture, out var percentage))
        {
            var newR = (byte)(color.Color.R * (1 + percentage));
            var newG = (byte)(color.Color.G * (1 + percentage));
            var newB = (byte)(color.Color.B * (1 + percentage));

            return new Color(color.Color.A, newR, newG, newB);
        }


        return AvaloniaProperty.UnsetValue;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}