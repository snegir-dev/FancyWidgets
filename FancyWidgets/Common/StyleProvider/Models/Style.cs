using Newtonsoft.Json;

namespace FancyWidgets.Common.StyleProvider.Models;

public class Style
{
    [JsonProperty(nameof(Name))]
    public string Name { get; set; }

    [JsonProperty(nameof(Description))] 
    public string? Description { get; set; }

    [JsonProperty(nameof(DataType))] 
    public string DataType { get; set; }

    [JsonProperty(nameof(Value))] 
    public object? Value { get; set; }
}