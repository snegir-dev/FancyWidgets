﻿using System.Reflection;
using FancyWidgets.Common.Convertors.Json;
using FancyWidgets.Common.Exceptions;
using FancyWidgets.Common.SettingProvider.Attributes;
using FancyWidgets.Common.SettingProvider.Interfaces;
using FancyWidgets.Common.SettingProvider.Models;
using FancyWidgets.Models;
using Newtonsoft.Json;
using static FancyWidgets.Common.Models.WidgetReactiveObject.ReactiveObjectDataStatusContainer;

namespace FancyWidgets.Common.SettingProvider;

public class SettingsProvider : ISettingsProvider
{
    protected readonly IWidgetJsonProvider WidgetJsonProvider;
    protected List<SettingsElement> SettingElements;
    protected readonly ISettingElementOperations Operations;

    public SettingsProvider(IWidgetJsonProvider widgetJsonProvider,
        ISettingElementOperations settingElementOperations,
        List<SettingsElement> settingsElements)
    {
        WidgetJsonProvider = widgetJsonProvider;
        SettingElements = settingsElements;
        Operations = settingElementOperations;
    }

    public virtual void InitializeSettings()
    {
        var settingElements = Operations.GenerateSettings();
        WidgetJsonProvider.SaveModel(settingElements, AppSettings.SettingsFile);
    }

    public virtual void LoadSettings()
    {
        foreach (var currentObjectDataStatus in GetDataStatus())
        {
            if (currentObjectDataStatus == null || currentObjectDataStatus.IsDataLoaded)
                continue;

            var propertyInfos = currentObjectDataStatus.WidgetReactiveObject.GetType()
                .GetProperties(SettingElementOperations.PropertyBindingFlags)
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
                    var destinationType = AppDomain.CurrentDomain.GetAssemblies()
                        .Select(a => a.GetType(settingElement.DataType))
                        .FirstOrDefault(t => t != null);
                    if (destinationType != null)
                        value = JsonConvert.DeserializeObject(settingElement.JValue, destinationType);
                }

                property?.SetValue(currentObjectDataStatus.WidgetReactiveObject, value);
                currentObjectDataStatus.IsDataLoaded = true;
            }
        }
    }

    public virtual void Add(string id, object value)
    {
        var settingElement = SettingElements.FirstOrDefault(e => e.Id == id);
        if (settingElement == null)
        {
            settingElement = new SettingsElement
            {
                Id = id,
                DataType = value.GetType().FullName!,
                JValue = JsonConvert.SerializeObject(value)
            };
            SettingElements.Add(settingElement);
            WidgetJsonProvider.SaveModel(SettingElements, AppSettings.SettingsFile);
        }
        else
        {
            throw new InvalidOperationException($"A record with this id - {id} already exists");
        }
    }

    public virtual void AddOrUpdateValue(string id, object value)
    {
        try
        {
            Add(id, value);
        }
        catch (InvalidOperationException)
        {
            SetValue(id, value);
        }
    }

    public virtual void Remove(string id)
    {
        var settingElement = SettingElements.FirstOrDefault(e => e.Id == id);
        if (settingElement != null)
        {
            SettingElements.Remove(settingElement);
            WidgetJsonProvider.SaveModel(SettingElements, AppSettings.SettingsFile);
            return;
        }

        throw NotFoundException.ThrowNotFoundException(id);
    }

    public virtual void Remove(string fullNameClass, string propertyName)
    {
        var settingElement = SettingElements.FirstOrDefault(e => e.FullClassName == fullNameClass
                                                                 && e.Name == propertyName);
        if (settingElement != null)
        {
            SettingElements.Remove(settingElement);
            WidgetJsonProvider.SaveModel(SettingElements, AppSettings.SettingsFile);
            return;
        }

        throw NotFoundException.ThrowNotFoundException(fullNameClass, propertyName);
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
            throw NotFoundException.ThrowNotFoundException(id);
        return JsonConvert.DeserializeObject<T>(value);
    }

    public virtual T? GetValue<T>(string fullNameClass, string propertyName)
    {
        var value = SettingElements.FirstOrDefault(e => e.FullClassName == fullNameClass
                                                        && e.Name == propertyName)?.JValue;
        if (string.IsNullOrWhiteSpace(value))
            throw NotFoundException.ThrowNotFoundException(fullNameClass, propertyName);
        return JsonConvert.DeserializeObject<T>(value);
    }

    public virtual bool TryGetValue<T>(string id, out T? result)
    {
        var value = SettingElements.FirstOrDefault(e => e.Id == id)?.JValue;
        if (string.IsNullOrWhiteSpace(value))
        {
            result = default;
            return false;
        }

        result = JsonConvert.DeserializeObject<T>(value);
        return true;
    }

    public virtual bool TryGetValue<T>(string fullNameClass, string propertyName, out T? result)
    {
        var value = SettingElements.FirstOrDefault(e => e.FullClassName == fullNameClass
                                                        && e.Name == propertyName)?.JValue;
        if (string.IsNullOrWhiteSpace(value))
        {
            result = default;
            return false;
        }

        result = JsonConvert.DeserializeObject<T>(value);
        return true;
    }

    protected virtual void SetValue(PropertyInfo? property, SettingsElement settingsElement, object? value)
    {
        foreach (var currentObjectDataStatus in GetDataStatus())
        {
            if (currentObjectDataStatus is null
                || !currentObjectDataStatus.WidgetReactiveObject.GetType()
                    .GetProperties(SettingElementOperations.PropertyBindingFlags).Contains(property))
                continue;

            property?.SetValue(currentObjectDataStatus.WidgetReactiveObject, value);
            settingsElement.DataType = property?.PropertyType.FullName!;
            settingsElement.JValue = JsonConvert.SerializeObject(value);
            WidgetJsonProvider.SaveModel(SettingElements, AppSettings.SettingsFile);
        }
    }

    protected virtual PropertyInfo? GetObjectPropertyById(string? id)
    {
        foreach (var currentObjectDataStatus in GetDataStatus())
        {
            if (currentObjectDataStatus is null || id is null)
                continue;

            var typeEditableObject = currentObjectDataStatus.WidgetReactiveObject.GetType();
            var editableObjectProperties
                = typeEditableObject.GetProperties(SettingElementOperations.PropertyBindingFlags);
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
        foreach (var currentObjectDataStatus in GetDataStatus())
        {
            if (currentObjectDataStatus is null || fullNameClass is null || propertyName is null)
                continue;

            var typeEditableObject = currentObjectDataStatus.WidgetReactiveObject.GetType();
            var editableObjectProperties =
                typeEditableObject.GetProperties(SettingElementOperations.PropertyBindingFlags);
            var property = editableObjectProperties.FirstOrDefault(p =>
                p.DeclaringType?.FullName == fullNameClass && p.Name == propertyName);

            if (property == null)
                continue;
            return property;
        }

        return null;
    }
}