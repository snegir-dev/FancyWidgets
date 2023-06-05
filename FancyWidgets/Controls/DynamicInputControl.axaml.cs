using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Metadata;
using FancyWidgets.Common.SettingProvider;
using FancyWidgets.Common.SettingProvider.Interfaces;
using ReactiveUI;
using Splat;

namespace FancyWidgets.Controls;

public partial class DynamicInputControl : TemplatedControl, IActivatableView
{
    public static readonly StyledProperty<string> DataTypeProperty =
        AvaloniaProperty.Register<DynamicInputControl, string>(nameof(DataType));

    [Content]
    public string DataType
    {
        get => GetValue(DataTypeProperty);
        set => SetValue(DataTypeProperty, value);
    }

    private StackPanel? _stackPanel;

    private List<ISettingsControl> _settingsControls = new();

    public void CreateInputControl()
    {
        foreach (var settingsControl in _settingsControls)
        {
            _stackPanel?.Children.Add(settingsControl.Control);
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _stackPanel = e.NameScope.Find<StackPanel>("StackPanel");
        _settingsControls = Locator.Current.GetServices<ISettingsControl>().ToList();
        CreateInputControl();
    }
}