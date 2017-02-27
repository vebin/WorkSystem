namespace YmtSystem.Domain.MongodbRepository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MongoDB.Bson;
    using MongoDB.Driver;

    partial interface IMongodbRepository<TEntity> where TEntity : class
    {
        MapReduceResult MapRedurce(MapReduceArgs args);
        IEnumerable<TResult> MapRedurce<TResult>(MapReduceArgs args);
        IEnumerable<BsonDocument> Group(GroupArgs args);
        IEnumerable<BsonDocument> Aggregation(AggregateArgs args);
    }
}
