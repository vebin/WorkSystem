namespace YmtSystem.Domain.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using YmtSystem.Domain.Shard;
    using YmtSystem.Domain.Specifications;

    public abstract partial class Repository<TEntity> : IRepository<TEntity>
      where TEntity : class
    {
        public abstract int Count(ISpecification<TEntity> specification);
        public abstract Task<int> AsyncCount(ISpecification<TEntity> specification);
        public abstract decimal? SyncSum(ISpecification<TEntity> specification, Expression<Func<TEntity, decimal?>> select);
        public abstract Task<int?> AsyncSum(ISpecification<TEntity> specification, Expression<Func<TEntity, int?>> select);
        public abstract IQueryable<IGrouping<TKey, TEntity>> Group<TKey>(ISpecification<TEntity> specification, Expression<Func<TEntity, TKey>> keySelect);
        public abstract Task<int?> AsyncSum(Expression<Func<TEntity, bool>> specification, Expression<Func<TEntity, int?>> select);
    }
}
