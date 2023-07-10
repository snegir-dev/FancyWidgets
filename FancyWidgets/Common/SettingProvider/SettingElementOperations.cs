using System.Reflection;
using Autofac;
using FancyWidgets.Common.Domain;
using FancyWidgets.Common.Locators;
using FancyWidgets.Common.SettingProvider.Attributes;
using FancyWidgets.Common.SettingProvider.Models;
using Newtonsoft.Json;

namespace FancyWidgets.Common.SettingProvider;

public class SettingElementOperations
{
    public const BindingFlags PropertyBindingFlags = BindingFlags.Instance
                                                     | BindingFlags.Public
                                                     | BindingFlags.NonPublic;

    protected List<SettingsElement> SettingElements;

    public SettingElementOperations(List<SettingsElement> settingElements)
    {
        SettingElements = settingElements;
    }

    public virtual IEnumerable<SettingsElement> GenerateSettings()
    {
        var classes = GetChangeableClasses();
        var properties = classes
            .SelectMany(c => c.GetProperties(PropertyBindingFlags))
            .Where(p => p.GetCustomAttribute<ConfigurablePropertyAttribute>() != null)
            .ToList();

        var customSettingsElements = GetCustomSettingsElements().ToList();
        var currentSettingElements = GetPreviewSettingsElements(properties).ToList();

        foreach (var settingsElement in currentSettingElements)
        {
            var property = GetPropertyInfo(properties, settingsElement);
            UpdateValueAndTypeSettingsElement(settingsElement, property);
        }

        RemoveObsoleteSettingsElements(currentSettingElements);

        SettingElements = SettingElements
            .Union(customSettingsElements)
            .ToList();

        return SettingElements;
    }

    private IEnumerable<SettingsElement> GetPreviewSettingsElements(IEnumerable<PropertyInfo> properties)
    {
        var currentSettingElements = new List<SettingsElement>();
        foreach (var property in properties)
        {
            if (property.DeclaringType is null)
                continue;

            var settingElement = new SettingsElement
            {
                Id = property.GetCustomAttribute<ConfigurablePropertyAttribute>()?.Id,
                FullClassName = property.DeclaringType?.FullName,
                Name = property.Name
            };

            currentSettingElements.Add(settingElement);
        }

        return currentSettingElements;
    }

    protected virtual IEnumerable<SettingsElement> GetCustomSettingsElements()
    {
        return SettingElements.Where(e => e.Name == null && e.FullClassName == null);
    }

    protected PropertyInfo? GetPropertyInfo(IEnumerable<PropertyInfo> propertyInfos,
        SettingsElement settingsElement)
    {
        foreach (var propertyInfo in propertyInfos)
        {
            var attribute = propertyInfo.GetCustomAttribute<ConfigurablePropertyAttribute>();
            if (attribute?.Id != null && attribute.Id == settingsElement.Id)
            {
                return propertyInfo;
            }

            if (propertyInfo.DeclaringType?.FullName == settingsElement.FullClassName
                && propertyInfo.Name == settingsElement.Name)
            {
                return propertyInfo;
            }
        }

        return null;
    }

    protected virtual void UpdateValueAndTypeSettingsElement(SettingsElement settingsElements,
        PropertyInfo? propertyInfo)
    {
        if (!SettingElements.Exists(e => e.Id == settingsElements.Id
                                         && e.FullClassName == settingsElements.FullClassName
                                         && e.Name == settingsElements.Name))
        {
            if (propertyInfo == null)
                return;

            var newSettingElement = AddOrUpdateValue(propertyInfo, settingsElements);
            SettingElements.Add(newSettingElement);
        }
    }

    protected virtual void RemoveObsoleteSettingsElements(List<SettingsElement> settingsElements)
    {
        for (var i = 0; i < SettingElements.Count; i++)
        {
            if (!settingsElements.Exists(e => e.Id == SettingElements[i].Id
                                              && e.FullClassName == SettingElements[i].FullClassName
                                              && e.Name == SettingElements[i].Name))
            {
                SettingElements.Remove(SettingElements[i]);
            }
        }
    }

    protected virtual SettingsElement AddOrUpdateValue(PropertyInfo property, SettingsElement settingsElement)
    {
        var declaringType = property.DeclaringType;
        if (declaringType == null)
            return settingsElement;

        var editableObject = WidgetLocator.Current.ResolveOptional(declaringType)
                             ?? Activator.CreateInstance(declaringType);
        var propertyValue = property.GetValue(editableObject);
        settingsElement.DataType = propertyValue?.GetType().AssemblyQualifiedName!;
        settingsElement.JValue = JsonConvert.SerializeObject(propertyValue);

        return settingsElement;
    }

    protected static IEnumerable<Type> GetChangeableClasses()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        return assemblies.SelectMany(a => a.GetExportedTypes()
            .Where(c => c.IsAssignableTo<WidgetReactiveObject>()));
    }
}