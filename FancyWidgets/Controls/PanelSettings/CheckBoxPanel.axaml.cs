using Avalonia.Controls;
using Avalonia.ReactiveUI;
using FancyWidgets.ViewModels.PanelSettings;
using ReactiveUI;

namespace FancyWidgets.Controls.PanelSettings;

public partial class CheckBoxPanel : ReactiveUserControl<FancyCheckBoxViewModel>
{
    public CheckBox CheckBoxControl { get; set; }
    public string? Title { get; set; }
    
    public CheckBoxPanel()
    {
        InitializeComponent(true);
        InitializeComponent();
        BindProperties();
    }

    private void InitializeComponent()
    {
        ViewModel = new FancyCheckBoxViewModel();
        DataContext = ViewModel;
    }
    
    private void BindProperties()
    {
        this.WhenActivated(_ =>
        {
            if (ViewModel == null)
                return;
            this.WhenAnyValue(x => x.Title).BindTo(this, x => x.ViewModel!.Title);
            this.WhenAnyValue(x => x.CheckBoxControl).BindTo(this, x => x.ViewModel!.CheckBox);
        });
        CheckBoxControl = CheckBox;
    }
    
    protected override Type StyleKeyOverride => typeof(CheckBoxPanel);
}