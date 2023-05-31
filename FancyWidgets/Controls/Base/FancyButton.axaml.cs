using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using FancyWidgets.ViewModels;

namespace FancyWidgets.Controls.Base;

public class FancyButton : ReactiveUserControl<FancyButtonViewModel>
{
    public FancyButton()
    {
        InitializeComponent();
        ViewModel = new FancyButtonViewModel();
        DataContext = ViewModel;
        ViewModel.Button = this.FindControl<Button>("Button");
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}