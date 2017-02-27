using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using YmtSystem.CrossCutting;
namespace YmtSystem.Infrastructure.EventStore
{
    public class DbContext : IDisposable
    {
        private MongoDatabase db;

        public DbContext()
        {
            db = ServerContext.Context.GetDatabase(Configure.MongoDatabaseName);
        }

        public MongoCollection<T> GetTypeCollection<T>(string collectionName)
        {
            return db.GetCollection<T>(collectionName);
        }
        public MongoCollection GetCollection(string collectionName)
        {
            return db.GetCollection(collectionName);
        }
        /// <summary>
        /// 获取强类型集合
        /// </summary>       
        public MongoCollection<T> GetTypeCollection<T>(string databaseName, string collectionName)
        {
            return ServerContext.Context.GetDatabase(databaseName).GetCollection<T>(collectionName);
        }
        /// <summary>
        /// Mongo 系统集合
        /// </summary>  
        public MongoCollection GetCollection(string databaseName, string collectionName)
        {
            return ServerContext.Context.GetDatabase(databaseName).GetCollection(collectionName);
        }       
        public void Dispose()
        {
            ServerContext.Context.Disconnect();
        }
        public static void Clear()
        {
            try
            {              
                pool.Dispose();
            }
            catch
            {
            }
        }
        private static readonly ThreadLocal<MongoConext> pool = new ThreadLocal<MongoConext>();
        //private static MongoConext server;
        /// <summary>
        ///  服务上下文
        /// </summary>
        //public MongoConext ServerContext { get { return LocalServiceLocator.GetService<MongoConext>(); } }
        public MongoConext ServerContext { get { if (pool.Value == null)pool.Value = new MongoConext(); return pool.Value; } }
    }
}
