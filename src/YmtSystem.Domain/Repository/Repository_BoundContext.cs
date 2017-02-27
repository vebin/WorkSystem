namespace YmtSystem.Domain.Repository
{
    using System;
    using YmtSystem.Domain.Shard;

    public abstract partial class Repository<TEntity> : IRepository<TEntity>
       where TEntity : class
    {
        private readonly IUnitOfWork unitofwork;

        public Repository(IUnitOfWork unitofwork)
        {
            this.unitofwork = unitofwork;
        }
        public IUnitOfWork Context
        {
            get { return this.unitofwork; }
        }
    }
}
