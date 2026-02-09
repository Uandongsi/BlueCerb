namespace BlueCerb.Core.Entities;

public class LogModel
{
    /// <summary>
    /// 日志产生时间
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.Now;

    /// <summary>
    /// 日志级别 (Info, Warning, Error, Success)
    /// 用于前端显示不同的颜色
    /// </summary>
    public string Level { get; set; } = "Info";

    /// <summary>
    /// 日志内容
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// 来源模块 (如: Battle, System, Trade)
    /// </summary>
    public string Source { get; set; } = "System";
}