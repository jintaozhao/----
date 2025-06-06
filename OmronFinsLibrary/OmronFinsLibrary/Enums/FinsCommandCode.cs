using System;

namespace OmronFinsLibrary.Enums
{
    /// <summary>
    /// FINS命令代码枚举
    /// </summary>
    public enum FinsCommandCode : byte
    {
        /// <summary>
        /// 内存区域读取
        /// </summary>
        MemoryAreaRead = 0x01,
        
        /// <summary>
        /// 内存区域写入
        /// </summary>
        MemoryAreaWrite = 0x02,
        
        /// <summary>
        /// 内存区域填充
        /// </summary>
        MemoryAreaFill = 0x03,
        
        /// <summary>
        /// 多个内存区域读取
        /// </summary>
        MultipleMemoryAreaRead = 0x04,
        
        /// <summary>
        /// 内存区域传输
        /// </summary>
        MemoryAreaTransfer = 0x05,
        
        /// <summary>
        /// 参数区域读取
        /// </summary>
        ParameterAreaRead = 0x20,
        
        /// <summary>
        /// 参数区域写入
        /// </summary>
        ParameterAreaWrite = 0x21,
        
        /// <summary>
        /// 数据链接表读取
        /// </summary>
        DataLinkTableRead = 0x22,
        
        /// <summary>
        /// 数据链接表写入
        /// </summary>
        DataLinkTableWrite = 0x23,
        
        /// <summary>
        /// 强制置位/复位
        /// </summary>
        ForceSetReset = 0x23,
        
        /// <summary>
        /// 强制置位/复位取消
        /// </summary>
        ForceSetResetCancel = 0x24,
        
        /// <summary>
        /// 运行
        /// </summary>
        Run = 0x04,
        
        /// <summary>
        /// 停止
        /// </summary>
        Stop = 0x02,
        
        /// <summary>
        /// CPU单元数据读取
        /// </summary>
        CpuUnitDataRead = 0x05,
        
        /// <summary>
        /// 连接数据读取
        /// </summary>
        ConnectionDataRead = 0x06,
        
        /// <summary>
        /// CPU单元状态读取
        /// </summary>
        CpuUnitStatusRead = 0x06
    }
}
