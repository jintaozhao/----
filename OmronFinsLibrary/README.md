# FINS通信库使用文档

## 概述

这是一个用于与欧姆龙PLC进行FINS通信的C#库，支持UDP和TCP两种通信方式。

## 功能特性

- ✅ 支持UDP和TCP通信协议
- ✅ 支持读取/写入字数据
- ✅ 支持读取/写入位数据
- ✅ 支持多种内存区域（CIO、DM、EM等）
- ✅ 异步操作支持
- ✅ 完整的错误处理
- ✅ 资源自动释放

## 基本使用

### 1. 创建客户端

```csharp
// 创建UDP客户端
using var udpClient = FinsClient.CreateUdpClient("192.168.1.100", 9600, 0x01, 0x01);

// 创建TCP客户端
using var tcpClient = FinsClient.CreateTcpClient("192.168.1.100", 9600, 0x01, 0x01);
```

### 2. 连接到PLC

```csharp
await client.ConnectAsync();
```

### 3. 读取数据

```csharp
// 读取DM区域的字数据
var words = await client.ReadWordsAsync(MemoryAreaCode.DataMemory, 0, 10);

// 读取CIO区域的位数据
var bits = await client.ReadBitsAsync(MemoryAreaCode.CIO, 0, 0, 16);
```

### 4. 写入数据

```csharp
// 写入DM区域的字数据
await client.WriteWordsAsync(MemoryAreaCode.DataMemory, 0, new ushort[] { 100, 200, 300 });

// 写入CIO区域的位数据
await client.WriteBitsAsync(MemoryAreaCode.CIO, 0, 0, new bool[] { true, false, true });
```

## 完整示例

```csharp
using System;
using System.Threading.Tasks;
using OmronFinsLibrary;
using OmronFinsLibrary.Enums;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // 创建UDP客户端连接到PLC
            using var client = FinsClient.CreateUdpClient("192.168.1.100", 9600, 0x01, 0x01);
            
            // 连接到PLC
            await client.ConnectAsync();
            Console.WriteLine("已连接到PLC");
            
            // 读取DM0-DM9的数据
            var dmData = await client.ReadWordsAsync(MemoryAreaCode.DataMemory, 0, 10);
            Console.WriteLine($"读取到DM数据: {string.Join(", ", dmData)}");
            
            // 写入数据到DM100
            await client.WriteWordsAsync(MemoryAreaCode.DataMemory, 100, new ushort[] { 1234 });
            Console.WriteLine("写入DM100成功");
            
            // 读取CIO0.00-CIO0.15的位状态
            var cioData = await client.ReadBitsAsync(MemoryAreaCode.CIO, 0, 0, 16);
            Console.WriteLine($"CIO位状态: {string.Join(", ", cioData)}");
            
            // 设置CIO0.00为ON
            await client.WriteBitsAsync(MemoryAreaCode.CIO, 0, 0, new bool[] { true });
            Console.WriteLine("设置CIO0.00为ON");
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"错误: {ex.Message}");
        }
    }
}
```

## 支持的内存区域

| 内存区域 | 枚举值 | 说明 |
|---------|-------|------|
| CIO | MemoryAreaCode.CIO | 输入/输出继电器 |
| Work | MemoryAreaCode.Work | 内部继电器 |
| Holding | MemoryAreaCode.Holding | 保持继电器 |
| Auxiliary | MemoryAreaCode.Auxiliary | 辅助继电器 |
| DM | MemoryAreaCode.DataMemory | 数据内存 |
| EM | MemoryAreaCode.ExtendedMemory | 扩展数据内存 |
| Timer/Counter | MemoryAreaCode.TimerCounter | 定时器/计数器 |

## 错误处理

库会抛出以下类型的异常：

- `InvalidOperationException`: 当客户端未连接或未初始化时
- `Exception`: 通信错误或其他运行时错误

建议在使用时添加适当的try-catch块进行错误处理。

## 注意事项

1. 使用完毕后请记得调用`Dispose()`方法或使用`using`语句自动释放资源
2. UDP通信是无连接的，TCP通信需要建立连接
3. 确保PLC的FINS服务已启用并配置正确的端口
4. 节点地址需要与PLC的配置匹配

## 版本信息

- 当前版本: 1.0.0
- 支持的.NET版本: .NET 9.0+
- 测试通过的欧姆龙PLC型号: CP1H, CJ2M, NJ系列等

## 技术支持

如果在使用过程中遇到问题，请检查：

1. 网络连接是否正常
2. PLC的FINS设置是否正确
3. IP地址和端口号是否正确
4. 节点地址是否匹配
