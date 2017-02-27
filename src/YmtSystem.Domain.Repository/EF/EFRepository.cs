namespace YmtSystem.Domain.Repository.EF
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using YmtSystem.CrossCutting;
    using YmtSystem.Domain.Repository;
    using YmtSystem.Domain.Shard;
    using YmtSystem.Domain.Specifications;

    public partial class EFRepository<TAggregateRoot> : Repository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        private IEFRepositoryContext contex;
        public EFRepository(IRepositoryContext contexts)
            : base(contexts)
        {
            if (contexts is IEFRepositoryContext)
                this.contex = contexts as IEFRepositoryContext;
            else
                throw new Exception<EFRepositoryException>("仓储类型错误");
        }

        public IEFRepositoryContext EFContext { get { return this.contex; } }

        public override void Add(TAggregateRoot aggregateRoot)
        {
            this.EFContext.RegisterNew(aggregateRoot);
        }

        public override TAggregateRoot FindOne(Guid key)
        {
            return this.EFContext.Context.Set<TAggregateRoot>().Find(key);
        }

        public override IEnumerable<TAggregateRoot> FindAll()
        {
            return this.FindAll(new AnySpecification<TAggregateRoot>());
        }

        public override IEnumerable<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder)
        {
            return this.FindAll(new AnySpecification<TAggregateRoot>(), sortPredicate, sortOrder);
        }

        public override PagedResult<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize)
        {
            return this.FindAll(new AnySpecification<TAggregateRoot>(), sortPredicate, sortOrder, pageNumber, pageSize);
        }

        public override IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification)
        {
            return this.FindAll(specification, null);
        }

        public override IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder)
        {
            var spec = specification.SatisfiedBy();
            var query = this.EFContext.Context.Set<TAggregateRoot>().Where(spec);
            return query.Sort(sortPredicate, sortOrder);
        }

        public override PagedResult<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize)
        {
            var spec = specification.SatisfiedBy();
            var query = this.EFContext.Context.Set<TAggregateRoot>().Where(spec);
            var result = query
                            .Sort(sortPredicate, sortOrder)
                            .Skip(pageNumber)
                            .Take(pageSize)
                            .GroupBy(p => new { Total = query.Count() })
                            .FirstOrDefault();
            return new PagedResult<TAggregateRoot>(result.Key.Total, (result.Key.Total + pageSize - 1) / pageSize, pageSize, pageNumber, result.Select(e => e));
        }

        public override IEnumerable<TAggregateRoot> FindAll(params Expression<Func<TAggregateRoot, dynamic>>[] includeLoadingProperties)
        {
            return FindAll(new AnySpecification<TAggregateRoot>(), includeLoadingProperties);
        }

        public override IEnumerable<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, dynamic>> sortPredicate
            , System.Data.SqlClient.SortOrder sortOrder
            , params System.Linq.Expressions.Expression<Func<TAggregateRoot, dynamic>>[] includeLoadingProperties)
        {
            return FindAll(new AnySpecification<TAggregateRoot>(), sortPredicate, sortOrder, includeLoadingProperties);
        }

        public override PagedResult<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, dynamic>> sortPredicate
            , SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<TAggregateRoot, dynamic>>[] includeLoadingProperties)
        {
            return FindAll(new AnySpecification<TAggregateRoot>(), sortPredicate, sortOrder, pageNumber, pageSize, includeLoadingProperties);
        }

        public override IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification
            , params Expression<Func<TAggregateRoot, dynamic>>[] includeLoadingProperties)
        {
            return FindAll(specification, null, System.Data.SqlClient.SortOrder.Unspecified, includeLoadingProperties);
        }

        public override IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification
            , Expression<Func<TAggregateRoot, dynamic>> sortPredicate
            , SortOrder sortOrder
            , params Expression<Func<TAggregateRoot, dynamic>>[] includeLoadingProperties)
        {
            this.EFContext.Context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            var dbSet = this.EFContext.Context.Set<TAggregateRoot>();
            var query = CreateQueryable(specification, includeLoadingProperties, dbSet);
            return query.Sort(sortPredicate, sortOrder);
        }

        public override YmtSystem.CrossCutting.PagedResult<TAggregateRoot> FindAll(
            ISpecification<TAggregateRoot> specification
            , Expression<Func<TAggregateRoot, dynamic>> sortPredicate
            , SortOrder sortOrder
            , int pageNumber
            , int pageSize
            , params System.Linq.Expressions.Expression<Func<TAggregateRoot, dynamic>>[] includeLoadingProperties)
        {
            var transaction = this.EFContext.Context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            var dbSet = this.EFContext.Context.Set<TAggregateRoot>();
            var query = CreateQueryable(specification, includeLoadingProperties, dbSet);
            var group = query.Sort(sortPredicate, sortOrder).Skip(pageNumber).Take(pageSize).GroupBy(e => new { Total = query.Count() }).FirstOrDefault();
            transaction.Commit();
            return new PagedResult<TAggregateRoot>(group.Key.Total, (group.Key.Total - 1 + pageSize) / pageSize, pageSize, pageNumber, group.Select(e => e));
        }

        public override TAggregateRoot Find(ISpecification<TAggregateRoot> specification)
        {
            return this.EFContext.Context.Set<TAggregateRoot>().Where(specification.SatisfiedBy()).FirstOrDefault();
        }

        public override TAggregateRoot Find(ISpecification<TAggregateRoot> specification
            , params Expression<Func<TAggregateRoot, dynamic>>[] includeLoadingProperties)
        {
            var query = this.EFContext.Context.Set<TAggregateRoot>();
            if (includeLoadingProperties == null || !includeLoadingProperties.Any()) return query.Where(specification.SatisfiedBy()).FirstOrDefault();
            var subQuery = CreateQueryable(specification, includeLoadingProperties, query);
            return subQuery.Where(specification.SatisfiedBy()).FirstOrDefault();
        }

        public override bool Exists(ISpecification<TAggregateRoot> specification)
        {
            return this.EFContext.Context.Set<TAggregateRoot>().Where(specification.SatisfiedBy()).Any();
        }

        public override void Remove(params TAggregateRoot[] aggregateRoot)
        {
            this.EFContext.RegisterDeleted(aggregateRoot);
        }

        public override void Update(TAggregateRoot aggregateRoot)
        {
            this.EFContext.RegisterModified(aggregateRoot);
        }

        public override int ExecuteCommand(string scriptCommand, params object[] parameters)
        {
            return this.EFContext.Context.Database.ExecuteSqlCommand(scriptCommand, parameters);
        }

        public override IEnumerable<TAggregateRoot> ExecuteQuery(string scriptQuery, params object[] parameters)
        {
            return this.EFContext.Context.Set<TAggregateRoot>().SqlQuery(scriptQuery, parameters).AsEnumerable();
        }

        private string GetEagerLoadingPath(Expression<Func<TAggregateRoot, dynamic>> eagerLoadingProperty)
        {
            MemberExpression memberExpression = this.GetMemberInfo(eagerLoadingProperty);
            var parameterName = eagerLoadingProperty.Parameters.First().Name;
            var memberExpressionStr = memberExpression.ToString();
            var path = memberExpressionStr.Replace(parameterName + ".", "");
            return path;
        }

        private MemberExpression GetMemberInfo(LambdaExpression lambda)
        {
            if (lambda == null)
                throw new ArgumentNullException("method");

            MemberExpression memberExpr = null;

            if (lambda.Body.NodeType == ExpressionType.Convert)
            {
                memberExpr =
                    ((UnaryExpression)lambda.Body).Operand as MemberExpression;
            }
            else if (lambda.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpr = lambda.Body as MemberExpression;
            }

            if (memberExpr == null)
                throw new ArgumentException("method");

            return memberExpr;
        }

        private IQueryable<TAggregateRoot> CreateQueryable(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>>[] includeLoadingProperties, DbSet<TAggregateRoot> dbSet)
        {
            IQueryable<TAggregateRoot> query = null;
            if (includeLoadingProperties != null && includeLoadingProperties.Any())
            {
                var eagerLoadingVal = includeLoadingProperties[0];
                var eagerLoadingPath = GetEagerLoadingPath(eagerLoadingVal);
                var subQuery = dbSet.Include(eagerLoadingPath);
                includeLoadingProperties.Each(e =>
                {
                    eagerLoadingVal = e;
                    eagerLoadingPath = GetEagerLoadingPath(e);
                    subQuery = subQuery.Include(eagerLoadingPath);
                });
                query = subQuery.Where(specification.SatisfiedBy());
            }
            else
            {
                query = dbSet.Where(specification.SatisfiedBy());
            }
            return query;
        }
    }
}
