using System.Reflection;
using Autofac;
using FancyWidgets.Common.Convertors.Json;
using FancyWidgets.Common.Domain;
using FancyWidgets.Common.Locators;
using FancyWidgets.Common.SettingProvider.Attributes;
using FancyWidgets.Common.SettingProvider.Interfaces;
using FancyWidgets.Common.SettingProvider.Models;
using FancyWidgets.Models;
using Newtonsoft.Json;
using static FancyWidgets.Common.SettingProvider.ViewModelsContainer;

namespace FancyWidgets.Common.SettingProvider;

public class SettingsProvider : ISettingsProvider
{
    private const BindingFlags PropertyBindingFlags = BindingFlags.Instance
                                                      | BindingFlags.Public
                                                      | BindingFlags.NonPublic;

    protected readonly IWidgetJsonProvider WidgetJsonProvider;
    protected List<SettingsElement> SettingElements;

    public SettingsProvider(IWidgetJsonProvider widgetJsonProvider1)
    {
        WidgetJsonProvider = widgetJsonProvider1;
        SettingElements = WidgetJsonProvider.GetModel<List<SettingsElement>>(AppSettings.SettingsFile)
                          ?? new List<SettingsElement>();
    }

    public virtual void InitializeSettings()
    {
        var settingElements = GenerateSettings();
        WidgetJsonProvider.SaveModel(settingElements, AppSettings.SettingsFile);
    }

    public virtual void LoadSettings()
    {
        foreach (var currentViewModel in CurrentViewModels)
        {
            if (currentViewModel is null)
                throw new NullReferenceException();

            var propertyInfos = currentViewModel.GetType()
                .GetProperties(PropertyBindingFlags)
                .Where(p => p.GetCustomAttribute<ConfigurablePropertyAttribute>() != null).ToList();

            foreach (var settingElement in SettingElements)
            {
                if (settingElement.JValue is null)
                    continue;
                var property = propertyInfos
                    .FirstOrDefault(p => p.Name == settingElement.Name
                                         && p.DeclaringType?.FullName == settingElement.FullClassName);
                object? value = null;
                if (settingElement.DataType != null)
                {
                    var destinationType = Type.GetType(settingElement.DataType);
                    if (destinationType != null)
                        value = JsonConvert.DeserializeObject(settingElement.JValue, destinationType);
                }

                property?.SetValue(currentViewModel, value);
            }
        }
    }

    public virtual void AddOrUpdateValue(string id, object value)
    {
        var settingElement = SettingElements.FirstOrDefault(e => e.Id == id);
        if (settingElement == null)
        {
            settingElement = new SettingsElement
            {
                Id = id,
                DataType = value.GetType().AssemblyQualifiedName!,
                JValue = JsonConvert.SerializeObject(value)
            };
            SettingElements.Add(settingElement);
        }
        else
        {
            settingElement.DataType = value.GetType().AssemblyQualifiedName!;
            settingElement.JValue = JsonConvert.SerializeObject(value);
        }

        WidgetJsonProvider.SaveModel(SettingElements, AppSettings.SettingsFile);
    }

    public virtual void SetValue(string id, object? value)
    {
        var settingElement = SettingElements.FirstOrDefault(e => e.Id == id);
        if (settingElement is null)
            return;

        var property = GetObjectPropertyById(id);
        SetValue(property, settingElement, value);
    }

    public virtual void SetValue(string fullNameClass, string propertyName, object? value)
    {
        SettingElements = WidgetJsonProvider.GetModel<List<SettingsElement>>(AppSettings.SettingsFile)
                          ?? new List<SettingsElement>();
        var settingElement =
            SettingElements.FirstOrDefault(e => e.FullClassName == fullNameClass
                                                && e.Name == propertyName);

        if (settingElement is null)
            return;

        var property = GetObjectPropertyByNamespaceAndName(fullNameClass, propertyName);
        SetValue(property, settingElement, value);
    }

    public virtual T? GetValue<T>(string id)
    {
        var value = SettingElements.FirstOrDefault(e => e.Id == id)?.JValue;
        if (string.IsNullOrWhiteSpace(value))
            return default;
        return JsonConvert.DeserializeObject<T>(value);
    }

    public virtual T? GetValue<T>(string fullNameClass, string propertyName)
    {
        var value = SettingElements.FirstOrDefault(e => e.FullClassName == fullNameClass
                                                        && e.Name == propertyName)?.JValue;
        if (string.IsNullOrWhiteSpace(value))
            return default;
        return JsonConvert.DeserializeObject<T>(value);
    }

    protected virtual void SetValue(PropertyInfo? property, SettingsElement settingsElement, object? value)
    {
        foreach (var currentViewModel in CurrentViewModels)
        {
            if (currentViewModel is not null
                && !currentViewModel.GetType().GetProperties(PropertyBindingFlags).Contains(property))
                continue;

            property?.SetValue(currentViewModel, value);
            settingsElement.DataType = property?.PropertyType.AssemblyQualifiedName!;
            settingsElement.JValue = JsonConvert.SerializeObject(value);
            WidgetJsonProvider.SaveModel(SettingElements, AppSettings.SettingsFile);
        }
    }

    protected virtual PropertyInfo? GetObjectPropertyById(string? id)
    {
        foreach (var currentViewModel in CurrentViewModels)
        {
            if (currentViewModel is null || id is null)
                continue;

            var typeEditableObject = currentViewModel.GetType();
            var editableObjectProperties = typeEditableObject.GetProperties(PropertyBindingFlags);
            var property = editableObjectProperties.FirstOrDefault(p =>
            {
                var attribute = p.GetCustomAttribute<ConfigurablePropertyAttribute>();
                return attribute?.Id == id;
            });

            if (property == null)
                continue;
            return property;
        }

        return null;
    }

    protected virtual PropertyInfo? GetObjectPropertyByNamespaceAndName(string? fullNameClass, string? propertyName)
    {
        foreach (var currentViewModel in CurrentViewModels)
        {
            if (currentViewModel is null || fullNameClass is null || propertyName is null)
                continue;

            var typeEditableObject = currentViewModel.GetType();
            var editableObjectProperties = typeEditableObject.GetProperties(PropertyBindingFlags);
            var property = editableObjectProperties.FirstOrDefault(p =>
                p.DeclaringType?.FullName == fullNameClass && p.Name == propertyName);

            if (property == null)
                continue;
            return property;
        }

        return null;
    }

    protected virtual IEnumerable<SettingsElement> GenerateSettings()
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