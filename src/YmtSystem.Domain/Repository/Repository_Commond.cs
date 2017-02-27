namespace YmtSystem.Domain.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using YmtSystem.Domain.Shard;

    public abstract partial class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        public abstract void Remove(params TEntity[] entity);
        public abstract void Update(TEntity entity);
        public abstract void Add(TEntity entity);
        public abstract void Add(IEnumerable<TEntity> entity);
        public abstract int ExecuteCommand(string scriptCommand, params object[] parameters);
        public abstract void AddOrUpdate(Expression<Func<TEntity, object>> identifierExpression, params TEntity[] entities);
    }
}
