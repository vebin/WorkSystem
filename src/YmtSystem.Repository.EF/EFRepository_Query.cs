namespace YmtSystem.Repository.EF
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using YmtSystem.CrossCutting;
    using YmtSystem.Domain.Repository;
    using YmtSystem.Domain.Shard;
    using YmtSystem.Domain.Specifications;
    /// <summary>
    /// 仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public partial class EFRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        public virtual ObjectQuery EntityQuery<TObject>(string queryString, params ObjectParameter[] parameters)
        {
            return ((this.EfUnitOfWork.Context as IObjectContextAdapter).ObjectContext).CreateQuery<TObject>(queryString, parameters);
        }

        public virtual TEntity FindOne<TPrimaryKey>(TPrimaryKey key, bool readUnCommit = false)
        {
            if (!readUnCommit)
                return this.EfUnitOfWork.CreateSet<TEntity>().Find(key);
            else
                return ReadUnCommit(() => this.EfUnitOfWork.CreateSet<TEntity>().Find(key));
        }

        public virtual TEntity FindOne(Expression<Func<TEntity, bool>> specification, bool readUnCommit = false)
        {
            if (!readUnCommit)
                return this.EfUnitOfWork.CreateSet<TEntity>()/*.Where(specification)*/.SingleOrDefault(specification);/*.FirstOrDefault();*/
            else
                return ReadUnCommit(() => this.EfUnitOfWork.CreateSet<TEntity>()/*.Where(specification)*/.SingleOrDefault(specification));/*.FirstOrDefault());*/
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> specification, bool readUnCommit = false)
        {
            if (!readUnCommit)
                return this.EfUnitOfWork.CreateSet<TEntity>()/*.Where(specification)*/.FirstOrDefault(specification);/*.FirstOrDefault();*/
            else
                return ReadUnCommit(() => this.EfUnitOfWork.CreateSet<TEntity>()/*.Where(specification)*/.FirstOrDefault(specification));/*.FirstOrDefault());*/
        }

        public virtual TEntity FindOne(Expression<Func<TEntity, bool>> specification
            , bool readUnCommit = false, params Expression<Func<TEntity, dynamic>>[] includeLoadingProperties)
        {
            Func<TEntity> read = () =>
            {
                var query = this.EfUnitOfWork.CreateSet<TEntity>();
                if (includeLoadingProperties == null || !includeLoadingProperties.Any()) return query.Where(specification).SingleOrDefault();/*.FirstOrDefault();*/
                var subQuery = CreateQueryable(specification, includeLoadingProperties, query);
                return subQuery/*.Where(specification)*/.SingleOrDefault(specification);/*.FirstOrDefault();*/
            };
            if (!readUnCommit) return read();
            else return ReadUnCommit(read);
        }

        public virtual IQueryable<TEntity> FindAll()
        {
            return this
                          .EfUnitOfWork
                          .CreateSet<TEntity>()
                          .Where(Specification<TEntity>.CreateAnySpecification().SatisfiedBy());
        }

        public virtual IQueryable<TEntity> FindTop(int top)
        {
            return this.FindTop(Specification<TEntity>.CreateAnySpecification().SatisfiedBy(), top);
        }

        public virtual IQueryable<TEntity> FindTop(Expression<Func<TEntity, bool>> specification, int top)
        {
            return this
                          .EfUnitOfWork
                          .CreateSet<TEntity>()
                          .Where(specification)
                          .Take(top);
        }

        public virtual IQueryable<TEntity> FindTop<TSortKey>(Expression<Func<TEntity, bool>> specification, int top, Expression<Func<TEntity, TSortKey>> sortPredicate, SortOrder sortOrder)
        {
            return this
                         .EfUnitOfWork
                         .CreateSet<TEntity>()
                         .Where(specification)
                         .Sort(sortPredicate, sortOrder)
                         .Take(top);
        }

        public virtual PagedResult<TEntity> Find<TSortKey>(
            Expression<Func<TEntity, bool>> specification
            , Expression<Func<TEntity, TSortKey>> sortPredicate
            , SortOrder sortOrder
            , int pageNumber
            , int pageSize)
        {
            var query = this
                        .EfUnitOfWork
                        .CreateSet<TEntity>()
                        .Where(specification);
            var result =
                        query
                        .Sort(sortPredicate, sortOrder)
                        .Skip(pageSize * pageNumber)
                        .Take(pageSize)
                        .GroupBy(t => new { Total = query.Count() })
                        .FirstOrDefault();


            if (result == null)
                return new PagedResult<TEntity>(0, 0, pageSize, pageNumber, Enumerable.Empty<TEntity>());

            return new PagedResult<TEntity>(result.Key.Total, (result.Key.Total + pageSize - 1) / pageSize, pageSize, pageNumber, result.Select(e => e));
        }

        public virtual IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> specification
            , params Expression<Func<TEntity, dynamic>>[] includeLoadingProperties)
        {
            var query = this.EfUnitOfWork.CreateSet<TEntity>();
            if (includeLoadingProperties == null || !includeLoadingProperties.Any())
                return query.Where(specification);
            var subQuery = CreateQueryable(specification, includeLoadingProperties, query);
            return subQuery.Where(specification);
        }

        public virtual IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> specification)
        {
            return this.EfUnitOfWork.CreateSet<TEntity>().Where(specification);
        }

        public virtual IQueryable<TEntity> Find<TSortKey>(Expression<Func<TEntity, TSortKey>> sortPredicate
            , System.Data.SqlClient.SortOrder sortOrder
            , params Expression<Func<TEntity, dynamic>>[] includeLoadingProperties)
        {
            return Find(new AnySpecification<TEntity>().SatisfiedBy(), sortPredicate, sortOrder, includeLoadingProperties);
        }

        public virtual PagedResult<TEntity> Find<TSortKey>(Expression<Func<TEntity, TSortKey>> sortPredicate
            , SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<TEntity, dynamic>>[] includeLoadingProperties)
        {
            return Find(new AnySpecification<TEntity>().SatisfiedBy(), sortPredicate, sortOrder, pageNumber, pageSize, includeLoadingProperties);
        }

        public virtual IQueryable<TEntity> Find<TSortKey>(Expression<Func<TEntity, bool>> specification
            , Expression<Func<TEntity, TSortKey>> sortPredicate
            , SortOrder sortOrder
            , params Expression<Func<TEntity, dynamic>>[] includeLoadingProperties)
        {
            var dbSet = this.EfUnitOfWork.CreateSet<TEntity>();
            var query = CreateQueryable(specification, includeLoadingProperties, dbSet);
            return query.Sort(sortPredicate, sortOrder);
        }

        public virtual PagedResult<TEntity> Find<TSortKey>(
            Expression<Func<TEntity, bool>> specification
            , Expression<Func<TEntity, TSortKey>> sortPredicate
            , SortOrder sortOrder
            , int pageNumber
            , int pageSize
            , params Expression<Func<TEntity, dynamic>>[] includeLoadingProperties)
        {
            var dbSet = this.EfUnitOfWork.CreateSet<TEntity>();
            var query = CreateQueryable(specification, includeLoadingProperties, dbSet);
            var group = query.Sort(sortPredicate, sortOrder).Skip(pageNumber).Take(pageSize).GroupBy(e => new { Total = query.Count() }).FirstOrDefault();
            if (group == null)
                return new PagedResult<TEntity>(0, 0, pageSize, pageNumber, Enumerable.Empty<TEntity>());
            return new PagedResult<TEntity>(group.Key.Total, (group.Key.Total - 1 + pageSize) / pageSize, pageSize, pageNumber, group.Select(e => e));
        }

        public virtual bool Exists(Expression<Func<TEntity, bool>> specification)
        {
            return this.EfUnitOfWork.CreateSet<TEntity>().Where(specification).Any();
        }

        public virtual IEnumerable<TEntity> ExecuteQuery(string scriptQuery, params object[] parameters)
        {
            return this.EfUnitOfWork.Context.Database.SqlQuery<TEntity>(scriptQuery, parameters).AsEnumerable();
        }

        public virtual IQueryable<TEntity> Find<TSortKey>(
           Expression<Func<TEntity, TSortKey>> sortPredicate
           , SortOrder sortOrder)
        {
            return this.Find(AndSpecification<TEntity>.CreateAnySpecification().SatisfiedBy(), sortPredicate, sortOrder);
        }

        public virtual PagedResult<TEntity> Find<TSortKey>(
            Expression<Func<TEntity, TSortKey>> sortPredicate
            , SortOrder sortOrder
            , int pageNumber
            , int pageSize)
        {
            return this.Find(AndSpecification<TEntity>.CreateAnySpecification().SatisfiedBy(), sortPredicate, sortOrder, pageNumber, pageSize, null);
        }

        public virtual IQueryable<TEntity> Find<TSortKey>(
            Expression<Func<TEntity, bool>> specification
            , Expression<Func<TEntity, TSortKey>> sortPredicate
            , SortOrder sortOrder)
        {
            return this.Find(specification, sortPredicate, sortOrder, null);
        }
        private TResult ReadUnCommit<TResult>(Func<TResult> fn, TResult DefResult = default(TResult))
        {
            DbContextTransaction transaction = null;
            try
            {
                transaction = this.EfUnitOfWork.Context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
                var result = fn();
                transaction.Commit();
                return result;
            }
            catch (Exception ex)
            {
                ex.Handler("EfRepository query，");
                transaction.Rollback();
                return DefResult;
            }
        }
        private IQueryable<TEntity> CreateQueryable<TProperty>(
         Expression<Func<TEntity, bool>> specification
          , Expression<Func<TEntity, TProperty>>[] includeLoadingProperties
          , DbSet<TEntity> dbSet)
        {
            IQueryable<TEntity> query = null;
            if (includeLoadingProperties != null && includeLoadingProperties.Any())
            {
                //启用显示加载的导航属性，则关闭延迟加载，此LazyLoadingEnabled默认为true                
                ((IObjectContextAdapter)this.EfUnitOfWork.Context).ObjectContext.ContextOptions.LazyLoadingEnabled = false;
                var subQuery = dbSet.AsQueryable();//.Include(includeLoadingProperties[0]);
                includeLoadingProperties.Each(e =>
                {
                    subQuery = subQuery.Include(e);
                });
                query = subQuery.Where(specification);
            }
            else
            {
                query = dbSet.Where(specification);
            }
            return query;
        }


    }
}
