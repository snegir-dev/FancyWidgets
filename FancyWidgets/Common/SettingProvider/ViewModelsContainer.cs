using ReactiveUI;

namespace FancyWidgets.Common.SettingProvider;

public static class ViewModelsContainer
{
    public static HashSet<ReactiveObject?> CurrentViewModels { get; } = new();
}