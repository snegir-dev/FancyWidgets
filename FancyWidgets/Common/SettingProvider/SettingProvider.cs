using System.Reflection;
using FancyWidgets.Common.Convertors;
using FancyWidgets.Common.SettingProvider.Attributes;
using FancyWidgets.Common.SettingProvider.Interfaces;
using FancyWidgets.Common.SettingProvider.Models;
using FancyWidgets.Models;
using ReactiveUI;
using Splat;

namespace FancyWidgets.Common.SettingProvider;

public class SettingProvider : ISettingProvider
{
    private static readonly JsonFileManager JsonFileManager = new();
    private readonly object _editableObject;
    private readonly List<SettingElement> _settingElements;

    public SettingProvider(object editableObject)
    {
        _editableObject = editableObject;
        _settingElements = JsonFileManager.GetModelFromJson<List<SettingElement>>(AppSettings.SettingFile);
    }

    public SettingProvider()
    {
        var editableObject = Locator.Current.GetService<IScreen>();
        if (editableObject != null)
        {
            _editableObject = editableObject;
            _settingElements = JsonFileManager.GetModelFromJson<List<SettingElement>>(AppSettings.SettingFile);
        }
        else
        {
            throw new NotImplementedException("To use SettingProvider, the model must implement the IScreen interface");
        }
    }

    public void LoadSettings()
    {
        var propertyInfos = _editableObject.GetType().GetProperties()
            .Where(p => p.GetCustomAttribute<ChangeablePropertyAttribute>() != null).ToList();

        foreach (var settingElement in _settingElements)
        {
            if (settingElement.Value == null)
                continue;
            var property = propertyInfos.First(p => p.Name == settingElement.Name
                                                    && p.DeclaringType?.Namespace == settingElement.Namespace);
            var destinationType = Type.GetType(settingElement.DataType)!;
            var type = CustomConvert.ChangeType(settingElement.Value, destinationType);
            property.SetValue(_editableObject, type);
        }
    }

    public void SetValue(string id, object? value)
    {
        var settingElement = _settingElements.FirstOrDefault(e => e.Id == id);

        if (settingElement == null)
            return;

        var property = GetPropertyById(id);
        SetValue(property, settingElement, value);
    }

    public void SetValue(string namespaceName, string propertyName, object? value)
    {
        var settingElements = JsonFileManager.GetModelFromJson<List<SettingElement>>(AppSettings.SettingFile);
        var settingElement =
            settingElements.FirstOrDefault(e => e.Namespace == namespaceName && e.Name == propertyName);

        if (settingElement == null)
            return;

        var property = GetPropertyByNamespaceAndName(namespaceName, propertyName);
        SetValue(property, settingElement, value);
    }

    public static void InitializeSettings()
    {
        var settingElements = GenerateSettings();
        JsonFileManager.SaveJsonFile(settingElements, AppSettings.SettingFile);
    }

    private void SetValue(PropertyInfo? property, SettingElement settingElement, object? value)
    {
        if (property != null)
        {
            var destinationType = Type.GetType(settingElement.DataType);
            if (destinationType != null)
            {
                var convertedValue = CustomConvert.ChangeType(value, destinationType);
                property.SetValue(_editableObject, convertedValue);
            }
        }

        settingElement.Value = value;
        JsonFileManager.SaveJsonFile(_settingElements, AppSettings.SettingFile);
    }

    private PropertyInfo? GetPropertyById(string id)
    {
        var typeEditableObject = _editableObject.GetType();
        var editableObjectProperties = typeEditableObject.GetProperties();

        return editableObjectProperties.FirstOrDefault(p =>
        {
            var attribute = p.GetCustomAttribute<ChangeablePropertyAttribute>();
            return attribute?.Id == id;
        });
    }

    private PropertyInfo? GetPropertyByNamespaceAndName(string namespaceName, string propertyName)
    {
        var typeEditableObject = _editableObject.GetType();
        var editableObjectProperties = typeEditableObject.GetProperties();

        return editableObjectProperties.FirstOrDefault(p =>
            p.PropertyType.Namespace == namespaceName && p.Name == propertyName);
    }

    private static IEnumerable<SettingElement> GenerateSettings()
    {
        var classes = GetChangeableClasses();
        var properties = classes
            .SelectMany(c => c.GetProperties())
            .Where(p => p.GetCustomAttribute<ChangeablePropertyAttribute>() != null);

        var settingElements = new List<SettingElement>();
        foreach (var property in properties)
        {
            if (property.DeclaringType == null)
                continue;

            var editableObject = Activator.CreateInstance(property.DeclaringType);
            var propertyValue = property.GetValue(editableObject);
            var settingElement = new SettingElement
            {
                Id = property.GetCustomAttribute<ChangeablePropertyAttribute>()?.Id,
                Namespace = property.DeclaringType?.Namespace,
                Name = property.Name,
                DataType = $"{propertyValue?.GetType().FullName}, {propertyValue?.GetType().Assembly.FullName}"
            };

            settingElements.Add(settingElement);
        }

        return settingElements;
    }

    private static IEnumerable<Type> GetChangeableClasses()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        return assemblies.SelectMany(a => a.GetExportedTypes()
            .Where(c => c.GetCustomAttribute<ChangeableObjectAttribute>() != null));
    }
}