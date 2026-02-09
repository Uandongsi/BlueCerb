namespace BlueCerb.Core.Entities;

/// <summary>
/// 表示视觉识别到的一个目标物体（可以是 YOLO 识别的怪物，也可以是 OCR 识别的文字块）。
/// </summary>
public class VisionObject
{
    /// <summary>
    /// 目标的标签或内容 (例如 "Monster_A", "Button_Ok", "购买")
    /// </summary>
    public string Label { get; set; } = string.Empty;

    /// <summary>
    /// 置信度 (0.0 - 1.0)，越高越可信
    /// </summary>
    public float Confidence { get; set; }

    /// <summary>
    /// 目标在屏幕上的位置 (归一化坐标系 0-10000)
    /// </summary>
    public GameRect Rect { get; set; }

    /// <summary>
    /// 获取目标的中心点 X 坐标 (归一化)
    /// </summary>
    public int CenterX => Rect.X + Rect.Width / 2;

    /// <summary>
    /// 获取目标的中心点 Y 坐标 (归一化)
    /// </summary>
    public int CenterY => Rect.Y + Rect.Height / 2;
}