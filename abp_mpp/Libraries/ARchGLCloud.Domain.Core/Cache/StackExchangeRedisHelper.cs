using StackExchange.Redis;
using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

namespace ARchGLCloud.Domain.Core.Cache
{
    /// <summary>
    /// StackExchangeRedisHelper
    /// 
    /// 在StackExchange.Redis中最重要的对象是ConnectionMultiplexer类， 它存在于StackExchange.Redis命名空间中。
    /// 这个类隐藏了Redis服务的操作细节，ConnectionMultiplexer类做了很多东西， 在所有调用之间它被设计为共享和重用的。
    /// 不应该为每一个操作都创建一个ConnectionMultiplexer 。 ConnectionMultiplexer是线程安全的 ， 推荐使用下面的方法。
    /// 在所有后续示例中 ， 都假定你已经实例化好了一个ConnectionMultiplexer类，它将会一直被重用 ，
    /// 现在我们来创建一个ConnectionMultiplexer实例。它是通过ConnectionMultiplexer.Connect 或者 ConnectionMultiplexer.ConnectAsync，
    /// 传递一个连接字符串或者一个ConfigurationOptions 对象来创建的。
    /// 连接字符串可以是以逗号分割的多个服务的节点.
    /// 
    /// 
    /// 注意 : 
    /// ConnectionMultiplexer 实现了IDisposable接口当我们不再需要是可以将其释放的 , 这里我故意不使用 using 来释放他。 
    /// 简单来讲创建一个ConnectionMultiplexer是十分昂贵的 ， 一个好的主意是我们一直重用一个ConnectionMultiplexer对象。
    /// 一个复杂的的场景中可能包含有主从复制 ， 对于这种情况，只需要指定所有地址在连接字符串中（它将会自动识别出主服务器）
    ///  ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("server1:6379,server2:6379");
    /// 假设这里找到了两台主服务器，将会对两台服务进行裁决选出一台作为主服务器来解决这个问题 ， 这种情况是非常罕见的 ，我们也应该避免这种情况的发生。
    /// 
    /// 
    /// 这里有个和 ServiceStack.Redis 大的区别是没有默认的连接池管理了。没有连接池自然有其利弊,最大的好处在于等待获取连接的等待时间没有了,
    /// 也不会因为连接池里面的连接由于没有正确释放等原因导致无限等待而处于死锁状态。缺点在于一些低质量的代码可能导致服务器资源耗尽。不过提供连接池等阻塞和等待的手段是和作者的设计理念相违背的。StackExchange.Redis这里使用管道和多路复用的技术来实现减少连接
    /// 
    /// 参考：http://www.cnblogs.com/Leo_wl/p/4968537.html
    /// 
    /// 修改记录
    /// 
    ///        2016.04.07 版本：1.0 SongBiao    主键创建。
    ///        
    /// <author>
    ///        <name>SongBiao</name>
    ///        <date>2016.04.07</date>
    /// </author>
    /// </summary>
    public static class StackExchangeRedisHelper
    {
        private static readonly object _locker = new object();
        private static ConnectionMultiplexer _instance = null;
        public static string ConnctString { get; set; }

        static StackExchangeRedisHelper()
        {
        }

