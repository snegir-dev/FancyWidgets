using FancyWidgets.Common.SettingProvider.Models;

namespace FancyWidgets.Common.SettingProvider;

public static class ReactiveObjectDataStatusContainer
{
    public static HashSet<ReactiveObjectDataStatus?> CurrentObjectDataStatuses { get; } = new();
}