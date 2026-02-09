using BlueCerb.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueCerb.Core.Interfaces;

/// <summary>
/// 视觉服务接口：提供 OCR 文字识别、YOLO 目标检测和图像匹配能力。
/// </summary>
public interface IVisionService
{
    /// <summary>
    /// [OCR] 在图像中查找指定的文字，返回其中心坐标。
    /// </summary>
    /// <param name="image">图像数据</param>
    /// <param name="text">要查找的文字关键字（支持模糊匹配）</param>
    /// <param name="confidence">最低置信度 (默认 0.6)</param>
    /// <returns>找到的第一个目标的归一化中心坐标 (X, Y)，未找到返回 null</returns>
    Task<(int X, int Y)?> FindTextAsync(byte[] image, string text, double confidence = 0.6);

    /// <summary>
    /// [YOLO] 使用深度学习模型检测图像中的所有目标。
    /// </summary>
    /// <param name="image">图像数据</param>
    /// <param name="label">要筛选的目标标签（如 "monster", "drop_item"），传 null 则返回所有目标</param>
    /// <returns>检测到的目标列表</returns>
    Task<IEnumerable<VisionObject>> DetectObjectsAsync(byte[] image, string label = null);

    /// <summary>
    /// [OCR] 识别指定区域内的所有文字内容。
    /// </summary>
    /// <param name="image">图像数据</param>
    /// <param name="roi">感兴趣区域 (Region of Interest)，使用归一化坐标。若为 null 则识别全图。</param>
    /// <returns>识别到的文本内容</returns>
    Task<string> RecognizeTextAsync(byte[] image, GameRect? roi = null);

    /// <summary>
    /// [Legacy] 传统模板匹配 (用于识别没有文字且 YOLO 难训练的简单图标)。
    /// </summary>
    /// <param name="image">图像数据</param>
    /// <param name="templateName">模板文件名 (不含路径，如 "coin_icon.png")</param>
    /// <param name="threshold">匹配阈值 (0.8 - 1.0)</param>
    /// <returns>匹配到的中心坐标，未找到返回 null</returns>
    Task<(int X, int Y)?> FindTemplateAsync(byte[] image, string templateName, double threshold = 0.8);
}