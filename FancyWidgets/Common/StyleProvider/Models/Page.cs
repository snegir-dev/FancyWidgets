using Newtonsoft.Json;

namespace FancyWidgets.Common.StyleProvider.Models;

public class Page
{
    [JsonProperty(nameof(Name))]
    public string Name { get; set; }

    [JsonProperty(nameof(Sections))] 
    public List<Section> Sections { get; set; } = new();
    
    public void MergeSectionsWithSameName()
    {
        var mergedSections = new List<Section>();
        var groupedSections = Sections.GroupBy(s => s.Name);
        
        foreach (var group in groupedSections)
        {
            var mergedSection = new Section { Name = group.Key };
            foreach (var section in group)
            {
                mergedSection.Styles.AddRange(section.Styles);
            }
            mergedSections.Add(mergedSection);
        }
        
        Sections = new HashSet<Section>(mergedSections).ToList();
    }
}