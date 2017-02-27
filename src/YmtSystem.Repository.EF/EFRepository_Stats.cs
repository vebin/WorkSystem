

namespace YmtSystem.Repository.EF
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using System.Data.Entity;
    using YmtSystem.Domain.Repository;
    using YmtSystem.Domain.Shard;
    using YmtSystem.Domain.Specifications;

    /// <summary>
    /// 仓储（统计）
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public partial class EFRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        public virtual IQueryable<IGrouping<TKey, TEntity>> Group<TKey>(Expression<Func<TEntity, bool>> specification, Expression<Func<TEntity, TKey>> keySelect)
        {
            return this
                         .EfUnitOfWork
                         .CreateSet<TEntity>()
                         .Where(specification)
                         .GroupBy(keySelect);
        }
        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> specification)
        {            
            return await this
                         .EfUnitOfWork
                         .CreateSet<TEntity>()
                         .Where(specification)
                         .CountAsync();
        }
        public virtual int Count(Expression<Func<TEntity, bool>> specification)
        {
            return this
                        .EfUnitOfWork
                        .CreateSet<TEntity>()
                        .Where(specification)
                        .Count();
        }
        public virtual Task<decimal?> SumAsync(Expression<Func<TEntity, bool>> specification, Expression<Func<TEntity, decimal?>> select)
        {
            return this
                        .EfUnitOfWork
                        .CreateSet<TEntity>()
                        .Where(specification)
                        .SumAsync(select);
        }

        public virtual decimal? SumSync(Expression<Func<TEntity, bool>> specification, Expression<Func<TEntity, decimal?>> select)
        {
            return this
                      .EfUnitOfWork
                      .CreateSet<TEntity>()
                      .Where(specification)
                      .Sum(select);
        }
        public virtual Task<int?> SumAsync(Expression<Func<TEntity, bool>> specification, Expression<Func<TEntity, int?>> select) 
        {
            return this
                        .EfUnitOfWork
                        .CreateSet<TEntity>()
                        .Where(specification)
                        .SumAsync(select);
        }
    }
}
