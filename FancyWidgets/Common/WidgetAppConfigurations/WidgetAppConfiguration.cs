using FancyWidgets.Common.System.IO;
using FancyWidgets.Common.WidgetAppConfigurations.Interfaces;
using FancyWidgets.Models;
using Microsoft.Extensions.Configuration;

namespace FancyWidgets.Common.WidgetAppConfigurations;

public class WidgetAppConfiguration : IWidgetAppConfiguration
{
    public ConfigurationManager Configuration { get; }

    public WidgetAppConfiguration(ConfigurationManager configuration)
    {
        Configuration = configuration;
    }

    public void LoadConfig()
    {
        var metadata = Path.Combine(WidgetPath.WorkDirectoryPath, AppSettings.WidgetMetadataFile);
        Configuration.AddJsonFile(metadata);
    }
}