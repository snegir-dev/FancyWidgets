using Avalonia.Media;
using Newtonsoft.Json;

namespace FancyWidgets.Common.Convertors.Type;

public static class CustomConvert
{
    public static object? ChangeType(object? value, global::System.Type conversionType)
    {
        var valueStr = value?.ToString();
        if (string.IsNullOrWhiteSpace(valueStr))
            return null;

        if (typeof(IBrush).IsAssignableFrom(conversionType))
            return Brush.Parse(valueStr);
        if (typeof(Enum).IsAssignableFrom(conversionType))
            return Enum.Parse(conversionType, valueStr);
        if (conversionType.IsClass || conversionType.IsValueType)
            return JsonConvert.DeserializeObject(valueStr, conversionType);

        return Convert.ChangeType(value, conversionType);
    }
}