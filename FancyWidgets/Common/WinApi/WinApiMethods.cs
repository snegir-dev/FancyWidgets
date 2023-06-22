using System.Collections;
using System.Runtime.InteropServices;

namespace FancyWidgets.Common.WinApi;

internal static class WinApiMethods
{
    public delegate bool EnumedWindow(IntPtr handleWindow, ArrayList handles);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool EnumWindows(EnumedWindow lpEnumFunc, ArrayList? lParam);
}