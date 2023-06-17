using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using ReactiveUI;

namespace FancyWidgets.ViewModels.PanelSettings;

public class GroupBoxViewModel : ReactiveObject
{
    private HeaderedContentControl _groupBox;
    private StackPanel _stackPanelContainer;

    public HeaderedContentControl GroupBox
    {
        get => _groupBox;
        set => this.RaiseAndSetIfChanged(ref _groupBox, value);
    }
    
    public StackPanel StackPanelContainer
    {
        get => _stackPanelContainer;
        set => this.RaiseAndSetIfChanged(ref _stackPanelContainer, value);
    }
}