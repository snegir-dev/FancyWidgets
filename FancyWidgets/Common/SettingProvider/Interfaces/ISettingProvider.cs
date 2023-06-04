namespace FancyWidgets.Common.SettingProvider.Interfaces;

public interface ISettingProvider
{
    void InitializeSettings();
    void LoadSettings();
    void AddValue(string id, Type dataType, object value);
    void SetValue(string id, object? value);
    void SetValue(string fullNameClass, string propertyName, object? value);
    T? GetValue<T>(string id);
    T? GetValue<T>(string fullNameClass, string propertyName);
}