using FancyWidgets.Common.SettingProvider.Models;

namespace FancyWidgets.Common.SettingProvider;

public static class ViewModelsContainer
{
    public static HashSet<ObjectWithDataStatus?> CurrentViewModels { get; } = new();
}