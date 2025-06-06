using System;

namespace OmronFinsLibrary.Enums
{
    /// <summary>
    /// 内存区域代码枚举
    /// </summary>
    public enum MemoryAreaCode : byte
    {
        /// <summary>
        /// CIO区域（输入/输出继电器）
        /// </summary>
        CIO = 0x30,
        
        /// <summary>
        /// 工作区域（内部继电器）
        /// </summary>
        Work = 0x31,
        
        /// <summary>
        /// 保持区域（保持继电器）
        /// </summary>
        Holding = 0x32,
        
        /// <summary>
        /// 辅助区域（辅助继电器）
        /// </summary>
        Auxiliary = 0x33,
        
        /// <summary>
        /// 定时器/计数器区域
        /// </summary>
        TimerCounter = 0x09,
        
        /// <summary>
        /// DM区域（数据内存）
        /// </summary>
        DataMemory = 0x02,
        
        /// <summary>
        /// EM区域（扩展数据内存）
        /// </summary>
        ExtendedMemory = 0x20,
        
        /// <summary>
        /// Task标志
        /// </summary>
        TaskFlag = 0x46,
        
        /// <summary>
        /// Task状态
        /// </summary>
        TaskStatus = 0x47,
        
        /// <summary>
        /// 时钟脉冲
        /// </summary>
        ClockPulse = 0x07,
        
        /// <summary>
        /// 条件标志
        /// </summary>
        ConditionFlag = 0x05
    }
}
