namespace YmtSystem.Repository.NH
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using NHibernate;
    using YmtSystem.CrossCutting;
    using YmtSystem.Domain.INHRepository;

    public partial class NHRepository<TEntity> : INHRepository<TEntity> where TEntity : class
    {
        protected virtual ICriteria CreateEntityCriteria()
        {
            return this._UnitOfWork.CurrentSession.Current.CreateCriteria<TEntity>();
        }

        protected virtual IQueryOver<TEntity> CreateQueryOver()
        {
            return this._UnitOfWork.CurrentSession.Current.QueryOver<TEntity>();
        }
       
        public virtual IQuery CreateEntityQuery(string queryString)
        {
            return this._UnitOfWork.CurrentSession.Current.CreateQuery(queryString);
        }

        public virtual IQueryOver<TEntity> FindQueryOver(Expression<Func<TEntity, bool>> specification, Expression<Func<TEntity, object>> orderBySpecification, SortOrder order, int pageIndex, int pageSize)
        {
            if (order == SortOrder.Descending)
                return this._UnitOfWork.CurrentSession.Current.QueryOver<TEntity>().Where(specification).OrderBy(orderBySpecification).Desc.Skip(pageIndex).Take(pageIndex * pageSize);
            else
                return this._UnitOfWork.CurrentSession.Current.QueryOver<TEntity>().Where(specification).OrderBy(orderBySpecification).Asc.Skip(pageIndex).Take(pageIndex * pageSize);
        }

        public virtual PagedResult<TEntity> FindPagedResult(Expression<Func<TEntity, bool>> specification, Expression<Func<TEntity, object>> orderBySpecification, SortOrder order, int pageIndex, int pageSize)
        {
            var query = this._UnitOfWork.CurrentSession.Current.QueryOver<TEntity>().Where(specification);
            var count = query.RowCount();
            var pageCount = (count / pageSize) + (count % pageSize > 0 ? 1 : count % pageSize);
            var orderByQuery = query.ThenBy(orderBySpecification);
            if (order == SortOrder.Descending)
                query = orderByQuery.Desc;
            else
                query = orderByQuery.Asc;

            var queryResult = query.Skip(pageIndex).Take(pageSize);

            return new PagedResult<TEntity>(count, pageCount, pageSize, pageIndex, queryResult.List());
        }

        public virtual TEntity FindOne(object id)
        {
            return this._UnitOfWork.CurrentSession.Current.Get<TEntity>(id, LockMode.None);
        }

        public virtual TEntity FindOne(object id, LockMode lockMode)
        {
            return this._UnitOfWork.CurrentSession.Current.Get<TEntity>(id, lockMode);
        }

        public virtual TEntity FindOne(Expression<Func<TEntity, bool>> specification, object obj, LockMode lockMode)
        {
            this._UnitOfWork.CurrentSession.Current.Lock(obj, lockMode);
            return this._UnitOfWork.CurrentSession.Current.QueryOver<TEntity>().Where(specification).SingleOrDefault();
        }

        public virtual TEntity Find(string sqlCommand)
        {
            return this._UnitOfWork.CurrentSession.Current.CreateSQLQuery(sqlCommand).AddEntity(typeof(TEntity)).UniqueResult<TEntity>();
        }

        public virtual TEntity FindSingleOrDefault(Expression<Func<TEntity, bool>> specification)
        {
            return this._UnitOfWork.CurrentSession.Current.QueryOver<TEntity>().Where(specification).SingleOrDefault();
        }

        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> specification)
        {
            return this._UnitOfWork.CurrentSession.Current.QueryOver<TEntity>().Where(specification).Future();
        }

        public virtual IQueryOver<TEntity> FindAll()
        {
            return this._UnitOfWork.CurrentSession.Current.QueryOver<TEntity>();
        }

        public virtual ISQLQuery CreateSQLQuerys(string sqlCommand)
        {
            return this._UnitOfWork.CurrentSession.Current.CreateSQLQuery(sqlCommand); ;
        }
    }
}
