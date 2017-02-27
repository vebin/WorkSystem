
namespace YmtSystem.Domain.INHRepository
{
    using System;

    public partial interface INHRepository<TEntity> where TEntity : class
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
