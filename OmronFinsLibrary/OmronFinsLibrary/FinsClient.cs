using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OmronFinsLibrary.Communication;
using OmronFinsLibrary.Enums;
using OmronFinsLibrary.Models;

namespace OmronFinsLibrary
{    /// <summary>
    /// FINS通信客户端
    /// </summary>
    public class FinsClient : IDisposable
    {
        private FinsUdpClient? _udpClient;
        private FinsTcpClient? _tcpClient;
        private bool _isUdp;
        
        /// <summary>
        /// 使用UDP协议创建FINS客户端
        /// </summary>
        /// <param name="remoteIp">远程IP地址</param>
        /// <param name="remotePort">远程端口</param>
        /// <param name="localNodeAddress">本地节点地址</param>
        /// <param name="remoteNodeAddress">远程节点地址</param>
        /// <returns></returns>
        public static FinsClient CreateUdpClient(string remoteIp, int remotePort = 9600, byte localNodeAddress = 0x01, byte remoteNodeAddress = 0x01)
        {
            return new FinsClient
            {
                _udpClient = new FinsUdpClient(remoteIp, remotePort, localNodeAddress, remoteNodeAddress),
                _isUdp = true
            };
        }
        
        /// <summary>
        /// 使用TCP协议创建FINS客户端
        /// </summary>
        /// <param name="remoteIp">远程IP地址</param>
        /// <param name="remotePort">远程端口</param>
        /// <param name="localNodeAddress">本地节点地址</param>
        /// <param name="remoteNodeAddress">远程节点地址</param>
        /// <returns></returns>
        public static FinsClient CreateTcpClient(string remoteIp, int remotePort = 9600, byte localNodeAddress = 0x01, byte remoteNodeAddress = 0x01)
        {
            return new FinsClient
            {
                _tcpClient = new FinsTcpClient(remoteIp, remotePort, localNodeAddress, remoteNodeAddress),
                _isUdp = false
            };
        }
          /// <summary>
        /// 连接到PLC
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ConnectAsync()
        {
            if (_isUdp)
                return _udpClient != null ? await _udpClient.ConnectAsync() : false;
            else
                return _tcpClient != null ? await _tcpClient.ConnectAsync() : false;
        }
        
        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            if (_isUdp)
                _udpClient?.Disconnect();
            else
                _tcpClient?.Disconnect();
        }
        
        /// <summary>
        /// 读取内存区域数据（字单位）
        /// </summary>
        /// <param name="memoryArea">内存区域代码</param>
        /// <param name="address">起始地址</param>
        /// <param name="count">读取字数</param>
        /// <returns></returns>
        public async Task<ushort[]> ReadWordsAsync(MemoryAreaCode memoryArea, ushort address, ushort count)
        {
            var request = new FinsRequest
            {
                Header = new FinsHeader
                {
                    ICF = 0x80,
                    RSV = 0x00,
                    GCT = 0x02,
                    DNA = 0x00,
                    DA2 = 0x00,
                    SNA = 0x00,
                    SA2 = 0x00
                },
                CommandCode = FinsCommandCode.MemoryAreaRead,
                SubCommandCode = 0x00,
                Data = new byte[]
                {
                    (byte)memoryArea,
                    (byte)(address >> 8),
                    (byte)(address & 0xFF),
                    0x00, // 位号
                    (byte)(count >> 8),
                    (byte)(count & 0xFF)
                }
            };
            
            var response = await SendRequestAsync(request);
            
            if (!response.IsSuccess)
                throw new Exception($"读取失败: 主响应代码={response.MainResponseCode}, 子响应代码={response.SubResponseCode}");
            
            var result = new ushort[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = (ushort)((response.Data[i * 2] << 8) | response.Data[i * 2 + 1]);
            }
            
            return result;
        }
        
        /// <summary>
        /// 写入内存区域数据（字单位）
        /// </summary>
        /// <param name="memoryArea">内存区域代码</param>
        /// <param name="address">起始地址</param>
        /// <param name="values">要写入的值</param>
        /// <returns></returns>
        public async Task<bool> WriteWordsAsync(MemoryAreaCode memoryArea, ushort address, ushort[] values)
        {
            var dataBytes = new List<byte>
            {
                (byte)memoryArea,
                (byte)(address >> 8),
                (byte)(address & 0xFF),
                0x00, // 位号
                (byte)(values.Length >> 8),
                (byte)(values.Length & 0xFF)
            };
            
            foreach (var value in values)
            {
                dataBytes.Add((byte)(value >> 8));
                dataBytes.Add((byte)(value & 0xFF));
            }
            
            var request = new FinsRequest
            {
                Header = new FinsHeader
                {
                    ICF = 0x80,
                    RSV = 0x00,
                    GCT = 0x02,
                    DNA = 0x00,
                    DA2 = 0x00,
                    SNA = 0x00,
                    SA2 = 0x00
                },
                CommandCode = FinsCommandCode.MemoryAreaWrite,
                SubCommandCode = 0x00,
                Data = dataBytes.ToArray()
            };
            
            var response = await SendRequestAsync(request);
            return response.IsSuccess;
        }
        
