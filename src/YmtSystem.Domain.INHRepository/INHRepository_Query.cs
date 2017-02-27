namespace YmtSystem.Domain.INHRepository
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

    public partial interface INHRepository<TEntity> where TEntity : class
    {
        //ICriteria CreateEntityCriteria();
        IQuery CreateEntityQuery(string queryString);
        ISQLQuery CreateSQLQuerys(string sqlCommand);

        IQueryOver<TEntity> FindQueryOver(Expression<Func<TEntity, bool>> specification, Expression<Func<TEntity, object>> orderBySpecification, SortOrder order, int pageIndex, int pageSize);
        PagedResult<TEntity> FindPagedResult(Expression<Func<TEntity, bool>> specification, Expression<Func<TEntity, object>> orderBySpecification, SortOrder order, int pageIndex, int pageSize);

        TEntity FindOne(object id);
        TEntity FindOne(object id, LockMode lockMode);
        TEntity FindOne(Expression<Func<TEntity, bool>> specification, object obj, LockMode lockMode);

        TEntity Find(string sqlCommand);
        TEntity FindSingleOrDefault(Expression<Func<TEntity, bool>> specification);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> specification);
        IQueryOver<TEntity> FindAll();
    }
}
