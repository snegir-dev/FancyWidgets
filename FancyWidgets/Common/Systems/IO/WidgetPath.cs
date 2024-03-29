﻿using System.Reflection;
using FancyWidgets.Common.Convertors.Json;

namespace FancyWidgets.Common.Systems.IO;

public static class WidgetPath
{
    public static string WorkDirectoryPath => GetWorkDirectoryPath();
    
    private static string GetWorkDirectoryPath()
    {
        var executingAssembly = Assembly.GetCallingAssembly();
        var workDirectoryPath = Path.GetDirectoryName(executingAssembly?.Location);
        if (workDirectoryPath == null)
            throw new NullReferenceException("Failed to get the path to the widget assembly.");

        return workDirectoryPath;
    }
}