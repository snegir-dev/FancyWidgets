using System.Reflection;
using FancyWidgets.Common.SettingProvider.Models;

namespace FancyWidgets.Common.SettingProvider.Interfaces;

public interface ISettingElementOperations
{
    IEnumerable<SettingsElement> GenerateSettings();
    IEnumerable<SettingsElement> GetCustomSettingsElements();

    PropertyInfo? GetPropertyInfo(IEnumerable<PropertyInfo> propertyInfos,
        SettingsElement settingsElement);

    void UpdateValueAndTypeSettingsElement(SettingsElement settingsElements,
        PropertyInfo? propertyInfo);

    void RemoveObsoleteSettingsElements(List<SettingsElement> settingsElements);

    SettingsElement AddOrUpdateValue(PropertyInfo property, SettingsElement settingsElement);
}