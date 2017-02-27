namespace YmtSystem.Domain.IEFRepository
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions; 
    using System.Threading.Tasks;
    using YmtSystem.Domain.Shard;
   
    public partial interface IRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// 根据实体的ID值，从仓储中读取实体。
        /// </summary>
        /// <param name="key">实体的ID值。</param>
        /// <returns>实体实例。</returns>
        Task<TEntity> AsyncFindOne(TimeSpan timeOut, params object[] key);       
        /// <summary>
        /// 以指定的排序字段和排序方式，从仓储中查找所有实体。
        /// </summary>
        /// <param name="sortPredicate">用于表述排序字段的Lambda表达式。</param>
        /// <param name="sortOrder">排序方式。</param>
        /// <returns>排序后的所有实体。</returns>
        Task<IEnumerable<TEntity>> AsyncFindAll<TSortKey>(Expression<Func<TEntity, TSortKey>> sortPredicate, SortOrder sortOrder);
    }
}
