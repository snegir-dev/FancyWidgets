using Avalonia;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FancyWidgets.Common.Convertors.NewtonsoftJson;

public class CornerRadiusConverter : JsonConverter<CornerRadius>
{
    public override bool CanRead => true;
    public override bool CanWrite => true;

    public override CornerRadius ReadJson(JsonReader reader, global::System.Type objectType, CornerRadius existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);
        var topLeft = (double)(jsonObject[nameof(CornerRadius.TopLeft)] ?? 0);
        var topRight = (double)(jsonObject[nameof(CornerRadius.TopRight)] ?? 0);
        var bottomRight = (double)(jsonObject[nameof(CornerRadius.BottomRight)] ?? 0);
        var bottomLeft = (double)(jsonObject[nameof(CornerRadius.BottomLeft)] ?? 0);

        return new CornerRadius(topLeft, topRight, bottomRight, bottomLeft);
    }

    public override void WriteJson(JsonWriter writer, CornerRadius value, JsonSerializer serializer)
    {
        var jsonObject = new JObject
        {
            { nameof(CornerRadius.TopLeft), value.TopLeft },
            { nameof(CornerRadius.TopRight), value.TopRight },
            { nameof(CornerRadius.BottomRight), value.BottomRight },
            { nameof(CornerRadius.BottomLeft), value.BottomLeft }
        };

        jsonObject.WriteTo(writer);
    }
}