using System;
using OmronFinsLibrary.Enums;

namespace OmronFinsLibrary.Models
{    /// <summary>
    /// FINS请求消息
    /// </summary>
    public class FinsRequest
    {
        /// <summary>
        /// FINS头部
        /// </summary>
        public FinsHeader Header { get; set; } = new FinsHeader();
        
        /// <summary>
        /// 命令代码
        /// </summary>
        public FinsCommandCode CommandCode { get; set; }
        
        /// <summary>
        /// 子命令代码
        /// </summary>
        public byte SubCommandCode { get; set; }
        
        /// <summary>
        /// 数据部分
        /// </summary>
        public byte[] Data { get; set; } = new byte[0];
        
        /// <summary>
        /// 转换为字节数组
        /// </summary>
        /// <returns></returns>
        public byte[] ToByteArray()
        {
            var headerBytes = Header.ToByteArray();
            var result = new byte[headerBytes.Length + 2 + Data.Length];
            
            Array.Copy(headerBytes, 0, result, 0, headerBytes.Length);
            result[headerBytes.Length] = (byte)CommandCode;
            result[headerBytes.Length + 1] = SubCommandCode;
            Array.Copy(Data, 0, result, headerBytes.Length + 2, Data.Length);
            
            return result;
        }
    }
}
