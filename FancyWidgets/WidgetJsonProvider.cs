using System.Reflection;
using Newtonsoft.Json;
using Path = System.IO.Path;

namespace FancyWidgets;

public class WidgetJsonProvider
{
    private string WorkDirectoryPath => GetWorkDirectoryPath();

    public void SaveModel(object model, string nameFile)
    {
        var filePath = Path.Combine(WorkDirectoryPath, nameFile);
        var jsonModel = JsonConvert.SerializeObject(model);
        File.WriteAllText(filePath, jsonModel);
    }

    public T GetModel<T>(string path) where T : new()
    {
        var json = GetStringJson(path);
        return JsonConvert.DeserializeObject<T>(json) ?? new T();
    }

    public void UpdateModel<T>(Action<T> updateAction, string path)
    {
        var json = GetStringJson(path);
        var model = JsonConvert.DeserializeObject<T>(json);

        if (model is null)
            return;

        updateAction(model);

        SaveModel(model, path);
    }

    private string GetStringJson(string path)
    {
        var filePath = Path.Combine(WorkDirectoryPath, path);
        if (!File.Exists(filePath))
            CreateJsonFile(filePath);

        return File.ReadAllText(filePath);
    }

    private void CreateJsonFile(string filePath)
    {
        File.WriteAllText(filePath, "");
    }

    private string GetWorkDirectoryPath()
    {
        var workDirectoryPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
        if (workDirectoryPath == null)
            throw new NullReferenceException("Failed to get the path to the widget assembly");

        return workDirectoryPath;
    }
}