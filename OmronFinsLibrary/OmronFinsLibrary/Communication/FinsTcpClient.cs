using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using OmronFinsLibrary.Models;

namespace OmronFinsLibrary.Communication
{
    /// <summary>
    /// FINS TCP通信类
    /// </summary>
    public class FinsTcpClient : IDisposable
    {
        private TcpClient? _tcpClient;
        private NetworkStream? _stream;
        private byte[] _buffer = new byte[4096];
        private byte _localNodeAddress;
        private byte _remoteNodeAddress;
        private byte _serviceId = 0;
        private readonly object _lock = new object();
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="remoteIp">远程IP地址</param>
        /// <param name="remotePort">远程端口，默认9600</param>
        /// <param name="localNodeAddress">本地节点地址</param>
        /// <param name="remoteNodeAddress">远程节点地址</param>
        public FinsTcpClient(string remoteIp, int remotePort = 9600, byte localNodeAddress = 0x01, byte remoteNodeAddress = 0x01)
        {
            RemoteIp = remoteIp;
            RemotePort = remotePort;
            _localNodeAddress = localNodeAddress;
            _remoteNodeAddress = remoteNodeAddress;
        }
        
        /// <summary>
        /// 远程IP地址
        /// </summary>
        public string RemoteIp { get; }
        
        /// <summary>
        /// 远程端口
        /// </summary>
        public int RemotePort { get; }
        
        /// <summary>
        /// 是否已连接
        /// </summary>
        public bool IsConnected => _tcpClient?.Connected ?? false;
        
        /// <summary>
        /// 连接到PLC
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ConnectAsync()
        {
            try
            {
                _tcpClient = new TcpClient();
                await _tcpClient.ConnectAsync(RemoteIp, RemotePort);
                _stream = _tcpClient.GetStream();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"连接失败: {ex.Message}", ex);
            }
        }
        
        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            _stream?.Close();
            _tcpClient?.Close();
        }
        
        /// <summary>
        /// 发送FINS请求并接收响应
        /// </summary>
        /// <param name="request">FINS请求</param>
        /// <param name="timeout">超时时间（毫秒）</param>
        /// <returns></returns>
        public async Task<FinsResponse> SendRequestAsync(FinsRequest request, int timeout = 5000)
        {
            if (!IsConnected)
                throw new InvalidOperationException("未连接到PLC");
                
            if (_stream == null)
                throw new InvalidOperationException("网络流未初始化");
                
            lock (_lock)
            {
                _serviceId = (byte)((_serviceId + 1) % 256);
                request.Header.SID = _serviceId;
                request.Header.SA1 = _localNodeAddress;
                request.Header.DA1 = _remoteNodeAddress;
            }
            
            try
            {
                // 发送请求
                var requestData = request.ToByteArray();
                
                // TCP需要发送长度前缀
                var lengthPrefix = BitConverter.GetBytes((uint)requestData.Length);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(lengthPrefix);
                    
                await _stream.WriteAsync(lengthPrefix, 0, 4);
                await _stream.WriteAsync(requestData, 0, requestData.Length);
                await _stream.FlushAsync();
                
                // 接收响应长度
                if (_tcpClient != null)
                    _tcpClient.ReceiveTimeout = timeout;
                var lengthBytes = new byte[4];
                await _stream.ReadExactAsync(lengthBytes, 0, 4);
                
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(lengthBytes);
                var responseLength = BitConverter.ToUInt32(lengthBytes, 0);
                
                // 接收响应数据
                var responseData = new byte[responseLength];
                await _stream.ReadExactAsync(responseData, 0, (int)responseLength);
                
                return FinsResponse.FromByteArray(responseData);
            }
            catch (Exception ex)
            {
                throw new Exception($"通信失败: {ex.Message}", ex);
            }
        }
        
        /// <summary>
        /// 创建基本的FINS请求头
        /// </summary>
        /// <returns></returns>
        protected FinsHeader CreateFinsHeader()
        {
            return new FinsHeader
            {
                ICF = 0x80,
                RSV = 0x00,
                GCT = 0x02,
                DNA = 0x00,
                DA1 = _remoteNodeAddress,
                DA2 = 0x00,
                SNA = 0x00,
                SA1 = _localNodeAddress,
                SA2 = 0x00,
                SID = _serviceId
            };
        }
        
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Disconnect();
            _stream?.Dispose();
            _tcpClient?.Dispose();
        }
    }
    
    /// <summary>
    /// NetworkStream扩展方法
    /// </summary>
    public static class NetworkStreamExtensions
    {
        /// <summary>
        /// 读取指定长度的数据
        /// </summary>
        /// <param name="stream">网络流</param>
        /// <param name="buffer">缓冲区</param>
        /// <param name="offset">偏移量</param>
        /// <param name="count">要读取的字节数</param>
        /// <returns></returns>
        public static async Task ReadExactAsync(this NetworkStream stream, byte[] buffer, int offset, int count)
        {
            int totalRead = 0;
            while (totalRead < count)
            {
                int bytesRead = await stream.ReadAsync(buffer, offset + totalRead, count - totalRead);
                if (bytesRead == 0)
                    throw new EndOfStreamException("连接意外关闭");
                totalRead += bytesRead;
            }
        }
    }
}
