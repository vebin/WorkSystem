namespace YmtSystem.Domain.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Linq.Expressions;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using YmtSystem.Domain.Shard;
    using YmtSystem.CrossCutting;
    using YmtSystem.Domain.Specifications;

    /// <summary>
    /// 仓储抽象基类
    /// </summary>
    /// <typeparam name="TEntity">TEntity 领域层聚合跟</typeparam>
    public abstract partial class Repository<TEntity> : IRepository<TEntity>
       where TEntity : class
    {
        public abstract IQueryable<TEntity> FindAll();
        public abstract IQueryable<TEntity> FindTop(int top);
        public abstract IQueryable<TEntity> FindTop(ISpecification<TEntity> specification, int top);
        public abstract IQueryable<TEntity> FindAll(ISpecification<TEntity> specification);
        public abstract IQueryable<TEntity> FindAll(ISpecification<TEntity> specification, params Expression<Func<TEntity, dynamic>>[] includeLoadingProperties);
        public abstract IQueryable<TEntity> FindTop<TSortKey>(ISpecification<TEntity> specification, int top, Expression<Func<TEntity, TSortKey>> sortPredicate, SortOrder sortOrder);
        public abstract IQueryable<TEntity> FindAll<TSortKey>(Expression<Func<TEntity, TSortKey>> sortPredicate, SortOrder sortOrder);
        public abstract IQueryable<TEntity> FindAll<TSortKey>(Expression<Func<TEntity, TSortKey>> sortPredicate, SortOrder sortOrder, params Expression<Func<TEntity, dynamic>>[] includeLoadingProperties);      
        public abstract IQueryable<TEntity> FindAll<TSortKey>(ISpecification<TEntity> specification, Expression<Func<TEntity, TSortKey>> sortPredicate, SortOrder sortOrder);
        public abstract IQueryable<TEntity> FindAll<TSortKey>(ISpecification<TEntity> specification, Expression<Func<TEntity, TSortKey>> sortPredicate, SortOrder sortOrder, params Expression<Func<TEntity, dynamic>>[] includeLoadingProperties);
        
        public abstract PagedResult<TEntity> FindAll<TSortKey>(ISpecification<TEntity> specification, Expression<Func<TEntity, TSortKey>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize);
        public abstract PagedResult<TEntity> FindAll<TSortKey>(ISpecification<TEntity> specification, Expression<Func<TEntity, TSortKey>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<TEntity, dynamic>>[] includeLoadingProperties);
        public abstract PagedResult<TEntity> FindAll<TSortKey>(Expression<Func<TEntity, TSortKey>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize);
        public abstract PagedResult<TEntity> FindAll<TSortKey>(Expression<Func<TEntity, TSortKey>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<TEntity, dynamic>>[] includeLoadingProperties);

        public abstract TEntity FindOne(ISpecification<TEntity> specification);
        public abstract TEntity FindOne(ISpecification<TEntity> specification, params Expression<Func<TEntity, dynamic>>[] includeLoadingProperties);
        public abstract TEntity FindOne<TPrimaryKey>(params TPrimaryKey[] key);

        public abstract bool Exists(ISpecification<TEntity> specification);

        public abstract IEnumerable<TEntity> ExecuteQuery(string scriptQuery, params object[] parameters);
    }
}
