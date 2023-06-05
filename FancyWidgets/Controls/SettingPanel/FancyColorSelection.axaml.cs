using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using FancyWidgets.ViewModels;
using ReactiveUI;

namespace FancyWidgets.Controls.SettingPanel;

public partial class FancyColorSelection : ReactiveUserControl<FancyColorSelectionViewModel>
{
    public string? Title { get; set; }

    public TextBox CustomTextBox { get; set; }
    
    public FancyColorSelection()
    {
        InitializeComponent();
        ViewModel = new FancyColorSelectionViewModel();
        DataContext = ViewModel;
        BindProperties();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        TextBox = this.FindControl<TextBox>("TextBox");
    }
    
    private void BindProperties()
    {
        this.WhenActivated(_ =>
        {
            if (ViewModel == null)
                return;
            this.WhenAnyValue(x => x.Title).BindTo(this, x => x.ViewModel!.Title);
            this.WhenAnyValue(x => x.TextBox).BindTo(this, x => x.ViewModel!.TextBox);
        });
        CustomTextBox = TextBox;
    }
}