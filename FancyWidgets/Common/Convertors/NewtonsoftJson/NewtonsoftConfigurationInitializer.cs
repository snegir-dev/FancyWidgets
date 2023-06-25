using Newtonsoft.Json;

namespace FancyWidgets.Common.Convertors.NewtonsoftJson;

public class NewtonsoftConfigurationInitializer
{
    public static void Initialize()
    {
        JsonConvert.DefaultSettings = () => new JsonSerializerSettings
        {
            Converters = { new CornerRadiusConverter() }
        };
    }
}