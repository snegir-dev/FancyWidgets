using System.Globalization;
using Avalonia.Data.Converters;

namespace FancyWidgets.Common.XamlConverters;

public class SubtractionConverter : IMultiValueConverter
{
    public object? Convert(IList<object?>? values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values is [double first, double second, ..])
            return second - first;

        return null;
    }
}