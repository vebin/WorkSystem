namespace YmtSystem.Repository.Mongodb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;
    using YmtSystem.Domain.MongodbRepository;

    public partial class MongodbRepository<TEntity> : IMongodbRepository<TEntity>
       where TEntity : class
    {
        public virtual IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> exp)
        {
            return this.context.GetCollection<TEntity>().AsQueryable().Where(exp);
        }      
        public virtual IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> exp, string dbName, string collectionName)
        {
            return this.context.GetCollection<TEntity>(dbName, collectionName).AsQueryable().Where(exp);
        }
    }
}
