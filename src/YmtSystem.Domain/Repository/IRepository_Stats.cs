

namespace YmtSystem.Domain.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using YmtSystem.Domain.Shard;
    using YmtSystem.Domain.Specifications;

    public partial interface IRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// 按指定的规格统计
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        int Count(Expression<Func<TEntity, bool>> specification);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> specification);
        decimal? SumSync(Expression<Func<TEntity, bool>> specification, Expression<Func<TEntity, decimal?>> select);
        Task<decimal?> SumAsync(Expression<Func<TEntity, bool>> specification, Expression<Func<TEntity, decimal?>> select);
        Task<int?> SumAsync(Expression<Func<TEntity, bool>> specification, Expression<Func<TEntity, int?>> select);
        IQueryable<IGrouping<TKey, TEntity>> Group<TKey>(Expression<Func<TEntity, bool>> specification, Expression<Func<TEntity, TKey>> keySelect);
    }
}
