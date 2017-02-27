namespace YmtSystem.Repository.EF
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using YmtSystem.Domain.Repository;
    using YmtSystem.Domain.Shard;

    public partial class EFRepository<TEntity> : IRepository<TEntity>
         where TEntity : class
    {
        public Task<TEntity> FindOneAsync(CancellationToken token, params object[] key)
        {
            return this.unitofwork.CreateSet<TEntity>().FindAsync(token, key);
        }
        public virtual async Task<TEntity> FindOneAsync(TimeSpan timeOut, params object[] key)
        {
            return await this.unitofwork.CreateSet<TEntity>().FindAsync(key).WithTimeout(timeOut);
        }

        public virtual Task<IEnumerable<TEntity>> FindAllAsync<TSortKey>(Expression<Func<TEntity, TSortKey>> sortPredicate, SortOrder sortOrder)
        {
            throw new NotImplementedException();
        }
    }
}
