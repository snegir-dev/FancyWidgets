﻿using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using FancyWidgets.ViewModels;
using ReactiveUI;

namespace FancyWidgets.Controls.SettingPanel;

public class FancyComboBox : ReactiveUserControl<FancyComboBoxViewModel>
{
    public ComboBox ComboBox { get; set; }
    public string? Title { get; set; } 
    
    public FancyComboBox()
    {
        InitializeComponent();
        BindProperties();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        ViewModel = new FancyComboBoxViewModel();
        DataContext = ViewModel;
        ComboBox = this.FindControl<ComboBox>("ComboBox");
    }
    
    private void BindProperties()
    {
        this.WhenActivated(_ =>
        {
            if (ViewModel == null)
                return;
            this.WhenAnyValue(x => x.Title).BindTo(this, x => x.ViewModel!.Title);
            this.WhenAnyValue(x => x.ComboBox).BindTo(this, x => x.ViewModel!.ComboBox);
        });
    }
}