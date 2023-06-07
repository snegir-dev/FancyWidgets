using Avalonia.ReactiveUI;
using ReactiveUI;

namespace FancyWidgets.Controls;

public class DynamicInputControlUi
{
    public StackPanel? StackPanel { get; private set; }

    public void Create(ReactiveUserControl<ReactiveObject> userControl)
    {
        userControl.Content(
            StackPanel(out var stackPanel)
                .Background(Brush.Parse("#151515"))
        );

        var resource = AvaloniaXamlLoader.Load(new Uri("avares://FancyWidgets/Resources/BaseResource.axaml"));
        userControl.Resources((ResourceDictionary)resource);

        StackPanel = stackPanel;
    }
}