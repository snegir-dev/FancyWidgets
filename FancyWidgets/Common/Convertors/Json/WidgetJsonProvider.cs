using FancyWidgets.Common.System.IO;
using Newtonsoft.Json;
using Path = System.IO.Path;

namespace FancyWidgets.Common.Convertors.Json;

internal class WidgetJsonProvider : IWidgetJsonProvider
{
    public virtual void SaveModel(object model, string nameFile)
    {
        var filePath = Path.Combine(WidgetPath.WorkDirectoryPath, nameFile);
        var jsonModel = JsonConvert.SerializeObject(model);
        File.WriteAllText(filePath, jsonModel);
    }

    public virtual T GetModel<T>(string path) where T : new()
    {
        var json = GetStringJson(path);
        return JsonConvert.DeserializeObject<T>(json) ?? new T();
    }

    public virtual void UpdateModel<T>(Action<T> updateAction, string path)
    {
        var json = GetStringJson(path);
        var model = JsonConvert.DeserializeObject<T>(json);

        if (model is null)
            return;

        updateAction(model);

        SaveModel(model, path);
    }

    protected virtual string GetStringJson(string path)
    {
        var filePath = Path.Combine(WidgetPath.WorkDirectoryPath, path);
        if (!File.Exists(filePath))
            CreateJsonFile(filePath);

        return File.ReadAllText(filePath);
    }

    protected virtual void CreateJsonFile(string filePath)
    {
        File.WriteAllText(filePath, string.Empty);
    }
}