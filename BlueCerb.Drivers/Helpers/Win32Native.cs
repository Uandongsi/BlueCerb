using System;
using System.Runtime.InteropServices;

namespace BlueCerb.Drivers.Helpers;

public static class Win32Native
{
    // 消息常量
    public const uint WM_LBUTTONDOWN = 0x0201;
    public const uint WM_LBUTTONUP = 0x0202;
    public const uint WM_RBUTTONDOWN = 0x0204;
    public const uint WM_RBUTTONUP = 0x0205;
    public const uint WM_KEYDOWN = 0x0100;
    public const uint WM_KEYUP = 0x0101;

    // 获取窗口客户区大小（不含标题栏，这才是游戏真正的坐标系）
    [DllImport("user32.dll")]
    public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

    // 检查窗口是否还活着
    [DllImport("user32.dll")]
    public static extern bool IsWindow(IntPtr hWnd);

    // 后台投递消息（实现无视遮挡的核心）
    [DllImport("user32.dll")]
    public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    // 将 X, Y 坐标打包成 LPARAM 格式
    public static IntPtr MakeLParam(int x, int y)
    {
        return (IntPtr)((y << 16) | (x & 0xFFFF));
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
        public int Width => Right - Left;
        public int Height => Bottom - Top;
    }
}