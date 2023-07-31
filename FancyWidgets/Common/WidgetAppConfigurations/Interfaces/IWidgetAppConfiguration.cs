using Microsoft.Extensions.Configuration;

namespace FancyWidgets.Common.WidgetAppConfigurations.Interfaces;

public interface IWidgetAppConfiguration
{
    ConfigurationManager Configuration { get; }
    void LoadConfig();
}