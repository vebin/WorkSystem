namespace YmtSystem.Repository.EF.Factory
{
    using System;
    using YmtSystem.Domain.Repository;

    public class RepositoryFactory : IRepositoryFactory
    {
        public RepositoryFactory()
        {

        }
        public TRepository CreateRepository<TEntity, TUnitOfWork, TRepository>(Func<IEFUnitOfWork, TRepository> factory)
            where TRepository : IRepository<TEntity>
            where TEntity : class
            where TUnitOfWork : YmtSystemDbContext
        {
            var unitOfWork = new DbContextFactory().CreateContext(typeof(TUnitOfWork));
            return factory(unitOfWork as IEFUnitOfWork);
        }
        public TRepository CreateRepository<TEntity, TRepository>(IEFUnitOfWork unitOfWork)
            where TRepository : IRepository<TEntity>
            where TEntity : class
        {
            TRepository repository = (TRepository)Activator.CreateInstance(typeof(TRepository), unitOfWork);
            return repository;
        }
    }
}
