using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using FancyWidgets.ViewModels;
using ReactiveUI;

namespace FancyWidgets.Controls.SettingPanel;

public class FancyCheckBox : ReactiveUserControl<FancyCheckBoxViewModel>
{
    public CheckBox CheckBox { get; set; }
    public string? Title { get; set; }
    
    public FancyCheckBox()
    {
        InitializeComponent();
        ViewModel = new FancyCheckBoxViewModel();
        DataContext = ViewModel;
        BindProperties();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        CheckBox = this.FindControl<CheckBox>("CheckBox");
    }
    
    private void BindProperties()
    {
        this.WhenActivated(_ =>
        {
            if (ViewModel == null)
                return;
            this.WhenAnyValue(x => x.Title).BindTo(this, x => x.ViewModel!.Title);
            this.WhenAnyValue(x => x.CheckBox).BindTo(this, x => x.ViewModel!.CheckBox);
        });
    }
}