using System.Reflection;
using Autofac;
using FancyWidgets.Common.Convertors.Json;
using FancyWidgets.Common.Convertors.Type;
using FancyWidgets.Common.Domain;
using FancyWidgets.Common.Locators;
using FancyWidgets.Common.SettingProvider.Attributes;
using FancyWidgets.Common.SettingProvider.Interfaces;
using FancyWidgets.Common.SettingProvider.Models;
using FancyWidgets.Models;
using static FancyWidgets.Common.SettingProvider.ViewModelsContainer;

namespace FancyWidgets.Common.SettingProvider;

public class SettingsProvider : ISettingsProvider
{
    private readonly IWidgetJsonProvider _widgetJsonProvider;
    private List<SettingsElement> _settingElements;

    public SettingsProvider(IWidgetJsonProvider widgetJsonProvider1)
    {
        _widgetJsonProvider = widgetJsonProvider1;
        _settingElements = _widgetJsonProvider.GetModel<List<SettingsElement>>(AppSettings.SettingsFile);
    }

    public virtual void InitializeSettings()
    {
        var settingElements = GenerateSettings();
        _widgetJsonProvider.SaveModel(settingElements, AppSettings.SettingsFile);
    }

    public virtual void LoadSettings()
    {
        foreach (var currentViewModel in CurrentViewModels)
        {
            if (currentViewModel is null)
                throw new NullReferenceException();

            var propertyInfos = currentViewModel.GetType().GetProperties()
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
                property?.SetValue(currentViewModel, type);
            }
        }
    }

    public virtual void AddValue(string id, Type dataType, object value)
    {
        var settingElement = _settingElements.FirstOrDefault(e => e.Id == id);
        if (settingElement == null)
        {
            settingElement = new SettingsElement()
            {
                Id = id,
                DataType = dataType.AssemblyQualifiedName!,
                Value = value
            };
            _settingElements.Add(settingElement);
        }
        else
        {
            settingElement.DataType = dataType.AssemblyQualifiedName!;
            settingElement.Value = value;
        }

        _widgetJsonProvider.SaveModel(_settingElements, AppSettings.SettingsFile);
    }

    public virtual void SetValue(string id, object? value)
    {
        var settingElement = _settingElements.FirstOrDefault(e => e.Id == id);
        if (settingElement is null)
            return;

        var property = GetEditableObjectPropertyById(id);
        SetValue(property, settingElement, value);
    }

    public virtual void SetValue(string fullNameClass, string propertyName, object? value)
    {
        var settingElements = _widgetJsonProvider.GetModel<List<SettingsElement>>(AppSettings.SettingsFile);
        var settingElement =
            settingElements.FirstOrDefault(e => e.FullNameClass == fullNameClass
                                                && e.Name == propertyName);

        if (settingElement is null)
            return;

        var property = GetEditableObjectPropertyByNamespaceAndName(fullNameClass, propertyName);
        SetValue(property, settingElement, value);
    }

    public virtual T? GetValue<T>(string id)
    {
        var value = _settingElements.First(e => e.Id == id).Value;
        return (T?)CustomConvert.ChangeType(value, typeof(T));
    }

    public virtual T? GetValue<T>(string fullNameClass, string propertyName)
    {
        var value = _settingElements.First(e => e.FullNameClass == fullNameClass
                                                && e.Name == propertyName).Value;
        return (T?)CustomConvert.ChangeType(value, typeof(T));
    }

    private void SetValue(PropertyInfo? property, SettingsElement settingsElement, object? value)
    {
        foreach (var currentViewModel in CurrentViewModels)
        {
            var t = currentViewModel?.GetType().GetProperties();

            if (currentViewModel is not null
                && !currentViewModel.GetType().GetProperties().Contains(property))
                continue;

            property?.SetValue(currentViewModel, value);
            settingsElement.Value = value;
            _widgetJsonProvider.SaveModel(_settingElements, AppSettings.SettingsFile);
        }
    }

    private PropertyInfo? GetEditableObjectPropertyById(string? id)
    {
        foreach (var currentViewModel in CurrentViewModels)
        {
            if (currentViewModel is null || id is null)
                continue;

            var typeEditableObject = currentViewModel.GetType();
            var editableObjectProperties = typeEditableObject.GetProperties();
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

    private PropertyInfo? GetEditableObjectPropertyByNamespaceAndName(string? fullNameClass, string? propertyName)
    {
        foreach (var currentViewModel in CurrentViewModels)
        {
            if (currentViewModel is null || fullNameClass is null || propertyName is null)
                continue;

            var typeEditableObject = currentViewModel.GetType();
            var editableObjectProperties = typeEditableObject.GetProperties();
            var property = editableObjectProperties.FirstOrDefault(p =>
                p.PropertyType.FullName == fullNameClass && p.Name == propertyName);

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
            .SelectMany(c => c.GetProperties())
            .Where(p => p.GetCustomAttribute<ConfigurablePropertyAttribute>() != null)
            .ToList();

        var customSettingsElements = GetCustomSettingsElements().ToList();
        var currentSettingElements = GetPreviewSettingsElements(properties).ToList();

        foreach (var settingsElement in currentSettingElements)
            UpdateSettingsElement(settingsElement, properties);
        RemoveObsoleteSettingsElements(currentSettingElements);

        _settingElements = _settingElements
            .Union(customSettingsElements)
            .ToList();

        return _settingElements;
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
                FullNameClass = property.DeclaringType?.FullName,
                Name = property.Name
            };

            currentSettingElements.Add(settingElement);
        }

        return currentSettingElements;
    }

    protected virtual IEnumerable<SettingsElement> GetCustomSettingsElements()
    {
        return _settingElements.Where(e => e.FullNameClass == null && e.Name == null);
    }

    protected static PropertyInfo? GetPropertyInfo(IEnumerable<PropertyInfo> propertyInfos,
        SettingsElement settingsElement)
    {
        foreach (var propertyInfo in propertyInfos)
        {
            var attribute = propertyInfo.GetCustomAttribute<ConfigurablePropertyAttribute>();
            if (attribute?.Id == settingsElement.Id)
            {
                return propertyInfo;
            }

            if (propertyInfo.DeclaringType?.FullName == settingsElement.FullNameClass
                && propertyInfo.Name == settingsElement.Name)
            {
                return propertyInfo;
            }
        }

        return null;
    }

    protected virtual void UpdateSettingsElement(SettingsElement settingsElements,
        IList<PropertyInfo> properties)
    {
        if (!_settingElements.Exists(e => e.Id == settingsElements.Id
                                          && e.FullNameClass == settingsElements.FullNameClass
                                          && e.Name == settingsElements.Name))
        {
            var property = GetPropertyInfo(properties, settingsElements);
            if (property == null)
                return;

            var newSettingElement = AddValue(property, settingsElements);
            _settingElements.Add(newSettingElement);
        }
    }

    protected virtual void RemoveObsoleteSettingsElements(List<SettingsElement> settingsElements)
    {
        for (var i = 0; i < _settingElements.Count; i++)
        {
            if (!settingsElements.Exists(e => e.Id == _settingElements[i].Id
                                              && e.FullNameClass == _settingElements[i].FullNameClass
                                              && e.Name == _settingElements[i].Name))
            {
                _settingElements.Remove(_settingElements[i]);
            }
        }
    }

    protected static SettingsElement AddValue(PropertyInfo property, SettingsElement settingsElement)
    {
        var declaringType = property.DeclaringType;
        if (declaringType == null)
            return settingsElement;

        var editableObject = WidgetLocator.Current.ResolveOptional(declaringType)
                             ?? Activator.CreateInstance(declaringType);
        var propertyValue = property.GetValue(editableObject);
        settingsElement.DataType = $"{propertyValue?.GetType().FullName}, " +
                                   $"{propertyValue?.GetType().Assembly.FullName}";
        settingsElement.Value = propertyValue;

        return settingsElement;
    }

    protected static IEnumerable<Type> GetChangeableClasses()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        return assemblies.SelectMany(a => a.GetExportedTypes()
            .Where(c => c.IsAssignableTo<WidgetReactiveObject>()));
    }
}