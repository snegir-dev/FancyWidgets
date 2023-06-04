using Newtonsoft.Json;

namespace FancyWidgets.Common.SettingProvider.Models;

public class SettingElement
{
    [JsonProperty(nameof(Id))]
    public string? Id { get; set; }
    
    [JsonProperty(nameof(FullNameClass))]
    public string? FullNameClass { get; set; }
    
    [JsonProperty(nameof(Name))]
    public string Name { get; set; }

    [JsonProperty(nameof(DataType))] 
    public string DataType { get; set; }

    [JsonProperty(nameof(Value))] 
    public object? Value { get; set; }
}