namespace FancyWidgets.Common.StyleProvider.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ChangeableObjectAttribute : Attribute
{
    public string Page { get; set; }

    public ChangeableObjectAttribute(string page)
    {
        Page = page;
    }
}