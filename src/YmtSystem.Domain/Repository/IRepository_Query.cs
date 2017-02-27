namespace YmtSystem.Domain.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using YmtSystem.Domain.Shard;
    using YmtSystem.Domain.Specifications;
    using YmtSystem.CrossCutting;

    /// <summary>
    /// 领域存储通用接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public partial interface IRepository<TEntity>
        where TEntity : class
    {
        IQueryable<TEntity> FindAll();

        IQueryable<TEntity> FindTop(int top);
        IQueryable<TEntity> FindTop(Expression<Func<TEntity, bool>> specification, int top);
        IQueryable<TEntity> FindTop<TSortKey>(Expression<Func<TEntity, bool>> specification, int top, Expression<Func<TEntity, TSortKey>> sortPredicate, SortOrder sortOrder);

        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> specification);
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> specification, params Expression<Func<TEntity, dynamic>>[] includeLoadingProperties);

        IQueryable<TEntity> Find<TSortKey>(Expression<Func<TEntity, TSortKey>> sortPredicate, SortOrder sortOrder);
        IQueryable<TEntity> Find<TSortKey>(Expression<Func<TEntity, TSortKey>> sortPredicate, SortOrder sortOrder, params Expression<Func<TEntity, dynamic>>[] includeLoadingProperties);
        IQueryable<TEntity> Find<TSortKey>(Expression<Func<TEntity, bool>> specification, Expression<Func<TEntity, TSortKey>> sortPredicate, SortOrder sortOrder);
        IQueryable<TEntity> Find<TSortKey>(Expression<Func<TEntity, bool>> specification, Expression<Func<TEntity, TSortKey>> sortPredicate, SortOrder sortOrder, params Expression<Func<TEntity, dynamic>>[] includeLoadingProperties);

        PagedResult<TEntity> Find<TSortKey>(Expression<Func<TEntity, TSortKey>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize);
        PagedResult<TEntity> Find<TSortKey>(Expression<Func<TEntity, TSortKey>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<TEntity, dynamic>>[] includeLoadingProperties);
        PagedResult<TEntity> Find<TSortKey>(Expression<Func<TEntity, bool>> specification, Expression<Func<TEntity, TSortKey>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize);
        PagedResult<TEntity> Find<TSortKey>(Expression<Func<TEntity, bool>> specification, Expression<Func<TEntity, TSortKey>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<TEntity, dynamic>>[] includeLoadingProperties);

        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> specification, bool readUnCommit = false);
        TEntity FindOne(Expression<Func<TEntity, bool>> specification, bool readUnCommit = false);
        TEntity FindOne(Expression<Func<TEntity, bool>> specification, bool readUnCommit = false, params Expression<Func<TEntity, dynamic>>[] includeLoadingProperties);
        TEntity FindOne<TPrimaryKey>(TPrimaryKey key, bool readUnCommit = false);

        bool Exists(Expression<Func<TEntity, bool>> specification);

        IEnumerable<TEntity> ExecuteQuery(string scriptQuery, params object[] parameters);
    }
}
