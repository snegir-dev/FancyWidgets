using Newtonsoft.Json;

namespace FancyWidgets.Common.StyleProvider.Models;

public class RootObject
{
    [JsonProperty(nameof(Pages))] 
    public List<Page> Pages { get; set; } = new();
    
    public void MergePagesWithSameName()
    {
        var mergedPages = new List<Page>();
        var groupedPages = Pages.GroupBy(p => p.Name);

        foreach (var group in groupedPages)
        {
            var mergedPage = new Page { Name = group.Key };
            foreach (var page in group)
            {
                mergedPage.Sections.AddRange(page.Sections);
            }
            
            mergedPages.Add(mergedPage);
        }

        Pages = new HashSet<Page>(mergedPages).ToList();
    }
}