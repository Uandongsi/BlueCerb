namespace BlueCerb.Core.Entities;

/// <summary>
/// 表示屏幕上的一个矩形区域（通常用于存储去除了黑边后的游戏画面区域）。
/// </summary>
public struct GameRect
{
    /// <summary>
    /// 区域左上角的 X 坐标（屏幕物理像素）
    /// </summary>
    public int X { get; set; }

    /// <summary>
    /// 区域左上角的 Y 坐标（屏幕物理像素）
    /// </summary>
    public int Y { get; set; }

    /// <summary>
    /// 区域的宽度（像素）
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// 区域的高度（像素）
    /// </summary>
    public int Height { get; set; }

    // 提供一个便捷属性，判断这个区域是否有效
    public bool IsValid => Width > 0 && Height > 0;

    // 方便调试时输出
    public override string ToString() => $"[X={X}, Y={Y}, W={Width}, H={Height}]";
}