        /// <summary>
        /// 使用一个静态属性来返回已连接的实例，如下列中所示。这样，一旦 ConnectionMultiplexer 断开连接，便可以初始化新的连接实例。
        /// </summary>
        public static ConnectionMultiplexer Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null || !_instance.IsConnected)
                        {
                            ThreadPool.SetMinThreads(200, 200);
                            _instance = ConnectionMultiplexer.Connect(ConnctString);
                            //注册如下事件
                            _instance.ConnectionFailed += MuxerConnectionFailed;
                            _instance.ConnectionRestored += MuxerConnectionRestored;
                            _instance.ErrorMessage += MuxerErrorMessage;
                            _instance.ConfigurationChanged += MuxerConfigurationChanged;
                            _instance.HashSlotMoved += MuxerHashSlotMoved;
                            _instance.InternalError += MuxerInternalError;
                        }
                    }
                }

                return _instance;
            }
        }
        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <returns></returns>
        public static IDatabase GetDatabase(int dbNumber)
        {
            return Instance.GetDatabase(dbNumber);
        }
        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        static byte[] Serialize(object o)
        {
            if (o == null)
            {
                return null;
            }
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, o);
                byte[] objectDataAsStream = memoryStream.ToArray();
                return objectDataAsStream;
            }
        }
        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        static T Deserialize<T>(byte[] stream)
        {
            if (stream == null)
            {
                return default;
            }
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream(stream))
            {
                T result = (T)binaryFormatter.Deserialize(memoryStream);
                return result;
            }
        }
        /// <summary>
        /// 配置更改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConfigurationChanged(object sender, EndPointEventArgs e)
        {
        }
        /// <summary>
        /// 发生错误时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerErrorMessage(object sender, RedisErrorEventArgs e)
        {
        }
        /// <summary>
        /// 重新建立连接之前的错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
        }
        /// <summary>
        /// 连接失败 ， 如果重新连接成功你将不会收到这个通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
        }
        /// <summary>
        /// 更改集群
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerHashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
        }
        /// <summary>
        /// redis类库错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerInternalError(object sender, InternalErrorEventArgs e)
        {
        }
        ///场景不一样，选择的模式便会不一样，大家可以按照自己系统架构情况合理选择长连接还是Lazy。
        ///建立连接后，通过调用ConnectionMultiplexer.GetDatabase 方法返回对 Redis Cache 数据库的引用。
        ///从 GetDatabase 方法返回的对象是一个轻量级直通对象，不需要进行存储。
        private readonly static Lazy<ConnectionMultiplexer> lazyConnection =
        new Lazy<ConnectionMultiplexer>(() =>
        {
            ThreadPool.SetMinThreads(200, 200);
            _instance = ConnectionMultiplexer.Connect(ConnctString);
            return _instance;
            //return ConnectionMultiplexer.Connect(new ConfigurationOptions
            //{
            //    AbortOnConnectFail = false,
            //    Ssl = true,
            //    ConnectRetry = 3,
            //    ConnectTimeout = 5000,
            //    SyncTimeout = 5000,
            //    DefaultDatabase = 0,
            //    EndPoints = { { "192.168.6.11:6379", 0 } },
            //    Password = "P@ssw0rd"
            //});
        });

        public static ConnectionMultiplexer Connection => lazyConnection.Value;


        #region  当作消息代理中间件使用 一般使用更专业的消息队列来处理这种业务场景
        /// <summary>
        /// 当作消息代理中间件使用
        /// 消息组建中,重要的概念便是生产者,消费者,消息中间件。
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static long Publish(string channel, string message)
        {
            ISubscriber sub = Instance.GetSubscriber();
            //return sub.Publish("messages", "hello");
            return sub.Publish(channel, message);
        }

        /// <summary>
        /// 在消费者端得到该消息并输出
        /// </summary>
        /// <param name="channelFrom"></param>
        /// <returns></returns>
        public static void Subscribe(string channelFrom)
        {
            ISubscriber sub = Instance.GetSubscriber();
            sub.Subscribe(channelFrom, (channel, message) =>
            {
                Console.WriteLine((string)message);
            });
        }
        #endregion

        /// <summary>
        /// GetServer方法会接收一个EndPoint类或者一个唯一标识一台服务器的键值对
        /// 有时候需要为单个服务器指定特定的命令
        /// 使用IServer可以使用所有的shell命令，比如：
        /// DateTime lastSave = server.LastSave();
        /// ClientInfo[] clients = server.ClientList();
        /// 如果报错在连接字符串后加 ,allowAdmin=true;
        /// </summary>
        /// <returns></returns>
        public static IServer GetServer(string host, int port)
        {
            IServer server = Instance.GetServer(host, port);
            return server;
        }

        /// <summary>
        /// 获取全部终结点
        /// </summary>
        /// <returns></returns>
        public static EndPoint[] GetEndPoints()
        {
            EndPoint[] endpoints = Instance.GetEndPoints();
            return endpoints;
        }

    }
}