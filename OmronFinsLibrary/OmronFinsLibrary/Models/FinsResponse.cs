using System;
using OmronFinsLibrary.Enums;

namespace OmronFinsLibrary.Models
{    /// <summary>
    /// FINS响应消息
    /// </summary>
    public class FinsResponse
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
        /// 主响应代码
        /// </summary>
        public ResponseCode MainResponseCode { get; set; }
        
        /// <summary>
        /// 子响应代码
        /// </summary>
        public byte SubResponseCode { get; set; }
        
        /// <summary>
        /// 数据部分
        /// </summary>
        public byte[] Data { get; set; } = new byte[0];
        
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess => MainResponseCode == ResponseCode.Normal && SubResponseCode == 0x00;
        
        /// <summary>
        /// 从字节数组创建FinsResponse
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <returns></returns>
        public static FinsResponse FromByteArray(byte[] data)
        {
            if (data.Length < 14)
                throw new ArgumentException("响应数据长度不足");
                
            var response = new FinsResponse
            {
                Header = FinsHeader.FromByteArray(data),
                CommandCode = (FinsCommandCode)data[10],
                SubCommandCode = data[11],
                MainResponseCode = (ResponseCode)data[12],
                SubResponseCode = data[13]
            };
            
            if (data.Length > 14)
            {
                response.Data = new byte[data.Length - 14];
                Array.Copy(data, 14, response.Data, 0, response.Data.Length);
            }
            
            return response;
        }
        
        /// <summary>
        /// 转换为字节数组
        /// </summary>
        /// <returns></returns>
        public byte[] ToByteArray()
        {
            var headerBytes = Header.ToByteArray();
            var result = new byte[headerBytes.Length + 4 + Data.Length];
            
            Array.Copy(headerBytes, 0, result, 0, headerBytes.Length);
            result[headerBytes.Length] = (byte)CommandCode;
            result[headerBytes.Length + 1] = SubCommandCode;
            result[headerBytes.Length + 2] = (byte)MainResponseCode;
            result[headerBytes.Length + 3] = SubResponseCode;
            Array.Copy(Data, 0, result, headerBytes.Length + 4, Data.Length);
            
            return result;
        }
    }
}
