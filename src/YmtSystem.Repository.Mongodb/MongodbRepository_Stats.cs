namespace YmtSystem.Repository.Mongodb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using YmtSystem.Domain.MongodbRepository;

    public partial class MongodbRepository<TEntity> : IMongodbRepository<TEntity>
       where TEntity : class
    {
        public virtual MapReduceResult MapRedurce(MapReduceArgs args) 
        {
            return this.context.GetCollection<TEntity>().MapReduce(args);
        }
        public virtual IEnumerable<TResult> MapRedurce<TResult>(MapReduceArgs args)
        {
            return this.context.GetCollection<TEntity>().MapReduce(args).GetResultsAs<TResult>();
        }
        public virtual IEnumerable<BsonDocument> Group(GroupArgs args)
        {
            return this.context.GetCollection<TEntity>().Group(args);
        }
        public virtual IEnumerable<BsonDocument> Aggregation(AggregateArgs args)
        {
            return this.context.GetCollection<TEntity>().Aggregate(args);
        }
    }
}
