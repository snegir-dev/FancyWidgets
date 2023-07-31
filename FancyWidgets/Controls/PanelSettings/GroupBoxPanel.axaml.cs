using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;
using Control = Avalonia.Controls.Control;

namespace FancyWidgets.Controls.PanelSettings;

public partial class GroupBoxPanel : UserControl
{
    public static readonly DirectProperty<GroupBoxPanel, string> HeaderProperty =
        AvaloniaProperty.RegisterDirect<GroupBoxPanel, string>(nameof(Header),
            o => o.Header, (o, v) => o.Header = v);

    public static readonly DirectProperty<GroupBoxPanel, bool> IsSeparateProperty =
        AvaloniaProperty.RegisterDirect<GroupBoxPanel, bool>(nameof(IsSeparate),
            o => o.IsSeparate, (o, v) => o.IsSeparate = v);

    public static readonly DirectProperty<GroupBoxPanel, IBrush> SeparatorColorProperty =
        AvaloniaProperty.RegisterDirect<GroupBoxPanel, IBrush>(nameof(SeparatorColor),
            o => o.SeparatorColor, (o, v) => o.SeparatorColor = v);

    private string _header = string.Empty;
    private bool _isSeparate = true;
    private IBrush _separatorColor = Brush.Parse("#1FFF");

    public string Header
    {
        get => _header;
        set
        {
            SetAndRaise(HeaderProperty, ref _header, value);
            GroupBoxControl.Header = value;
        }
    }

    public bool IsSeparate
    {
        get => _isSeparate;
        set => SetAndRaise(IsSeparateProperty, ref _isSeparate, value);
    }

    public IBrush SeparatorColor
    {
        get => _separatorColor;
        set => SetAndRaise(SeparatorColorProperty, ref _separatorColor, value);
    }

    public GroupBoxPanel()
    {
        InitializeComponent();
        GroupBoxContent.Initialized += AddControlSeparators;
    }

    private void AddControlSeparators(object? sender, EventArgs eventArgs)
    {
        if (!IsSeparate)
            return;

        var controls = new List<Control>();
        for (var i = 0; i < GroupBoxContent.Children.Count - 1; i++)
        {
            controls.Add(GroupBoxContent.Children[i]);
            controls.Add(new Separator
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(5, 0),
                CornerRadius = new CornerRadius(999),
                Height = 2,
                Background = SeparatorColor
            });
        }

        controls.Add(GroupBoxContent.Children[^1]);
        GroupBoxContent.Children.Clear();
        GroupBoxContent.Children.AddRange(controls);
    }
}