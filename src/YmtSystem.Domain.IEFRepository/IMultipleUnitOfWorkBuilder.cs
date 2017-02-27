namespace YmtSystem.Domain.IEFRepository
{
    using System;
    using YmtSystem.Domain.Shard;

    public interface IMultipleUnitOfWorkBuilder
    {
        IMultipleUnitOfWorkBuilder Append(IUnitOfWork unitOfWork);
        void Commit();
    }
}
