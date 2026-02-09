using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace BlueCerb.Core.Interfaces;

/// <summary>
/// 网络服务接口 (Web 交易增强版)
/// <para>专门针对网页版交易市场设计，支持 Cookie 维持、图片获取和长连接复用。</para>
/// </summary>
public interface INetworkService
{
    // ============================
    // 基础请求 (支持自动携带 Cookie)
    // ============================

    /// <summary>
    /// [GET] 获取网页源码或 JSON 数据。
    /// </summary>
    Task<string> GetStringAsync(string url, Dictionary<string, string>? headers = null);

    /// <summary>
    /// [POST] 提交表单或 JSON 数据（用于下订单）。
    /// </summary>
    Task<string> PostJsonAsync(string url, object data, Dictionary<string, string>? headers = null);

    // ============================
    // 新增：针对网页交易的增强功能
    // ============================

    /// <summary>
    /// [GET] 下载二进制数据（专用于获取验证码图片、物品缩略图）。
    /// </summary>
    /// <param name="url">图片地址</param>
    /// <returns>图片的字节数组</returns>
    Task<byte[]> GetBytesAsync(string url);

    /// <summary>
    /// [Cookie] 手动同步 Cookie。
    /// <para>场景：当你通过 WebView2 登录成功后，需要把浏览器的 Cookie 提取出来给 HTTP 协议层使用。</para>
    /// </summary>
    /// <param name="url">域名 (如 https://market.game.com)</param>
    /// <param name="cookieName">Cookie 名称</param>
    /// <param name="cookieValue">Cookie 值</param>
    void UpdateCookie(string url, string cookieName, string cookieValue);

    /// <summary>
    /// [Cookie] 获取当前所有的 Cookie 容器（用于持久化保存登录状态）。
    /// </summary>
    CookieContainer GetCookieContainer();

    /// <summary>
    /// [配置] 设置全局默认请求头。
    /// <para>场景：设置 User-Agent 伪装成真实的 Chrome 浏览器，防止被反爬虫拦截。</para>
    /// </summary>
    void SetDefaultHeaders(Dictionary<string, string> headers);
    
    /// <summary>
    /// [代理] 切换 IP（多账号防封必备）。
    /// </summary>
    void SetProxy(string proxyUrl);
}