using BlueCerb.Core.Entities;
using System;

namespace BlueCerb.Core.Interfaces;

/// <summary>
/// 游戏驱动接口：定义与游戏窗口的底层交互契约。
/// </summary>
public interface IGameDriver
{
    /// <summary>
    /// 绑定指定句柄的游戏窗口。
    /// </summary>
    /// <param name="hwnd">游戏窗口句柄</param>
    /// <returns>绑定是否成功</returns>
    bool BindWindow(IntPtr hwnd);

    /// <summary>
    /// 解除当前窗口绑定，释放资源。
    /// </summary>
    void Unbind();

    /// <summary>
    /// 获取当前窗口是否已绑定且有效。
    /// </summary>
    bool IsBound { get; }

    /// <summary>
    /// 高速截取当前游戏画面的像素数据。
    /// <para>注意：驱动层应自动处理黑边裁剪，返回纯净的游戏画面。</para>
    /// </summary>
    /// <returns>图像字节数组 (通常为 BGR 或 BGRA 格式)</returns>
    byte[] CaptureFrame();

    /// <summary>
    /// 执行鼠标点击操作（使用归一化坐标）。
    /// </summary>
    /// <param name="normalizedX">X 坐标 (0-10000)，0为最左，10000为最右。</param>
    /// <param name="normalizedY">Y 坐标 (0-10000)，0为最上，10000为最下。</param>
    /// <remarks>
    /// 驱动层需负责将归一化坐标映射回当前窗口的实际像素坐标。
    /// 示例：(5000, 5000) 总是代表屏幕中心。
    /// </remarks>
void SendClick(int normalizedX, int normalizedY, MouseButton button = MouseButton.Left);

    /// <summary>
    /// 发送键盘按键。
    /// </summary>
    /// <param name="key">自定义的虚拟按键枚举</param>
    void SendKey(GameKey key);

    /// <summary>
    /// 获取当前有效的游戏区域（去除了黑边的实际画面区域）。
    /// </summary>
    GameRect GameArea { get; }
}