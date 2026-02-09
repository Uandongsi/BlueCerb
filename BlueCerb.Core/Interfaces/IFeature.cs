using BlueCerb.Core.Entities;
using System;
using System.Threading.Tasks;

namespace BlueCerb.Core.Interfaces;

/// <summary>
/// 功能模块标准接口：所有业务逻辑类（如战斗、交易）都必须实现此接口。
/// </summary>
public interface IFeature
{
    /// <summary>
    /// 功能名称 (显示在 UI 侧边栏或日志中)
    /// </summary>
    string Name { get; }

    /// <summary>
    /// 功能描述 (用于鼠标悬停提示)
    /// </summary>
    string Description { get; }

    /// <summary>
    /// 当前运行状态 (UI 绑定此属性来切换 "开始/停止" 按钮的文字)
    /// </summary>
    bool IsRunning { get; }

    /// <summary>
    /// 异步执行核心逻辑。
    /// <para>注意：实现类必须处理好 CancellationToken，确保能被立即停止。</para>
    /// </summary>
    Task ExecuteAsync();

    /// <summary>
    /// 立即停止当前逻辑（通常通过取消 CancellationToken 实现）。
    /// </summary>
    void Stop();

    /// <summary>
    /// [事件] 当功能产生日志时触发。
    /// <para>UI 层订阅此事件，将后台的运行情况显示在底部的日志控制台。</para>
    /// </summary>
    event Action<LogModel> OnLog;
}