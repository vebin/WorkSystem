namespace YmtSystem.Domain.MongodbRepository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MongoDB.Driver;

    public partial interface IMongodbRepository<TEntity> where TEntity : class
    {
        void BatchAdd(IEnumerable<TEntity> documents, WriteConcern writeConcern);
        void Save(TEntity document, WriteConcern writeConcern = null);
        WriteConcernResult Add(TEntity document, MongoInsertOptions iOptions = null);
        void Add(TEntity document);
        WriteConcernResult Add(TEntity document, WriteConcern writeConcern = null);
        long? Remove(IMongoQuery query, WriteConcern writeConcern = null);
        void Remove(IMongoQuery query);
        Tuple<TResult, int> FindAndRemove<TResult>(IMongoQuery query, IMongoSortBy order);
        Tuple<TResult, int> FindAndRemove<TResult>(FindAndRemoveArgs args);
        Tuple<TResult, int> FindAndModify<TResult>(IMongoQuery query, IMongoUpdate update, bool upSet = true);
        Tuple<TResult, int> FindAndModify<TResult>(FindAndModifyArgs args);
        WriteConcernResult Update(IMongoQuery query, IMongoUpdate update);
        WriteConcernResult Update(IMongoQuery query, IMongoUpdate update, MongoUpdateOptions options);
        WriteConcernResult Update(IMongoQuery query, IMongoUpdate update, string dbName, string collectionName);
        WriteConcernResult Update(IMongoQuery query, IMongoUpdate update, MongoUpdateOptions options, string dbName, string collectionName);
        void CreateCappedCollection(string dbName, string collectionName, long maxSize, long maxDocNum,bool dropExistsCollection=true);
        void CreateCappedCollection(long maxSize, long maxDocNum, bool dropExistsCollection = true);
    }
}
