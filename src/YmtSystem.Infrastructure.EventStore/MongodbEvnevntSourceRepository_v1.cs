namespace YmtSystem.Infrastructure.EventStore.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using MongoDB.Driver.Linq;
    using MongoDB.Bson;
    using MongoDB.Driver.Builders;
    using MongoDB.Driver;
    using YmtSystem.CrossCutting;
    using YmtSystem.Infrastructure.EventStore.Repository.Register;

    public class MongodbEvnevntSourceRepository<TEntity> : DbContext, IMongodbEvnevntSourceRepository<TEntity>
        where TEntity : class
    {
        public EntityMappingConfigure MappingCfg { get { return EntityMappingSource.GetMapping(typeof(TEntity)); } }

        #region [ 不指定数据库表 ]
        public TEntity FindById<TKey>(TKey id)
        {
            return GetTypeCollection<TEntity>(MappingCfg.ToDatabase, MappingCfg.ToCollection).FindOneById(BsonValue.Create(id));
        }
        public TEntity FindOne(Expression<Func<TEntity, bool>> exp)
        {
            return GetTypeCollection<TEntity>(MappingCfg.ToDatabase, MappingCfg.ToCollection).AsQueryable().Where(exp).FirstOrDefault();
        }
        public TEntity FindAndModify(IMongoQuery query, IMongoUpdate update)
        {
            return GetTypeCollection<TEntity>(MappingCfg.ToDatabase, MappingCfg.ToCollection)
                .FindAndModify(query, SortBy.Null, update, true, true).GetModifiedDocumentAs<TEntity>();
        }
        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> exp)
        {
            return GetTypeCollection<TEntity>(MappingCfg.ToDatabase, MappingCfg.ToCollection).AsQueryable().Where(exp);
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> exp, int index, int limit)
        {
            return GetTypeCollection<TEntity>(MappingCfg.ToDatabase, MappingCfg.ToCollection)
                    .AsQueryable()
                    .Where(exp)
                    .Skip(index * limit)
                    .Take(limit);
        }

        public IQueryable<TEntity> Find<TSortKey>(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, TSortKey>> orderExp, System.Data.SqlClient.SortOrder order, int index, int limit)
        {
            var query = GetTypeCollection<TEntity>(MappingCfg.ToDatabase, MappingCfg.ToCollection)
                                                                                                .AsQueryable()
                                                                                                .Where(exp);
            if (order == System.Data.SqlClient.SortOrder.Ascending)
                return query.OrderBy(orderExp)
                            .Skip(index * limit)
                            .Take(limit);
            else if (order == System.Data.SqlClient.SortOrder.Descending)
                return query.OrderByDescending(orderExp)
                           .Skip(index * limit)
                           .Take(limit);
            else
                return query.OrderByDescending(orderExp)
                           .Skip(index * limit)
                           .Take(limit);
        }
        public void Add(TEntity entity)
        {
            GetTypeCollection<TEntity>(MappingCfg.ToDatabase, MappingCfg.ToCollection).Insert(entity);
        }

        public void Save(TEntity entity)
        {
            GetTypeCollection<TEntity>(MappingCfg.ToDatabase, MappingCfg.ToCollection).Save(entity);
        }

        public void AddRang(IEnumerable<TEntity> entity)
        {
            GetTypeCollection<TEntity>(MappingCfg.ToDatabase, MappingCfg.ToCollection).InsertBatch(entity);
        }
        public void Remove(string name, BsonValue value)
        {
            GetTypeCollection<TEntity>(MappingCfg.ToDatabase, MappingCfg.ToCollection).Remove(Query.EQ(name, value));
        }
        public void Remove(Expression<Func<TEntity, bool>> exp)
        {
            GetTypeCollection<TEntity>(MappingCfg.ToDatabase, MappingCfg.ToCollection)
                .AsQueryable()
                .Where(exp)
                .Each(e => Remove("_id", null));
        }

        public bool Exists(Expression<Func<TEntity, bool>> exp)
        {
            return GetTypeCollection<TEntity>(MappingCfg.ToDatabase, MappingCfg.ToCollection).AsQueryable().Any(exp);
        }

        public long Count(Expression<Func<TEntity, bool>> exp)
        {
            return GetTypeCollection<TEntity>(MappingCfg.ToDatabase, MappingCfg.ToCollection)
                   .AsQueryable()
                   .Where(exp)
                   .Count();
        }

        #region   [ 基于mongo Query ]
        public IQueryable<TEntity> Find(IMongoQuery query, IMongoFields fields)
        {
            return GetTypeCollection<TEntity>(MappingCfg.ToDatabase, MappingCfg.ToCollection).Find(query).SetFields(fields).AsQueryable();
        }
        public TEntity FindOne(IMongoQuery query)
        {
            return GetTypeCollection<TEntity>(MappingCfg.ToDatabase, MappingCfg.ToCollection).FindOne(query);
        }
        public IQueryable<TEntity> Find(IMongoQuery query, IMongoFields fields, int index, int limit)
        {
            return GetTypeCollection<TEntity>(MappingCfg.ToDatabase, MappingCfg.ToCollection)
                        .Find(query)
                        .SetFields(fields)
                        .SetSkip(index * limit)
                        .SetLimit(limit)
                        .AsQueryable();
        }
        public IQueryable<TEntity> Find<TSortKey>(IMongoQuery query, IMongoSortBy sort, IMongoFields fields, int index, int limit)
        {
            return GetTypeCollection<TEntity>(MappingCfg.ToDatabase, MappingCfg.ToCollection)
                    .Find(query)
                    .SetFields(fields)
                    .SetSortOrder(sort)
                    .SetSkip(index * limit)
                    .SetLimit(limit)
                    .AsQueryable();
        }
        public bool Remove(IMongoQuery query)
        {
            return GetTypeCollection<TEntity>(MappingCfg.ToDatabase, MappingCfg.ToCollection).Remove(query).Ok;
        }
        public bool Exists(IMongoQuery query)
        {
            return GetTypeCollection<TEntity>(MappingCfg.ToDatabase, MappingCfg.ToCollection).Find(query).Any();
        }
        public long Count(IMongoQuery query)
        {
            return GetTypeCollection<TEntity>(MappingCfg.ToDatabase, MappingCfg.ToCollection).Count(query);
        }
        public void Update(IMongoQuery query, IMongoUpdate update)
        {
            GetTypeCollection<TEntity>(MappingCfg.ToDatabase, MappingCfg.ToCollection)
                .Update(query, update, new MongoUpdateOptions { Flags = UpdateFlags.Upsert });
        }
        #endregion
        public IEnumerable<TResult> MarpRedurce<TResult>(IMongoQuery query, IMongoSortBy sortBy, string javaScriptMapFun, string javaScriptReduceFun)
        {
            var map = MapReduceOptions.SetOutput(new MapReduceOutput { Mode = MapReduceOutputMode.Replace, CollectionName = "mr" })
                                      .SetSortOrder(sortBy);
            return GetTypeCollection<TEntity>(MappingCfg.ToDatabase, MappingCfg.ToCollection)
                 .MapReduce(query, javaScriptMapFun, javaScriptReduceFun, map).GetResultsAs<TResult>();
        }
        #endregion

        #region [ 指定数据库，表 ]
        public TEntity FindById<TKey>(TKey id, string dbName, string collectionName)
        {
            return GetTypeCollection<TEntity>(dbName, collectionName).FindOneById(BsonValue.Create(id));
        }
        public TEntity FindOne(Expression<Func<TEntity, bool>> exp, string dbName, string collectionName)
        {
            return GetTypeCollection<TEntity>(dbName, collectionName).AsQueryable().FirstOrDefault(exp);
        }
        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> exp, string dbName, string collectionName)
        {
            return GetTypeCollection<TEntity>(dbName, collectionName).AsQueryable().Where(exp);
        }
        public void Add(TEntity entity, string dbName, string collectionName)
        {
            GetTypeCollection<TEntity>(dbName, collectionName).Insert(entity);
        }
        public void Update(IMongoQuery query, IMongoUpdate update, string dbName, string collectionName)
        {
            GetTypeCollection<TEntity>(dbName, collectionName)
                .Update(query, update, new MongoUpdateOptions { Flags = UpdateFlags.Upsert });
        }
        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> exp, int index, int limit, string dbName, string collectionName)
        {
            throw new NotImplementedException();
        }

        public void AddRang(IEnumerable<TEntity> entity, string dbName, string collectionName)
        {
            throw new NotImplementedException();
        }

        public void Remove(Expression<Func<TEntity, bool>> exp, string dbName, string collectionName)
        {
            throw new NotImplementedException();
        }

        public bool Exists(Expression<Func<TEntity, bool>> exp, string dbName, string collectionName)
        {
            return GetTypeCollection<TEntity>(dbName, collectionName).AsQueryable().Any(exp);
        }

        public long Count(Expression<Func<TEntity, bool>> exp, string dbName, string collectionName)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region [ async ]
        public void AsyncAdd(TEntity entity)
        {
            Task.Factory.StartNew(() => GetTypeCollection<TEntity>(MappingCfg.ToDatabase, MappingCfg.ToCollection).Insert(entity));
        }
        public void AsyncAdd(TEntity entity, string dbName, string collectionName)
        {
            Task.Factory.StartNew(() => GetTypeCollection<TEntity>(dbName, collectionName).Insert(entity));
        }
        #endregion
    }
}
