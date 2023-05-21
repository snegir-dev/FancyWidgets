using Newtonsoft.Json;
using FancyWidgets.Models;

namespace FancyWidgets;

public class JsonFileManager
{
    private readonly string _workingDirectory = Directory.GetCurrentDirectory();

    public string GetStringJson(string path)
    {
        var filePath = Path.Combine(_workingDirectory, path);
        if (!File.Exists(filePath))
            CreateJsonFile(filePath);

        return File.ReadAllText(filePath);
    }

    public void SaveJsonFile(object model, string nameFile)
    {
        var filePath = Path.Combine(_workingDirectory, nameFile);
        var jsonModel = JsonConvert.SerializeObject(model);
        File.WriteAllText(filePath, jsonModel);
    }
    
    public T GetModelFromJson<T>(string path) where T : new()
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

        SaveJsonFile(model, path);
    }

    private void CreateJsonFile(string filePath)
    {
        File.WriteAllText(filePath, "{}");
    }
}