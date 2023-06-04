namespace FancyWidgets.Common.SettingProvider.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ConfigurablePropertyAttribute : Attribute
{
    public string? Id { get; }

    public ConfigurablePropertyAttribute(string? id = null) => Id = id;
}