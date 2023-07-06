using Newtonsoft.Json;

namespace FancyWidgets.Common.SettingProvider.Models;

public class SettingsElement
{
    [JsonProperty(nameof(Id))] 
    public string? Id { get; set; }

    [JsonProperty(nameof(FullNameClass))] 
    public string? FullNameClass { get; set; }

    [JsonProperty(nameof(Name))] 
    public string? Name { get; set; }

    [JsonProperty(nameof(DataType))] 
    public string? DataType { get; set; }

    [JsonProperty(nameof(JValue))] 
    public string? JValue { get; set; }
}