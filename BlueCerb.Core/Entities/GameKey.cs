namespace BlueCerb.Core.Entities;

/// <summary>
/// 游戏内常用按键定义 (与 Win32 VirtualKey 对应)
/// </summary>
public enum GameKey
{
    None = 0,
    
    // 常用功能键
    Esc = 0x1B,
    Enter = 0x0D,
    Space = 0x20,
    Tab = 0x09,
    
    // 技能/菜单键 (根据游戏实际需求扩展)
    F1 = 0x70,
    F2 = 0x71,
    F3 = 0x72,
    F4 = 0x73,
    F5 = 0x74,
    
    // 字母键
    A = 0x41,
    B = 0x42,
    C = 0x43,
    D = 0x44,
    E = 0x45,
    F = 0x46,
    G = 0x47,
    H = 0x48,
    I = 0x49,
    J = 0x4A,
    K = 0x4B,
    L = 0x4C,
    M = 0x4D,
    N = 0x4E,
    O = 0x4F,
    P = 0x50,
    Q = 0x51,
    R = 0x52,
    S = 0x53,
    T = 0x54,
    U = 0x55,
    V = 0x56,
    W = 0x57,
    X = 0x58,
    Y = 0x59,
    Z = 0x5A,

    // 数字键
    D0 = 0x30,
    D1 = 0x31,
    D2 = 0x32,
    D3 = 0x33,
    
    // 控制键
    Control = 0x11,
    Shift = 0x10,
    Alt = 0x12
}