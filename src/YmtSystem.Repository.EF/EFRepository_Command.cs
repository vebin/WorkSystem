namespace YmtSystem.Repository.EF
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Data.Entity.Migrations;
    using YmtSystem.Domain.Repository;
    using YmtSystem.Domain.Shard;

    partial class EFRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        public virtual void Add(TEntity entity)
        {
            this.EfUnitOfWork.RegisterNew(entity);
        }
        public virtual void Add(IEnumerable<TEntity> entity, bool bulk = false)
        {
            if (!bulk)
                this.EfUnitOfWork.RegisterNew(entity);
            else
                this.EfUnitOfWork.RegisterNewBulk(entity);
        }
        public virtual void Remove(params TEntity[] entity)
        {
            this.EfUnitOfWork.RegisterDeleted(entity);
        }
        public virtual void Update(TEntity entity)
        {
            this.EfUnitOfWork.RegisterModified(entity);
        }
        public virtual int ExecuteCommand(string scriptCommand, params object[] parameters)
        {
            return this.EfUnitOfWork.Context.Database.ExecuteSqlCommand(scriptCommand, parameters);
        }
        public virtual void AddOrUpdate(Expression<Func<TEntity, object>> identifierExpression, params TEntity[] entities)
        {
            this.EfUnitOfWork.CreateSet<TEntity>().AddOrUpdate(identifierExpression, entities);
        }
        public virtual void ApplyCurrentValues(TEntity original, TEntity current) 
        {
            this.EfUnitOfWork.Context.Entry(original).CurrentValues.SetValues(current);
        }
    }
}
