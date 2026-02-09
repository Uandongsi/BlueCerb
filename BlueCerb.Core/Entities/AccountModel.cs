namespace BlueCerb.Core.Entities;

public class AccountModel
{
    /// <summary>
    /// 账户唯一 ID (用于数据库索引)
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 登录用户名
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// 加密后的密码 (不要存明文！)
    /// </summary>
    public string EncryptedPassword { get; set; } = string.Empty;

    /// <summary>
    /// 备注信息 (如：大号、仓库号)
    /// </summary>
    public string Note { get; set; } = string.Empty;

    /// <summary>
    /// 代理配置字符串 (格式如: socks5://127.0.0.1:1080)
    /// 若为空则直连
    /// </summary>
    public string ProxyConnection { get; set; } = string.Empty;
    
    /// <summary>
    /// 是否启用 (在列表中勾选)
    /// </summary>
    public bool IsEnabled { get; set; } = true;
}