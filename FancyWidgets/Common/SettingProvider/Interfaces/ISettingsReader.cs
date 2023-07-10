namespace FancyWidgets.Common.SettingProvider.Interfaces;

public interface ISettingsReader
{
    T? GetValue<T>(string id);
    T? GetValue<T>(string fullNameClass, string propertyName);
    bool TryGetValue<T>(string id, out T? result);
    bool TryGetValue<T>(string fullNameClass, string propertyName, out T? result);
}