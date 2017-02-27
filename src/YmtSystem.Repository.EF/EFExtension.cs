namespace YmtSystem.Repository.EF
{
    using System;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using YmtSystem.Domain.Shard;
    using YmtSystem.CrossCutting;
    using System.Data.Entity.Validation;
    using System.Collections.Generic;
    using System.Text;
    using System.Web.Script.Serialization;

    internal static class EFExtension
    {
        internal static string TrySerializeEntity<TEntity>(this TEntity entity)
        {
            try
            {
                return new JavaScriptSerializer().Serialize(entity);
            }
            catch
            {
                return "Serialize entity error";
            }
        }
        internal static IOrderedQueryable<TEntity> Sort<TEntity, TKey>(this  IQueryable<TEntity> query, Expression<Func<TEntity, TKey>> sortEx, SortOrder sortOrder)
        {
            if (sortOrder == SortOrder.Unspecified || sortOrder == SortOrder.Descending)
                return query.OrderByDescending(sortEx);
            else if (sortOrder == SortOrder.Ascending)
                return query.OrderBy(sortEx);
            else
                throw new Exception<EFRepositoryException>("非法的排序标识");
        }
        internal static string ValidationErrorAppendToString(this  ICollection<DbValidationError> v)
        {
            if (v == null) return string.Empty;
            var str = new StringBuilder();
            v.Each(_v =>
            {
                str.AppendFormat("{0}-{1}", _v.PropertyName, _v.ErrorMessage);
            });
            return str.ToString();
        }
        #region Internal Methods
        internal static void SetPartModifyField<TEntity>(this DbContext context, TEntity entity, params string[] field) where TEntity : class
        {
            var entry = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)context).ObjectContext.
               ObjectStateManager.GetObjectStateEntry(entity);
            field.Each(e =>
            entry.SetModifiedProperty(e));
        }
        [Obsolete("使用Sort<TEntity, TKey>")]
        internal static IOrderedQueryable<TAggregateRoot> Sort<TAggregateRoot>(this IQueryable<TAggregateRoot> query
            , Expression<Func<TAggregateRoot, dynamic>> sortPredicate
            , SortOrder order)
            where TAggregateRoot : class, IAggregateRoot
        {
            if (sortPredicate == null) return (IOrderedQueryable<TAggregateRoot>)query;
            switch (order)
            {
                case SortOrder.Ascending:
                    return SortByAsc(query, sortPredicate);
                case SortOrder.Descending:
                    return SortByDescending(query, sortPredicate);
                default:
                    return SortByDescending(query, sortPredicate);
            }
        }

        internal static IOrderedQueryable<TAggregateRoot> SortByAsc<TAggregateRoot>(this IQueryable<TAggregateRoot> query, Expression<Func<TAggregateRoot, dynamic>> sortPredicate)
            where TAggregateRoot : class, IAggregateRoot
        {
            return InvokeSortBy(query, sortPredicate, SortOrder.Ascending);
        }

        internal static IOrderedQueryable<TAggregateRoot> SortByDescending<TAggregateRoot>(this IQueryable<TAggregateRoot> query, Expression<Func<TAggregateRoot, dynamic>> sortPredicate)
            where TAggregateRoot : class, IAggregateRoot
        {
            return InvokeSortBy(query, sortPredicate, SortOrder.Descending);
        }
        #endregion

        #region Private Methods
        [Obsolete("使用 Sort<TEntity, TKey>")]
        private static IOrderedQueryable<TAggregateRoot> InvokeSortBy<TAggregateRoot>(IQueryable<TAggregateRoot> query,
            Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder)
            where TAggregateRoot : class, IAggregateRoot
        {
            var param = sortPredicate.Parameters[0];
            string propertyName = null;
            Type propertyType = null;
            Expression bodyExpression = null;
            if (sortPredicate.Body is UnaryExpression)
            {
                UnaryExpression unaryExpression = sortPredicate.Body as UnaryExpression;
                bodyExpression = unaryExpression.Operand;
            }
            else if (sortPredicate.Body is MemberExpression)
            {
                bodyExpression = sortPredicate.Body;
            }
            else
                throw new ArgumentException(@"The body of the sort predicate expression should be 
                either UnaryExpression or MemberExpression.", "sortPredicate");
            MemberExpression memberExpression = (MemberExpression)bodyExpression;
            propertyName = memberExpression.Member.Name;
            if (memberExpression.Member.MemberType == MemberTypes.Property)
            {
                PropertyInfo propertyInfo = memberExpression.Member as PropertyInfo;
                propertyType = propertyInfo.PropertyType;
            }
            else
                throw new InvalidOperationException(@"Cannot evaluate the type of property since the member expression 
                represented by the sort predicate expression does not contain a PropertyInfo object.");

            Type funcType = typeof(Func<,>).MakeGenericType(typeof(TAggregateRoot), propertyType);
            LambdaExpression convertedExpression = Expression.Lambda(funcType,
                Expression.Convert(Expression.Property(param, propertyName), propertyType), param);

            var sortingMethods = typeof(Queryable).GetMethods(BindingFlags.Public | BindingFlags.Static);
            var sortingMethodName = GetSortingMethodName(sortOrder);
            var sortingMethod = sortingMethods.Where(sm => sm.Name == sortingMethodName &&
                sm.GetParameters() != null &&
                sm.GetParameters().Length == 2).First();
            return (IOrderedQueryable<TAggregateRoot>)sortingMethod
                .MakeGenericMethod(typeof(TAggregateRoot), propertyType)
                .Invoke(null, new object[] { query, convertedExpression });
        }

        private static string GetSortingMethodName(SortOrder sortOrder)
        {
            switch (sortOrder)
            {
                case SortOrder.Ascending:
                    return "OrderBy";
                case SortOrder.Descending:
                    return "OrderByDescending";
                default:
                    throw new ArgumentException("Sort Order must be specified as either Ascending or Descending.",
            "sortOrder");
            }
        }
        #endregion
    }
}
