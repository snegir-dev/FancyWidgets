using Avalonia.Controls;
using Avalonia.ReactiveUI;
using FancyWidgets.ViewModels.PanelSettings;
using ReactiveUI;

namespace FancyWidgets.Controls.PanelSettings;

public partial class ComboBoxPanel : ReactiveUserControl<FancyComboBoxViewModel>
{
    public ComboBox ComboBoxControl { get; set; }
    public string? Title { get; set; } 
    
    public ComboBoxPanel()
    {
        InitializeComponent(true);
        InitializeComponent();
        BindProperties();
    }

    private void InitializeComponent()
    {
        ViewModel = new FancyComboBoxViewModel();
        DataContext = ViewModel;
    }
    
    private void BindProperties()
    {
        this.WhenActivated(_ =>
        {
            if (ViewModel == null)
                return;
            this.WhenAnyValue(x => x.Title).BindTo(this, x => x.ViewModel!.Title);
            this.WhenAnyValue(x => x.ComboBoxControl).BindTo(this, x => x.ViewModel!.ComboBox);
        });
        ComboBoxControl = ComboBox;
    }
}