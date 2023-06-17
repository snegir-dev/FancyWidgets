using Avalonia.Controls;
using Avalonia.ReactiveUI;
using FancyWidgets.ViewModels.PanelSettings;
using ReactiveUI;

namespace FancyWidgets.Controls.PanelSettings;

public partial class ButtonPanel : ReactiveUserControl<FancyButtonViewModel>
{
    public Button ButtonControl { get; set; }
    public string? Title { get; set; }

    public ButtonPanel()
    {
        InitializeComponent(true);
        InitializeComponent();
        BindProperties();
    }

    private void InitializeComponent()
    {
        ViewModel = new FancyButtonViewModel();
        DataContext = ViewModel;
    }

    private void BindProperties()
    {
        this.WhenActivated(_ =>
        {
            if (ViewModel == null)
                return;
            this.WhenAnyValue(x => x.Title).BindTo(this, x => x.ViewModel!.Title);
            this.WhenAnyValue(x => x.ButtonControl).BindTo(this, x => x.ViewModel!.Button);
        });
        ButtonControl = Button;
    }

    protected override Type StyleKeyOverride => typeof(ButtonPanel);
}