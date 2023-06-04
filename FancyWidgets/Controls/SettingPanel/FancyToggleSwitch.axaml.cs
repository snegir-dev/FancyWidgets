using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using FancyWidgets.ViewModels;
using ReactiveUI;

namespace FancyWidgets.Controls.SettingPanel;

public class FancyToggleSwitch : ReactiveUserControl<FancyToggleSwitchViewModel>
{
    public ToggleSwitch ToggleSwitch { get; set; }
    public string? Title { get; set; }

    public FancyToggleSwitch()
    {
        InitializeComponent();
        ViewModel = new FancyToggleSwitchViewModel();
        DataContext = ViewModel;
        BindProperties();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        ToggleSwitch = this.FindControl<ToggleSwitch>("ToggleSwitch");
    }

    private void BindProperties()
    {
        this.WhenActivated(_ =>
        {
            if (ViewModel == null)
                return;
            this.WhenAnyValue(x => x.Title).BindTo(this, x => x.ViewModel!.Title);
            this.WhenAnyValue(x => x.ToggleSwitch).BindTo(this, x => x.ViewModel!.ToggleSwitch);
        });
    }
}