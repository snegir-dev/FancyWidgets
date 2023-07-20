using System.Reflection;
using FancyWidgets.Common.Convertors.Json;

namespace FancyWidgets.Common.System.IO;

public static class WidgetPath
{
    public static string WorkDirectoryPath => GetWorkDirectoryPath();
    
    private static string GetWorkDirectoryPath()
    {
        var executingAssembly = Assembly.GetAssembly(typeof(IWidgetJsonProvider));
        var workDirectoryPath = Path.GetDirectoryName(executingAssembly?.Location);
        if (workDirectoryPath == null)
            throw new NullReferenceException("Failed to get the path to the widget assembly.");

        return workDirectoryPath;
    }
}