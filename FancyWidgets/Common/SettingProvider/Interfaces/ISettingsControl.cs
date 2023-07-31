using Avalonia.Controls;

namespace FancyWidgets.Common.SettingProvider.Interfaces;

public interface ISettingsControl
{
    public int Order { get; protected set; }
}