using System.Collections;
using System.Runtime.InteropServices;

namespace FancyWidgets.Common.WinApi;

public static class WinApiMethods
{
    public delegate IntPtr WindowProcedureDelegate(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam);
    public delegate bool EnumedWindow(IntPtr handleWindow, ArrayList handles);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
    
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool EnumWindows(EnumedWindow lpEnumFunc, ArrayList? lParam);
}