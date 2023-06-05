using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using FancyWidgets.ViewModels;
using ReactiveUI;

namespace FancyWidgets.Controls.SettingPanel;

public partial class FancyButton : ReactiveUserControl<FancyButtonViewModel>
{
    public Button CustomButton { get; set; }
    public string? Title { get; set; }
    
    public FancyButton()
    {
        InitializeComponent();
        ViewModel = new FancyButtonViewModel();
        DataContext = ViewModel;
        BindProperties();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        Button = this.FindControl<Button>("Button");
    }
    
    private void BindProperties()
    {
        this.WhenActivated(_ =>
        {
            if (ViewModel == null)
                return;
            this.WhenAnyValue(x => x.Title).BindTo(this, x => x.ViewModel!.Title);
            this.WhenAnyValue(x => x.Button).BindTo(this, x => x.ViewModel!.Button);
        });
        CustomButton = Button;
    }
}