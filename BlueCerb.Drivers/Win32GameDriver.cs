using BlueCerb.Core;
using BlueCerb.Core.Entities;
using BlueCerb.Core.Interfaces;
using BlueCerb.Drivers.Helpers;
using System;

namespace BlueCerb.Drivers;

public class Win32GameDriver : IGameDriver
{
    private IntPtr _hwnd = IntPtr.Zero;
    private GameRect _validGameArea; // 存储去除黑边后的有效区域

    // --- 接口实现：绑定与验证 ---

    public bool BindWindow(IntPtr hwnd)
    {
        if (hwnd == IntPtr.Zero || !Win32Native.IsWindow(hwnd))
            return false;

        _hwnd = hwnd;
        
        // 绑定成功后，立即计算一次有效区域（处理黑边）
        RefreshGameArea();
        return true;
    }

    public void Unbind()
    {
        _hwnd = IntPtr.Zero;
        _validGameArea = new GameRect(); // 清空区域
    }

    // 随时检查窗口是否有效（防止游戏崩溃后驱动还在瞎点）
    public bool IsBound => _hwnd != IntPtr.Zero && Win32Native.IsWindow(_hwnd);

    public GameRect GameArea => _validGameArea;

    // --- 接口实现：点击与输入 ---

    public void SendClick(int normalizedX, int normalizedY, MouseButton button = MouseButton.Left)
    {
        if (!IsBound) return;

        // 1. 获取像素坐标
        // 这里的逻辑是：把 0-10000 映射到 _validGameArea (纯净游戏区)
        // 这样即使有黑边，5000,5000 依然点的是游戏画面的中心，而不是窗口中心
        int pixelX = _validGameArea.X + (int)(normalizedX / 10000.0 * _validGameArea.Width);
        int pixelY = _validGameArea.Y + (int)(normalizedY / 10000.0 * _validGameArea.Height);

        // 2. 打包坐标参数
        IntPtr lParam = Win32Native.MakeLParam(pixelX, pixelY);

        // 3. 发送按下和松开消息 (模拟完整点击)
        if (button == MouseButton.Left)
        {
            Win32Native.PostMessage(_hwnd, Win32Native.WM_LBUTTONDOWN, (IntPtr)1, lParam);
            // 稍微延迟一点点通常更稳，但在纯驱动层我们直接发，由上层控制频率
            Win32Native.PostMessage(_hwnd, Win32Native.WM_LBUTTONUP, IntPtr.Zero, lParam);
        }
        else if (button == MouseButton.Right)
        {
            Win32Native.PostMessage(_hwnd, Win32Native.WM_RBUTTONDOWN, (IntPtr)1, lParam);
            Win32Native.PostMessage(_hwnd, Win32Native.WM_RBUTTONUP, IntPtr.Zero, lParam);
        }
    }

    public void SendKey(GameKey key)
    {
        if (!IsBound) return;

        // 映射 GameKey 到 Windows 虚拟键码
        int virtualKey = (int)key;

        Win32Native.PostMessage(_hwnd, Win32Native.WM_KEYDOWN, (IntPtr)virtualKey, IntPtr.Zero);
        Win32Native.PostMessage(_hwnd, Win32Native.WM_KEYUP, (IntPtr)virtualKey, IntPtr.Zero);
    }

    public byte[] CaptureFrame()
    {
        if (!IsBound) return Array.Empty<byte>();

        // 这里暂时用 GDI 截图演示，为了极致性能你应该在这里调用 DxgiCaptureHelper
        // 实际项目中：return _dxgiHelper.Capture(_hwnd);
        return new byte[0]; // 占位符
    }

    // --- 私有辅助：黑边计算 ---

    /// <summary>
/// 刷新游戏区域计算逻辑
/// 这里决定了点击坐标 (0-10000) 最终落在屏幕的哪里
/// </summary>
    private void RefreshGameArea()
    {
        // 1. 调用系统 API 获取“客户区”大小
        // Win32Native.GetClientRect 会自动忽略标题栏和边框
        // 此时 rect.Right 将会是 990，rect.Bottom 将会是 560
        if (!Win32Native.GetClientRect(_hwnd, out var rect)) 
        {
            return; 
        }

        // 2. 将系统返回的 RECT 转换为我们的 GameRect
        // 注意：GetClientRect 返回的 Left 和 Top 永远是 0
        // 因为 PostMessage 发送消息时，坐标原点(0,0)就是游戏画面的左上角（标题栏下方）

        int gameWidth = rect.Width;   // 这里会自动拿到 990
        int gameHeight = rect.Height; // 这里会自动拿到 560

        // 3. (可选) 黑边处理逻辑
        // 如果你的游戏为了保持比例（比如强制 16:9），在 990x560 的窗口里可能会有黑边
        // 990 / 560 = 1.767，非常接近 16:9 (1.777)
        // 假设游戏真的填满了，没有黑边，那就不需要偏移

        int blackBorderX = 0;
        int blackBorderY = 0;

        // 如果未来发现有黑边，可以在这里加逻辑：
        // if (gameWidth / (double)gameHeight > 1.777) { ... 计算左右黑边 ... }

        // 4. 最终赋值
        _validGameArea = new GameRect
        {
            X = blackBorderX,  // 0
            Y = blackBorderY,  // 0
            Width = gameWidth - (blackBorderX * 2),  // 990
            Height = gameHeight - (blackBorderY * 2) // 560
        };

        // 调试日志（实际运行时可以删掉）
        // Console.WriteLine($"检测到游戏画面大小: {_validGameArea.Width}x{_validGameArea.Height}");
    }
}