namespace YmtSystem.Repository.Mongodb.Context
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Threading;
    using MongoDB.Driver;
    using MongoDB.Bson;
    using YmtSystem.CrossCutting;
    using YmtSystem.Domain.MongodbRepository;
    using YmtSystem.Repository.Mongodb.Mapping;
    using System.Reflection;
    using YmtSystem.CrossCutting.Utility;

    /// <summary>
    /// mongodb context
    /// </summary>
    public abstract class MongodbContext : IMongodbContext, IDisposable
    {
        private MongoClient client;
        private MongoServer server;
        private MongoDatabase dataBase;

        private readonly string contextName;
        private static readonly EntityClassMap mapList = new EntityClassMap();

        /// <summary>
        /// MongodbContext
        /// </summary>
        /// <param name="mongoUrl"></param>
        public MongodbContext(string mongoUrl)
        {           
            contextName = this.GetType().FullName;
            InitServer(mongoUrl);
            InitMapping();
        }
        /// <summary>
        /// 获取集合
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="dbName">dbName</param>
        /// <param name="collectionName">collectionName</param>
        /// <returns></returns>
        public MongoCollection<TEntity> GetCollection<TEntity>(string dbName, string collectionName)
        {
            YmtSystemAssert.AssertArgumentNotEmpty(dbName, "数据库名不能为空");
            YmtSystemAssert.AssertArgumentNotEmpty(collectionName, "集合名不能为空");

            return this.DbServer.GetDatabase(dbName).GetCollection<TEntity>(collectionName);
        }
        /// <summary>
        ///  获取集合
        /// </summary>
        /// <param name="dbName">dbName</param>
        /// <param name="collectionName">collectionName</param>
        /// <returns></returns>
        public MongoCollection<BsonDocument> GetCollection(string dbName, string collectionName)
        {
            YmtSystemAssert.AssertArgumentNotEmpty(dbName, "数据库名不能为空");
            YmtSystemAssert.AssertArgumentNotEmpty(collectionName, "集合名不能为空");

            return this.DbServer.GetDatabase(dbName).GetCollection(collectionName);
        }
        /// <summary>
        /// 获取集合
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public virtual MongoCollection<TEntity> GetCollection<TEntity>()
        {
            var cfg = GetMapCfg<TEntity>();
            return this.GetCollection<TEntity>(cfg.ToDatabase, cfg.ToCollection);
        }
        /// <summary>
        ///  Database
        /// </summary>
        /// <param name="dbName">dbName</param>
        /// <returns></returns>
        public MongoDatabase Database(string dbName)
        {
            YmtSystemAssert.AssertArgumentNotEmpty(dbName, "数据库名不能为空");
            return this.DbServer.GetDatabase(dbName);
        }
        /// <summary>
        /// GetMapCfg
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public EntityMappingConfigure GetMapCfg<TEntity>()
        {
            var cfg = mapList.GetMap(contextName).FirstOrDefault(e => e.MappType == typeof(TEntity));
            YmtSystemAssert.AssertArgumentNotNull(cfg, string.Format("实体{0}无映射", typeof(TEntity).FullName));
            return cfg;
        }
        /// <summary>
        /// mongodb server
        /// </summary>
        protected MongoServer DbServer
        {
            get
            {
                this.server = client.GetServer();
                YmtSystemAssert.AssertArgumentNotNull(this.server, "server 为空！");
                return this.server;
            }
        }
        protected MongoDatabase Database<TEntity>()
        {
            var cfg = GetMapCfg<TEntity>();
            YmtSystemAssert.AssertArgumentNotNull(cfg, string.Format("{0}未配置实体映射", typeof(TEntity)));
            return DbServer.GetDatabase(cfg.ToDatabase);
        }
        /// <summary>
        /// 创建实体映射
        /// </summary>
        /// <param name="map"></param>
        protected abstract void OnEntityMap(EntityClassMap map, string contextName);

        private void InitServer(string mongoUrl)
        {
            YmtSystemAssert.AssertArgumentNotNull(mongoUrl, "mongoUrl 为空！");
            Interlocked.CompareExchange(ref client, new MongoClient(MongoClientSettings.FromUrl(MongoUrl.Create(mongoUrl))), null);
        }
        private void InitMapping()
        {      
            //if (Interlocked.CompareExchange(ref mapList, new EntityClassMap(), null) == null)
            if (!mapList.IsinitMap(contextName))
            {
                this.OnEntityMap(mapList, contextName);
            }
        }
        public void Dispose()
        {

        }
    }
}
