namespace YmtSystem.Repository.Mongodb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using YmtAuth.Domain.Repository;
    using YmtSystem.CrossCutting;
    using YmtSystem.Domain.MongodbRepository;


    public partial class MongodbRepository<TEntity> : IMongodbRepository<TEntity>
        where TEntity : class
    {
        public virtual void BatchAdd(IEnumerable<TEntity> documents, WriteConcern writeConcern)
        {
            this.context.GetCollection<TEntity>().InsertBatch(documents, writeConcern);
        }
        public virtual WriteConcernResult Add(TEntity document, WriteConcern writeConcern = null)
        {
            if (writeConcern == null)
                return this.context.GetCollection<TEntity>().Insert(document);
            else return this.context.GetCollection<TEntity>().Insert(document, writeConcern);
        }
        public virtual void Add(TEntity document)
        {
            this.context.GetCollection<TEntity>().Insert(document);
        }
        public virtual WriteConcernResult Add(TEntity document, MongoInsertOptions iOptions = null)
        {
            if (iOptions == null)
                return this.context.GetCollection<TEntity>().Insert(document);
            return this.context.GetCollection<TEntity>().Insert(document, iOptions);
        }
        public virtual void Save(TEntity document, WriteConcern writeConcern = null)
        {
            if (writeConcern != null)
                this.context.GetCollection<TEntity>().Save(document, writeConcern);
            else
                this.context.GetCollection<TEntity>().Save(document);
        }
        public virtual long? Remove(IMongoQuery query, WriteConcern writeConcern = null)
        {
            if (writeConcern == null)
            {
                this.context.GetCollection<TEntity>().Remove(query);
                return 0L;
            }
            else
            {
                var result = this.context.GetCollection<TEntity>().Remove(query, writeConcern);
                return result.DocumentsAffected;
            }
        }
        public virtual void Remove(IMongoQuery query)
        {
            this.context.GetCollection<TEntity>().Remove(query);
        }
        public virtual Tuple<TResult, int> FindAndRemove<TResult>(IMongoQuery query, IMongoSortBy order)
        {
            var result = this.context.GetCollection<TEntity>().FindAndRemove(new FindAndRemoveArgs { Query = query, SortBy = order });
            return Tuple.Create(result.GetModifiedDocumentAs<TResult>(), result.ModifiedDocument.ElementCount);
        }
        public virtual Tuple<TResult, int> FindAndRemove<TResult>(FindAndRemoveArgs args)
        {
            var result = this.context.GetCollection<TEntity>().FindAndRemove(args);
            return Tuple.Create(result.GetModifiedDocumentAs<TResult>(), result.ModifiedDocument.ElementCount);
        }

        public virtual Tuple<TResult, int> FindAndModify<TResult>(IMongoQuery query, IMongoUpdate update, bool upSet = true)
        {
            var result = this.context.GetCollection<TEntity>().FindAndModify(new FindAndModifyArgs { Query = query, Update = update, Upsert = upSet });
            return Tuple.Create(result.GetModifiedDocumentAs<TResult>(), result.ModifiedDocument.ElementCount);
        }
        public virtual Tuple<TResult, int> FindAndModify<TResult>(FindAndModifyArgs args)
        {
            var result = this.context.GetCollection<TEntity>().FindAndModify(args);
            return Tuple.Create(result.GetModifiedDocumentAs<TResult>(), result.ModifiedDocument.ElementCount);
        }

        public virtual WriteConcernResult Update(IMongoQuery query, IMongoUpdate update)
        {
            return this.context.GetCollection<TEntity>().Update(query, update);
        }
        public virtual WriteConcernResult Update(IMongoQuery query, IMongoUpdate update, MongoUpdateOptions options)
        {
            if (options != null)
                return this.context.GetCollection<TEntity>().Update(query, update, options);
            return this.context.GetCollection<TEntity>().Update(query, update);
        }
        public virtual void CreateCappedCollection(string dbName, string collectionName, long maxSize, long maxDocNum, bool dropExistsCollection = true)
        {
            YmtSystemAssert.AssertArgumentRange(maxSize, 0L, long.MaxValue, "maxSize 超过范围");
            YmtSystemAssert.AssertArgumentRange(maxDocNum, 0L, long.MaxValue, "maxDocNum 超过范围");
           
            var exist = this.context.Database(dbName).CollectionExists(collectionName);
            if (dropExistsCollection && exist)
                this.context.Database(dbName).DropCollection(collectionName);
            else if (exist)
                throw new Exception<MongodbRepositoryException>("collectionName exists");
            
            var options = CollectionOptions.SetCapped(true).SetMaxSize(maxSize).SetMaxDocuments(maxDocNum);
            this.context.Database(dbName).CreateCollection(collectionName, options);
        }
        public virtual void CreateCappedCollection(long maxSize, long maxDocNum, bool dropExistsCollection = true)
        {
            YmtSystemAssert.AssertArgumentRange(maxSize, 0L, long.MaxValue, "maxSize 超过范围");
            YmtSystemAssert.AssertArgumentRange(maxDocNum, 0L, long.MaxValue, "maxDocNum 超过范围");
            
            var exist = this.context.GetCollection<TEntity>().Exists();
            if (dropExistsCollection && this.context.GetCollection<TEntity>().Exists())
                this.context.GetCollection<TEntity>().Drop();
            else if (exist)
                throw new Exception<MongodbRepositoryException>("collectionName exists");
            
            var options = CollectionOptions.SetCapped(true).SetMaxSize(maxSize).SetMaxDocuments(maxDocNum);
            this.context.GetCollection<TEntity>().Database.CreateCollection(context.GetMapCfg<TEntity>().ToCollection, options);
        }


        public WriteConcernResult Update(IMongoQuery query, IMongoUpdate update, string dbName, string collectionName)
        {
            throw new NotImplementedException();
        }

        public WriteConcernResult Update(IMongoQuery query, IMongoUpdate update, MongoUpdateOptions options, string dbName, string collectionName)
        {
            throw new NotImplementedException();
        }
    }
}
