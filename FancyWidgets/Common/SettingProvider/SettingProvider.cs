using System.Reflection;
using FancyWidgets.Common.Convertors;
using FancyWidgets.Common.SettingProvider.Attributes;
using FancyWidgets.Common.SettingProvider.Interfaces;
using FancyWidgets.Common.SettingProvider.Models;
using FancyWidgets.Models;
using ReactiveUI;
using Splat;

namespace FancyWidgets.Common.SettingProvider;

internal class SettingProvider : ISettingProvider
{
    private readonly WidgetJsonProvider _widgetJsonProvider = new();
    private readonly object _editableObject;
    private readonly List<SettingElement> _settingElements;

    public SettingProvider(object editableObject)
    {
        _editableObject = editableObject;
        _settingElements = _widgetJsonProvider.GetModel<List<SettingElement>>(AppSettings.SettingFile);
    }

    public SettingProvider()
    {
        var editableObject = Locator.Current.GetService<IScreen>();
        if (editableObject is not null)
        {
            _editableObject = editableObject;
            _settingElements = _widgetJsonProvider.GetModel<List<SettingElement>>(AppSettings.SettingFile);
        }
        else
        {
            throw new NotImplementedException("To use SettingProvider, the model must implement the IScreen interface");
        }
    }

    public void InitializeSettings()
    {
        var settingElements = GenerateSettings();
        _widgetJsonProvider.SaveModel(settingElements, AppSettings.SettingFile);
    }

    public void LoadSettings()
    {
        var propertyInfos = _editableObject.GetType().GetProperties()
            .Where(p => p.GetCustomAttribute<ConfigurablePropertyAttribute>() != null).ToList();

        foreach (var settingElement in _settingElements)
        {
            if (settingElement.Value is null)
                continue;
            var property = propertyInfos
                .FirstOrDefault(p => p.Name == settingElement.Name
                                     && p.DeclaringType?.FullName == settingElement.FullNameClass);
            var destinationType = Type.GetType(settingElement.DataType)!;
            var type = CustomConvert.ChangeType(settingElement.Value, destinationType);
            property?.SetValue(_editableObject, type);
        }
    }

    public void AddValue(string id, Type dataType, object value)
    {
        var settingElement = new SettingElement()
        {
            Id = id,
            DataType = $"{dataType.FullName}, {dataType.Assembly.FullName}",
            Value = value
        };
        _settingElements.Add(settingElement);
        _widgetJsonProvider.SaveModel(_settingElements, AppSettings.SettingFile);
    }

    public void SetValue(string id, object? value)
    {
        var settingElement = _settingElements.FirstOrDefault(e => e.Id == id);
        if (settingElement is null)
            return;

        var property = GetEditableObjectPropertyById(id);
        SetValue(property, settingElement, value);
    }

    public void SetValue(string fullNameClass, string propertyName, object? value)
    {
        var settingElements = _widgetJsonProvider.GetModel<List<SettingElement>>(AppSettings.SettingFile);
        var settingElement =
            settingElements.FirstOrDefault(e => e.FullNameClass == fullNameClass
                                                && e.Name == propertyName);

        if (settingElement is null)
            return;

        var property = GetEditableObjectPropertyByNamespaceAndName(fullNameClass, propertyName);
        SetValue(property, settingElement, value);
    }

    public T? GetValue<T>(string id)
    {
        var value = _settingElements.First(e => e.Id == id).Value;
        return (T?)CustomConvert.ChangeType(value, typeof(T));
    }

    public T? GetValue<T>(string fullNameClass, string propertyName)
    {
        var value = _settingElements.First(e => e.FullNameClass == fullNameClass
                                                && e.Name == propertyName).Value;
        return (T?)CustomConvert.ChangeType(value, typeof(T));
    }

    private void SetValue(PropertyInfo? property, SettingElement settingElement, object? value)
    {
        property?.SetValue(_editableObject, value);
        settingElement.Value = value;
        _widgetJsonProvider.SaveModel(_settingElements, AppSettings.SettingFile);
    }

    private PropertyInfo? GetEditableObjectPropertyById(string id)
    {
        var typeEditableObject = _editableObject.GetType();
        var editableObjectProperties = typeEditableObject.GetProperties();

        return editableObjectProperties.FirstOrDefault(p =>
        {
            var attribute = p.GetCustomAttribute<ConfigurablePropertyAttribute>();
            return attribute?.Id == id;
        });
    }

    private PropertyInfo? GetEditableObjectPropertyByNamespaceAndName(string fullNameClass, string propertyName)
    {
        var typeEditableObject = _editableObject.GetType();
        var editableObjectProperties = typeEditableObject.GetProperties();

        return editableObjectProperties.FirstOrDefault(p =>
            p.PropertyType.FullName == fullNameClass && p.Name == propertyName);
    }

    private static IEnumerable<SettingElement> GenerateSettings()
    {
        var classes = GetChangeableClasses();
        var properties = classes
            .SelectMany(c => c.GetProperties())
            .Where(p => p.GetCustomAttribute<ConfigurablePropertyAttribute>() != null);

        var settingElements = new List<SettingElement>();
        foreach (var property in properties)
        {
            if (property.DeclaringType is null)
                continue;

            var editableObject = Activator.CreateInstance(property.DeclaringType);
            var propertyValue = property.GetValue(editableObject);
            var settingElement = new SettingElement
            {
                Id = property.GetCustomAttribute<ConfigurablePropertyAttribute>()?.Id,
                FullNameClass = property.DeclaringType?.FullName,
                Name = property.Name,
                DataType = $"{propertyValue?.GetType().FullName}, {propertyValue?.GetType().Assembly.FullName}",
                Value = propertyValue
            };

            settingElements.Add(settingElement);
        }

        return settingElements;
    }

    private static IEnumerable<Type> GetChangeableClasses()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        return assemblies.SelectMany(a => a.GetExportedTypes()
            .Where(c => c.GetCustomAttribute<ConfigurableObjectAttribute>() != null));
    }
}