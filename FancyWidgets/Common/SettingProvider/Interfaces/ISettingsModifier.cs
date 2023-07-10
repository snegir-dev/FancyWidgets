namespace FancyWidgets.Common.SettingProvider.Interfaces;

public interface ISettingsModifier
{
    void AddOrUpdateValue(string id, object value);
    void SetValue(string id, object? value);
    void SetValue(string fullNameClass, string propertyName, object? value);
}