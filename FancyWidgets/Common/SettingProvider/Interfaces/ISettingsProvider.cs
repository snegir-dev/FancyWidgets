namespace FancyWidgets.Common.SettingProvider.Interfaces;

public interface ISettingsProvider : ISettingsInitializer, ISettingsLoader, 
    ISettingsModifier, ISettingsReader
{
}