using ReactiveUI;

namespace FancyWidgets.Common.SettingProvider;

public static class ViewModelContainer
{
    public static ReactiveObject? CurrentViewModel { get; set; }
}