namespace YmtSystem.Repository.EF.Factory
{
    using System;
    using YmtSystem.Domain.Repository;

    public interface IRepositoryFactory
    {
        TRepository CreateRepository<TEntity, TUnitOfWork, TRepository>(Func<IEFUnitOfWork, TRepository> factory)
            where TRepository : IRepository<TEntity>
            where TEntity : class
            where TUnitOfWork : YmtSystemDbContext;
        TRepository CreateRepository<TEntity, TRepository>(IEFUnitOfWork unitOfWork)
            where TRepository : IRepository<TEntity>
            where TEntity : class;
    }
}
