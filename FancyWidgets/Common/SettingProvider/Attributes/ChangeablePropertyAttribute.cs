namespace FancyWidgets.Common.SettingProvider.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ChangeablePropertyAttribute : Attribute
{
    public string? Id { get; }

    public ChangeablePropertyAttribute(string? id = null)
    {
        Id = id;
    }
}