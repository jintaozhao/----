using System;

namespace OmronFinsLibrary.Models
{
    /// <summary>
    /// 内存地址信息
    /// </summary>
    public class MemoryAddress
    {
        /// <summary>
        /// 内存区域代码
        /// </summary>
        public byte AreaCode { get; set; }
        
        /// <summary>
        /// 地址
        /// </summary>
        public ushort Address { get; set; }
        
        /// <summary>
        /// 位号（对于位操作）
        /// </summary>
        public byte Bit { get; set; }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="areaCode">内存区域代码</param>
        /// <param name="address">地址</param>
        /// <param name="bit">位号</param>
        public MemoryAddress(byte areaCode, ushort address, byte bit = 0)
        {
            AreaCode = areaCode;
            Address = address;
            Bit = bit;
        }
        
        /// <summary>
        /// 转换为字节数组
        /// </summary>
        /// <returns></returns>
        public byte[] ToByteArray()
        {
            return new byte[]
            {
                AreaCode,
                (byte)(Address >> 8),
                (byte)(Address & 0xFF),
                Bit
            };
        }
        
        /// <summary>
        /// 从字节数组创建MemoryAddress
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <returns></returns>
        public static MemoryAddress FromByteArray(byte[] data)
        {
            if (data.Length < 4)
                throw new ArgumentException("数据长度不足");
                
            return new MemoryAddress(
                data[0],
                (ushort)((data[1] << 8) | data[2]),
                data[3]
            );
        }
        
        /// <summary>
        /// 重写ToString方法
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Area:{AreaCode:X2} Address:{Address} Bit:{Bit}";
        }
    }
}
