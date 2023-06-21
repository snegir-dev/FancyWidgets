using Avalonia;
using FancyWidgets.Common.WinApi;
using WinApi.User32;

namespace FancyWidgets.Common.System;

internal class WindowSystemManager
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

    public void SetWindowChildToDesktop()
    {
        var desktopHandler = GetDesktopHandler();
        User32Methods.GetWindowRect(_windowHandler, out var rect);
        User32Methods.SetWindowLongPtr(_windowHandler, (int)WindowLongFlags.GWLP_HWNDPARENT, desktopHandler);
        User32Methods.SetWindowPos(_windowHandler, 0, rect.Left, rect.Top, rect.Width, rect.Height,
            WindowPositionFlags.SWP_NOMOVE
            | WindowPositionFlags.SWP_NOSIZE
            | WindowPositionFlags.SWP_NOZORDER
            | WindowPositionFlags.SWP_NOACTIVATE
            | WindowPositionFlags.SWP_FRAMECHANGED);
    }

    private IntPtr GetDesktopHandler()
    {
        var desktopHandler = IntPtr.Zero;
        WinApiMethods.EnumWindows((window, _) =>
        {
            var hNextWin = User32Methods.FindWindowEx(window, IntPtr.Zero,
                WinApiConstants.ShellDllDefView, null);
            if (hNextWin != IntPtr.Zero)
                desktopHandler = hNextWin;

            return true;
        }, null);

        return desktopHandler;
    }
}