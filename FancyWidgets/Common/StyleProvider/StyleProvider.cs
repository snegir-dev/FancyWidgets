using System.Reflection;
using FancyWidgets.Common.Convertors;
using FancyWidgets.Common.StyleProvider.Attributes;
using FancyWidgets.Common.StyleProvider.Models;
using FancyWidgets.Models;
using Style = FancyWidgets.Common.StyleProvider.Models.Style;

namespace FancyWidgets.Common.StyleProvider;

public class StyleProvider : IStyleProvider
{
    private readonly JsonFileManager _jsonFileManager = new();
    private readonly object _editableObject;

    public StyleProvider(object editableObject, bool isStyleRegenerate = false)
    {
        _editableObject = editableObject;
        if (isStyleRegenerate)
            InitializeJsonStyle();
    }

    public void LoadStyles()
    {
        var styles = _jsonFileManager.GetModelFromJson<RootObject>(AppSettings.StylesFile);
        var classAttribute = _editableObject.GetType().GetCustomAttribute<ChangeableObjectAttribute>()!;
        var propertyInfos = _editableObject.GetType().GetProperties()
            .Where(p => p.GetCustomAttribute<ChangeablePropertyAttribute>() != null).ToList();

        var page = styles.Pages.First(p => p.Name == classAttribute.Page);
        foreach (var section in page.Sections)
        {
            foreach (var style in section.Styles)
            {
                var property = propertyInfos.First(p => p.Name == style.Name);
                var destinationType = Type.GetType(style.DataType)!;
                var type = CustomConvert.ChangeType(style.Value, destinationType);
                property.SetValue(_editableObject, type);
            }
        }
    }

    private void InitializeJsonStyle()
    {
        var root = GenerateRootObject();

        if (root.Pages.Count <= 0)
            _jsonFileManager.SaveJsonFile(root, AppSettings.StylesFile);
    }

    private RootObject GenerateRootObject()
    {
        var pages = new List<Page>();
        foreach (var @class in GetChangeableClasses())
        {
            var classAttribute = @class.GetCustomAttribute<ChangeableObjectAttribute>()!;
            var sections = CreateSections(@class).ToList();

            var page = new Page
            {
                Name = classAttribute.Page,
                Sections = sections
            };

            pages.Add(page);
        }

        var root = new RootObject()
        {
            Pages = pages
        };

        root.MergePagesWithSameName();

        foreach (var page in root.Pages)
        {
            page.MergeSectionsWithSameName();
        }

        return root;
    }

    private IEnumerable<Section> CreateSections(Type @class)
    {
        var properties = @class.GetProperties()
            .Where(p => p.GetCustomAttribute<ChangeablePropertyAttribute>() != null);

        var sections = new List<Section>();
        foreach (var propertyInfo in properties)
        {
            var propertyAttribute = propertyInfo.GetCustomAttribute<ChangeablePropertyAttribute>()!;
            var value = propertyInfo.GetValue(_editableObject);
            var style = new Style
            {
                Name = propertyAttribute.Name,
                Description = propertyAttribute.Description,
                DataType = $"{value?.GetType().FullName}, {value?.GetType().Assembly.FullName}",
                Value = propertyInfo.GetValue(_editableObject)
            };

            var section = new Section
            {
                Name = propertyAttribute.Section,
                Styles = { style }
            };

            sections.Add(section);
        }

        return sections;
    }

    private IEnumerable<Type> GetChangeableClasses()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        return assemblies.SelectMany(a => a.GetExportedTypes()
            .Where(c => c.GetCustomAttribute<ChangeableObjectAttribute>() != null));
    }
}