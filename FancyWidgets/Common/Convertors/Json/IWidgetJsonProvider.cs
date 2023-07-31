namespace FancyWidgets.Common.Convertors.Json;

public interface IWidgetJsonProvider
{
    void SaveModel(object model, string nameFile);
    T? GetModel<T>(string path);
    void UpdateModel<T>(Action<T> updateAction, string path, bool createIfMissing = false)
        where T : new();
}