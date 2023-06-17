using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using FancyWidgets.ViewModels.PanelSettings;
using ReactiveUI;
using Control = Avalonia.Controls.Control;

namespace FancyWidgets.Controls.PanelSettings;

public partial class GroupBoxPanel : ReactiveUserControl<GroupBoxViewModel>
{
    public HeaderedContentControl GroupBox { get; set; }
    public StackPanel StackPanelContainer { get; set; }

    public GroupBoxPanel()
    {
        InitializeComponent(true);
        InitializeComponent();
        BindProperties();
        StackPanel.Initialized += AddControlSeparators;
    }

    private void InitializeComponent()
    {
        ViewModel = new GroupBoxViewModel();
        DataContext = ViewModel;
    }
    
    private void BindProperties()
    {
        this.WhenActivated(_ =>
        {
            if (ViewModel == null)
                return;
            this.WhenAnyValue(x => x.GroupBox).BindTo(this, x => x.ViewModel!.GroupBox);
            this.WhenAnyValue(x => x.StackPanelContainer).BindTo(this, x => x.ViewModel!.StackPanelContainer);
        });
        StackPanelContainer = StackPanel;
        GroupBox = HeaderedContentControl;
    }

    private void AddControlSeparators(object? sender, EventArgs eventArgs)
    {
        var controls = new List<Control>();
        for (var i = 0; i < StackPanel.Children.Count - 1; i++)
        {
            controls.Add(StackPanel.Children[i]);
            controls.Add(new Separator
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(5, 0),
                CornerRadius = new CornerRadius(999),
                Height = 2,
                Background = Brush.Parse("#1FFF")
            });
        }
            
        controls.Add(StackPanel.Children[^1]);
        StackPanel.Children.Clear();
        StackPanel.Children.AddRange(controls);
    }
}