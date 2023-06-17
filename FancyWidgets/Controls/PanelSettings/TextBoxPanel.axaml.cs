﻿using Avalonia.Controls;
using Avalonia.ReactiveUI;
using FancyWidgets.ViewModels.PanelSettings;
using ReactiveUI;

namespace FancyWidgets.Controls.PanelSettings;

public partial class TextBoxPanel : ReactiveUserControl<FancyTextBoxViewModel>
{
    public string? Title { get; set; }
    public TextBox TextBoxControl { get; set; }

    public TextBoxPanel()
    {
        InitializeComponent(true);
        InitializeComponent();
        BindProperties();
    }

    private void InitializeComponent()
    {
        ViewModel = new FancyTextBoxViewModel();
        DataContext = ViewModel;
    }

    private void BindProperties()
    {
        this.WhenActivated(_ =>
        {
            if (ViewModel == null)
                return;
            this.WhenAnyValue(x => x.Title).BindTo(this, x => x.ViewModel!.Title);
            this.WhenAnyValue(x => x.TextBoxControl).BindTo(this, x => x.ViewModel!.TextBox);
        });
        TextBoxControl = TextBox;
    }
    
    protected override Type StyleKeyOverride => typeof(TextBoxPanel);
}