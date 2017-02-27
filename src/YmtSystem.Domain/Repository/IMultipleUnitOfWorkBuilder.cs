namespace YmtSystem.Domain.Repository
{
    using System;
    using YmtSystem.Domain.Shard;

    public interface IMultipleUnitOfWorkBuilder
    {
        IMultipleUnitOfWorkBuilder Append(IUnitOfWork unitOfWork);
        void Commit();
    }
}
