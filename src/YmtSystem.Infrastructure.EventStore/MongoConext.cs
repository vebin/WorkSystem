using System;
using MongoDB.Driver;

namespace YmtSystem.Infrastructure.EventStore
{
    public class MongoConext : IDisposable
    {
        private MongoServer server;
        /// <summary>
        /// 初始化
        /// </summary>
        public MongoConext()
        {
            InitMongoServer();
        }
       
        /// <summary>
        /// 当前服务实例数量
        /// </summary>
        public int CurrentServerInstanceCount { get { return MongoServer.ServerCount; } }
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            this.server.Disconnect();
        }
        /// <summary>
        /// 当前服务上下文
        /// </summary>
        public MongoServer Context { get { return server; } }

        private void InitMongoServer()
        {
            try
            {
                server = new MongoClient(MongoClientSettings.FromUrl(MongoUrl.Create(Configure.MongoUrl))).GetServer();
            }
            catch (Exception ex)
            {
                YmtSystem.CrossCutting.YmatouLoggingService.Error("初始化mongo错误 {0}", ex.ToString());
                throw;
            }
        }
    }
}
