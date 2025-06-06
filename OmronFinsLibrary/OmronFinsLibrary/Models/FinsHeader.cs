using System;
using OmronFinsLibrary.Enums;

namespace OmronFinsLibrary.Models
{
    /// <summary>
    /// FINS帧头部信息
    /// </summary>
    public class FinsHeader
    {
        /// <summary>
        /// 信息控制字段
        /// </summary>
        public byte ICF { get; set; } = 0x80;
        
        /// <summary>
        /// 保留字段
        /// </summary>
        public byte RSV { get; set; } = 0x00;
        
        /// <summary>
        /// 网关数量
        /// </summary>
        public byte GCT { get; set; } = 0x02;
        
        /// <summary>
        /// 目标网络地址
        /// </summary>
        public byte DNA { get; set; } = 0x00;
        
        /// <summary>
        /// 目标节点地址
        /// </summary>
        public byte DA1 { get; set; }
        
        /// <summary>
        /// 目标单元地址
        /// </summary>
        public byte DA2 { get; set; } = 0x00;
        
        /// <summary>
        /// 源网络地址
        /// </summary>
        public byte SNA { get; set; } = 0x00;
        
        /// <summary>
        /// 源节点地址
        /// </summary>
        public byte SA1 { get; set; }
        
        /// <summary>
        /// 源单元地址
        /// </summary>
        public byte SA2 { get; set; } = 0x00;
        
        /// <summary>
        /// 服务标识
        /// </summary>
        public byte SID { get; set; }
        
        /// <summary>
        /// 转换为字节数组
        /// </summary>
        /// <returns></returns>
        public byte[] ToByteArray()
        {
            return new byte[]
            {
                ICF, RSV, GCT, DNA, DA1, DA2, SNA, SA1, SA2, SID
            };
        }
        
        /// <summary>
        /// 从字节数组创建FinsHeader
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <returns></returns>
        public static FinsHeader FromByteArray(byte[] data)
        {
            if (data.Length < 10)
                throw new ArgumentException("数据长度不足");
                
            return new FinsHeader
            {
                ICF = data[0],
                RSV = data[1],  
                GCT = data[2],
                DNA = data[3],
                DA1 = data[4],
                DA2 = data[5],
                SNA = data[6],
                SA1 = data[7],
                SA2 = data[8],
                SID = data[9]
            };
        }
    }
}
