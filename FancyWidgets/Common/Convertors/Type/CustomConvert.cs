using Avalonia.Media;
using Newtonsoft.Json;

namespace FancyWidgets.Common.Convertors.Type;

public static class CustomConvert
{
    public static object? ChangeType(object? value, global::System.Type conversionType)
    {
        if (string.IsNullOrWhiteSpace(value?.ToString()))
            return null;
        
        if (typeof(IBrush).IsAssignableFrom(conversionType))
            return Brush.Parse(value.ToString());
        if (typeof(Enum).IsAssignableFrom(conversionType))
            return Enum.Parse(conversionType, value.ToString());
        if (conversionType.IsClass)
            return JsonConvert.DeserializeObject(value.ToString(), conversionType);

        return Convert.ChangeType(value, conversionType);
    }
}