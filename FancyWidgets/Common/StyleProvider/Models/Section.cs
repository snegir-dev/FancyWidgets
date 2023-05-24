using Newtonsoft.Json;

namespace FancyWidgets.Common.StyleProvider.Models;

public class Section
{
    [JsonProperty(nameof(Name))]
    public string Name { get; set; }

    [JsonProperty(nameof(Styles))] 
    public List<Style> Styles { get; set; } = new();
}