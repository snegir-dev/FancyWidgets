namespace FancyWidgets.Common.SettingProvider.Interfaces;

public interface ISettingsModifier
{
    void AddValue(string id, Type dataType, object value);
    void SetValue(string id, object? value);
    void SetValue(string fullNameClass, string propertyName, object? value);
}