        /// <summary>
        /// 读取位数据
        /// </summary>
        /// <param name="memoryArea">内存区域代码</param>
        /// <param name="address">地址</param>
        /// <param name="bit">位号</param>
        /// <param name="count">读取位数</param>
        /// <returns></returns>
        public async Task<bool[]> ReadBitsAsync(MemoryAreaCode memoryArea, ushort address, byte bit, ushort count)
        {
            var request = new FinsRequest
            {
                Header = new FinsHeader
                {
                    ICF = 0x80,
                    RSV = 0x00,
                    GCT = 0x02,
                    DNA = 0x00,
                    DA2 = 0x00,
                    SNA = 0x00,
                    SA2 = 0x00
                },
                CommandCode = FinsCommandCode.MemoryAreaRead,
                SubCommandCode = 0x00,
                Data = new byte[]
                {
                    (byte)memoryArea,
                    (byte)(address >> 8),
                    (byte)(address & 0xFF),
                    bit,
                    (byte)(count >> 8),
                    (byte)(count & 0xFF)
                }
            };
            
            var response = await SendRequestAsync(request);
            
            if (!response.IsSuccess)
                throw new Exception($"读取失败: 主响应代码={response.MainResponseCode}, 子响应代码={response.SubResponseCode}");
            
            var result = new bool[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = response.Data[i] != 0;
            }
            
            return result;
        }
        
        /// <summary>
        /// 写入位数据
        /// </summary>
        /// <param name="memoryArea">内存区域代码</param>
        /// <param name="address">地址</param>
        /// <param name="bit">位号</param>
        /// <param name="values">要写入的值</param>
        /// <returns></returns>
        public async Task<bool> WriteBitsAsync(MemoryAreaCode memoryArea, ushort address, byte bit, bool[] values)
        {
            var dataBytes = new List<byte>
            {
                (byte)memoryArea,
                (byte)(address >> 8),
                (byte)(address & 0xFF),
                bit,
                (byte)(values.Length >> 8),
                (byte)(values.Length & 0xFF)
            };
            
            foreach (var value in values)
            {
                dataBytes.Add((byte)(value ? 0x01 : 0x00));
            }
            
            var request = new FinsRequest
            {
                Header = new FinsHeader
                {
                    ICF = 0x80,
                    RSV = 0x00,
                    GCT = 0x02,
                    DNA = 0x00,
                    DA2 = 0x00,
                    SNA = 0x00,
                    SA2 = 0x00
                },
                CommandCode = FinsCommandCode.MemoryAreaWrite,
                SubCommandCode = 0x00,
                Data = dataBytes.ToArray()
            };
            
            var response = await SendRequestAsync(request);
            return response.IsSuccess;
        }
        
        /// <summary>
        /// 读取CPU单元状态
        /// </summary>
        /// <returns></returns>
        public async Task<byte[]> ReadCpuUnitStatusAsync()
        {
            var request = new FinsRequest
            {
                Header = new FinsHeader
                {
                    ICF = 0x80,
                    RSV = 0x00,
                    GCT = 0x02,
                    DNA = 0x00,
                    DA2 = 0x00,
                    SNA = 0x00,
                    SA2 = 0x00
                },
                CommandCode = FinsCommandCode.CpuUnitStatusRead,
                SubCommandCode = 0x00,
                Data = new byte[0]
            };
            
            var response = await SendRequestAsync(request);
            
            if (!response.IsSuccess)
                throw new Exception($"读取CPU状态失败: 主响应代码={response.MainResponseCode}, 子响应代码={response.SubResponseCode}");
            
            return response.Data;
        }
          /// <summary>
        /// 发送FINS请求
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="timeout">超时时间</param>
        /// <returns></returns>
        private async Task<FinsResponse> SendRequestAsync(FinsRequest request, int timeout = 5000)
        {
            if (_isUdp)
            {
                if (_udpClient == null)
                    throw new InvalidOperationException("UDP客户端未初始化");
                return await _udpClient.SendRequestAsync(request, timeout);
            }
            else
            {
                if (_tcpClient == null)
                    throw new InvalidOperationException("TCP客户端未初始化");
                return await _tcpClient.SendRequestAsync(request, timeout);
            }
        }
        
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            _udpClient?.Dispose();
            _tcpClient?.Dispose();
        }
    }
}
