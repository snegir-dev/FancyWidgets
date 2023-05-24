namespace FancyWidgets.Common.StyleProvider.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ChangeablePropertyAttribute : Attribute
{
    public string Section { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }

    public ChangeablePropertyAttribute(string section, string name, string? description = null)
    {
        Section = section;
        Name = name;
        Description = description;
    }
}
