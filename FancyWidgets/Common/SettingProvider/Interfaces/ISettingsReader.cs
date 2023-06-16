namespace FancyWidgets.Common.SettingProvider.Interfaces;

public interface ISettingsReader
{
    T? GetValue<T>(string id);
    T? GetValue<T>(string fullNameClass, string propertyName);
}