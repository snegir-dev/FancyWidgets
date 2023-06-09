using WinApi.User32;

namespace FancyWidgets.Common.System;

public class WindowSystemManager
{
    private readonly IntPtr _windowHandler;

    public WindowSystemManager(IntPtr windowHandler)
    {
        _windowHandler = windowHandler;
    }

    public void HideMinimizeAndMaximizeButtons()
    {
        var currentStyle = User32Methods.GetWindowLongPtr(_windowHandler, (int)WindowLongFlags.GWL_STYLE);
        var newStyle = currentStyle & (IntPtr)(~(WindowStyles.WS_MINIMIZEBOX | WindowStyles.WS_MAXIMIZEBOX));
        User32Methods.SetWindowLongPtr(_windowHandler, (int)WindowLongFlags.GWL_STYLE, newStyle);
    }
    
    public void HideFromAltTab()
    {
        var currentStyle = User32Helpers.GetWindowLongPtr(_windowHandler, WindowLongFlags.GWL_EXSTYLE);
        User32Helpers.SetWindowLongPtr(_windowHandler, WindowLongFlags.GWL_EXSTYLE,
            currentStyle | (int)WindowExStyles.WS_EX_NOACTIVATE);
    }
    
    public void WidgetToBottom(PixelPoint position, int width, int height)
    {
        User32Methods.SetWindowPos(_windowHandler,
            (IntPtr)HwndZOrder.HWND_BOTTOM, position.X, position.Y, width, height,
            WindowPositionFlags.SWP_NOACTIVATE);
    }
}