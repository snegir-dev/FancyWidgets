namespace FancyWidgets.Common.Convertors.Json;

public interface IWidgetJsonProvider
{
    void SaveModel(object model, string nameFile);
    T GetModel<T>(string path) where T : new();
    void UpdateModel<T>(Action<T> updateAction, string path);
}