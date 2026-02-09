using System;
using System.Drawing; // 需要引用 System.Drawing.Common 包
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace BlueCerb.Drivers.Helpers;

public static class ImageCaptureHelper
{
    // 引入 GDI+ 和 User32 相关 API
    [DllImport("user32.dll")]
    private static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

    [DllImport("user32.dll")]
    private static extern IntPtr GetWindowDC(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

    [DllImport("gdi32.dll")]
    private static extern IntPtr CreateCompatibleDC(IntPtr hDC);

    [DllImport("gdi32.dll")]
    private static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);

    [DllImport("gdi32.dll")]
    private static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

    [DllImport("gdi32.dll")]
    private static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

    [DllImport("gdi32.dll")]
    private static extern bool DeleteObject(IntPtr hObject);

    [DllImport("gdi32.dll")]
    private static extern bool DeleteDC(IntPtr hDC);

    // 常量
    private const int PW_CLIENTONLY = 1; // 仅截取客户区（去掉标题栏）
    private const int SRCCOPY = 0x00CC0020;

    /// <summary>
    /// 后台截图核心方法：无视遮挡
    /// </summary>
    /// <param name="hWnd">窗口句柄</param>
    /// <param name="width">宽</param>
    /// <param name="height">高</param>
    /// <returns>图像的字节数组 (Format: Bmp/Png)</returns>
    public static byte[] CaptureWindowBackground(IntPtr hWnd, int width, int height)
    {
        if (width <= 0 || height <= 0) return Array.Empty<byte>();

        IntPtr hdcSrc = GetWindowDC(hWnd);
        IntPtr hdcDest = CreateCompatibleDC(hdcSrc);
        IntPtr hBitmap = CreateCompatibleBitmap(hdcSrc, width, height);
        IntPtr hOld = SelectObject(hdcDest, hBitmap);

        try
        {
            // 核心：使用 PrintWindow 强制窗口绘制自己
            // PW_CLIENTONLY 自动去掉标题栏，配合我们之前的 GetClientRect 完美吻合
            bool success = PrintWindow(hWnd, hdcDest, PW_CLIENTONLY);

            if (!success)
            {
                // 如果 PrintWindow 失败（极少数游戏不支持），回退到 BitBlt
                // 注意：BitBlt 只能截取未被遮挡的部分
                BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, SRCCOPY);
            }

            // 将内存 Bitmap 转为字节数组
            using (Bitmap bmp = Image.FromHbitmap(hBitmap))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    // 为了速度，可以存为 BMP (无压缩)，VisionService 那边直接读
                    // 如果为了省内存，可以存为 JPEG
                    bmp.Save(ms, ImageFormat.Bmp);
                    return ms.ToArray();
                }
            }
        }
        finally
        {
            // 清理 GDI 资源（非常重要，否则会内存泄漏！）
            SelectObject(hdcDest, hOld);
            DeleteObject(hBitmap);
            DeleteDC(hdcDest);
            ReleaseDC(hWnd, hdcSrc);
        }
    }
}