namespace FancyWidgets.Common.SettingProvider.Interfaces;

public interface ISettingsModifier
{
    void Add(string id, object value);
    void AddOrUpdateValue(string id, object value);
    void SetValue(string id, object? value);
    void SetValue(string fullNameClass, string propertyName, object? value);
    void Remove(string id);
    void Remove(string fullNameClass, string propertyName);
}