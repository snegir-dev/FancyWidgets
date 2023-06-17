using Avalonia.Controls;
using Avalonia.ReactiveUI;
using FancyWidgets.ViewModels.PanelSettings;
using ReactiveUI;

namespace FancyWidgets.Controls.PanelSettings;

public partial class ToggleSwitchPanel : ReactiveUserControl<FancyToggleSwitchViewModel>
{
    public ToggleSwitch ToggleSwitchControl { get; set; }
    public string? Title { get; set; }

    public ToggleSwitchPanel()
    {
        InitializeComponent(true);
        InitializeComponent();
        BindProperties();
    }

    private void InitializeComponent()
    {
        ViewModel = new FancyToggleSwitchViewModel();
        DataContext = ViewModel;
    }

    private void BindProperties()
    {
        this.WhenActivated(_ =>
        {
            if (ViewModel == null)
                return;
            this.WhenAnyValue(x => x.Title).BindTo(this, x => x.ViewModel!.Title);
            this.WhenAnyValue(x => x.ToggleSwitchControl).BindTo(this, x => x.ViewModel!.ToggleSwitch);
        });
        ToggleSwitchControl = ToggleSwitch;
    }
}