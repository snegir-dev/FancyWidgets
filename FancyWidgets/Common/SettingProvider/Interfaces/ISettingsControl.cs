using Avalonia.Controls;
using Avalonia.Styling;

namespace FancyWidgets.Common.SettingProvider.Interfaces;

public interface ISettingsControl : IStyleable
{
    public IControl Control { get; }
    new Type StyleKey { get; }
}