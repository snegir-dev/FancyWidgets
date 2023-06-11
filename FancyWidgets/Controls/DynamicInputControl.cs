using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Metadata;
using Avalonia.ReactiveUI;
using FancyWidgets.Common.SettingProvider;
using FancyWidgets.Common.SettingProvider.Interfaces;
using ReactiveUI;
using Splat;

namespace FancyWidgets.Controls;

public class DynamicInputControl : ReactiveUserControl<ReactiveObject>
{
    private readonly DynamicInputControlUi _dynamicInputControlUi = new();
    private readonly StackPanel? _stackPanel;
    private List<ISettingsControl> _settingsControls = new();

    public DynamicInputControl()
    {
        _dynamicInputControlUi.Create(this);
        _stackPanel = _dynamicInputControlUi.StackPanel;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        _settingsControls = Locator.Current.GetServices<ISettingsControl>().ToList();
        CreateInputControl();
    }

    private void CreateInputControl()
    {
        foreach (var settingsControl in _settingsControls)
        {
            _stackPanel?.Children.Add(settingsControl.Control);
        }
    }
}