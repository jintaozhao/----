using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using OmronFinsLibrary.Models;

namespace OmronFinsLibrary.Communication
{
    /// <summary>
    /// FINS UDP通信类
    /// </summary>
    public class FinsUdpClient : IDisposable
    {
        private UdpClient _udpClient;
        private IPEndPoint _remoteEndPoint;
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
        public FinsUdpClient(string remoteIp, int remotePort = 9600, byte localNodeAddress = 0x01, byte remoteNodeAddress = 0x01)
        {
            _remoteEndPoint = new IPEndPoint(IPAddress.Parse(remoteIp), remotePort);
            _localNodeAddress = localNodeAddress;
            _remoteNodeAddress = remoteNodeAddress;
            _udpClient = new UdpClient();
        }
          /// <summary>
        /// 连接到PLC
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ConnectAsync()
        {
            try
            {
                // UDP是无连接协议，这里只是设置目标端点
                _udpClient.Connect(_remoteEndPoint);
                return await Task.FromResult(true);
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
            _udpClient?.Close();
        }
        
        /// <summary>
        /// 发送FINS请求并接收响应
        /// </summary>
        /// <param name="request">FINS请求</param>
        /// <param name="timeout">超时时间（毫秒）</param>
        /// <returns></returns>
        public async Task<FinsResponse> SendRequestAsync(FinsRequest request, int timeout = 5000)
        {
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
                await _udpClient.SendAsync(requestData, requestData.Length);
                
                // 接收响应
                _udpClient.Client.ReceiveTimeout = timeout;
                var result = await _udpClient.ReceiveAsync();
                
                return FinsResponse.FromByteArray(result.Buffer);
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
            _udpClient?.Dispose();
        }
    }
}
