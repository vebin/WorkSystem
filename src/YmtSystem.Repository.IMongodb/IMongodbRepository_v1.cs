namespace YmtAuth.Domain.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using MongoDB.Driver;

    public interface IMongodbRepository_v1<TEntity> where TEntity : class
    {
        #region [ 不指定数据库，表 ]
        TEntity FindById<TKey>(TKey id);
        TEntity FindOne(Expression<Func<TEntity, bool>> exp);
        TEntity FindAndModify(IMongoQuery query, IMongoUpdate update);
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> exp);
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> exp, int index, int limit);
        IQueryable<TEntity> FindAll(string dbName, string collectionName, int index, int limit);
        IQueryable<TEntity> Find<TSortKey>(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, TSortKey>> orderExp, System.Data.SqlClient.SortOrder order, int index, int limit);
        void Add(TEntity entity);

        void Save(TEntity entity);
        void AddRang(IEnumerable<TEntity> entity);
        void Remove<TKey>(TKey id);
        void Remove(Expression<Func<TEntity, bool>> exp);
        bool Exists(Expression<Func<TEntity, bool>> exp);
        long Count(Expression<Func<TEntity, bool>> exp);
        #region [基于Query 查询]
        TEntity FindOne(IMongoQuery query, bool isSingleOrDefault = false);
        IQueryable<TEntity> Find(IMongoQuery query);
        IQueryable<TEntity> Find(IMongoQuery query, IMongoFields fields);
        IQueryable<TEntity> Find(IMongoQuery query, IMongoFields fields, int index, int limit);
        IQueryable<TEntity> Find<TSortKey>(IMongoQuery query, IMongoSortBy sort, IMongoFields fields, int index, int limit);
        bool Remove(IMongoQuery query);
        bool Exists(IMongoQuery query);
        long Count(IMongoQuery query);
        void Update(IMongoQuery query, IMongoUpdate update);
        #endregion
        IEnumerable<TResult> MarpRedurce<TResult>(IMongoQuery query, IMongoSortBy sortBy, string javaScriptMapFun, string javaScriptReduceFun);
        #endregion
        #region [Async]
        void AsyncAdd(TEntity entity, Action<AggregateException> errHandle = null);
        void AsyncAdd(TEntity entity, string dbName, string collectionName, Action<AggregateException> errHandle = null);
        Task<bool> AsyncRemove(IMongoQuery quer);
        #endregion
        #region [ 指定数据库，表 ]
        TEntity FindById<TKey>(TKey id, string dbName, string collectionName);
        TEntity FindOne(Expression<Func<TEntity, bool>> exp, string dbName, string collectionName);
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> exp, string dbName, string collectionName);
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> exp, int index, int limit, string dbName, string collectionName);
        void Add(TEntity entity, string dbName, string collectionName);
        void AddRang(IEnumerable<TEntity> entity, string dbName, string collectionName);
        void Remove(Expression<Func<TEntity, bool>> exp, string dbName, string collectionName);
        bool Exists(Expression<Func<TEntity, bool>> exp, string dbName, string collectionName);
        long Count(Expression<Func<TEntity, bool>> exp, string dbName, string collectionName);
        void Update(IMongoQuery query, IMongoUpdate update, string dbName, string collectionName);
        void BulkUpdate(Action<BulkWriteOperation> command, bool order, string dbName, string collectionName, Action<BulkWriteResult> resultHandler = null, Action<Exception> errHandle = null);
        #endregion
    }
}
