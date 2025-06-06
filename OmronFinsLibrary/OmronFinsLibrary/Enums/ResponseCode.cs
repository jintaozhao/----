using System;

namespace OmronFinsLibrary.Enums
{
    /// <summary>
    /// 响应代码枚举
    /// </summary>
    public enum ResponseCode : byte
    {
        /// <summary>
        /// 正常完成
        /// </summary>
        Normal = 0x00,
        
        /// <summary>
        /// 服务取消
        /// </summary>
        ServiceCanceled = 0x01,
        
        /// <summary>
        /// 本地节点不在网络中
        /// </summary>
        LocalNodeNotInNetwork = 0x02,
        
        /// <summary>
        /// 令牌超时
        /// </summary>
        TokenTimeout = 0x03,
        
        /// <summary>
        /// 重试次数超过
        /// </summary>
        RetriesExceeded = 0x04,
        
        /// <summary>
        /// 太多发送帧
        /// </summary>
        TooManySendFrames = 0x05,
        
        /// <summary>
        /// 节点地址范围错误
        /// </summary>
        NodeAddressRangeError = 0x06,
        
        /// <summary>
        /// 节点地址重复
        /// </summary>
        NodeAddressDuplicated = 0x07,
        
        /// <summary>
        /// 目标节点不在网络中
        /// </summary>
        DestinationNodeNotInNetwork = 0x01,
        
        /// <summary>
        /// 单元丢失
        /// </summary>
        UnitMissing = 0x02,
        
        /// <summary>
        /// 第三节点丢失
        /// </summary>
        ThirdNodeMissing = 0x03,
        
        /// <summary>
        /// 目标节点忙
        /// </summary>
        DestinationNodeBusy = 0x04,
        
        /// <summary>
        /// 响应超时
        /// </summary>
        ResponseTimeout = 0x05,
        
        /// <summary>
        /// 命令格式错误
        /// </summary>
        CommandFormatError = 0x10,
        
        /// <summary>
        /// 参数错误
        /// </summary>
        ParameterError = 0x11,
        
        /// <summary>
        /// 读取长度错误
        /// </summary>
        ReadLengthError = 0x12,
        
        /// <summary>
        /// 命令长度错误
        /// </summary>
        CommandLengthError = 0x13,
        
        /// <summary>
        /// 写入长度错误
        /// </summary>
        WriteLengthError = 0x14,
        
        /// <summary>
        /// 内存区域指定错误
        /// </summary>
        MemoryAreaError = 0x15,
        
        /// <summary>
        /// 地址范围错误
        /// </summary>
        AddressRangeError = 0x16,
        
        /// <summary>
        /// 地址范围超过
        /// </summary>
        AddressRangeExceeded = 0x17,
        
        /// <summary>
        /// 程序丢失
        /// </summary>
        ProgramMissing = 0x18,
        
        /// <summary>
        /// 关系表错误
        /// </summary>
        RelationalError = 0x19,
        
        /// <summary>
        /// 数据错误
        /// </summary>
        DataError = 0x1A,
        
        /// <summary>
        /// 命令错误
        /// </summary>
        CommandError = 0x1B,
        
        /// <summary>
        /// 无法执行
        /// </summary>
        CannotExecute = 0x1C
    }
}
