namespace YmtSystem.Repository.NH
{
    using System;
    using NHibernate;
    using YmtSystem.Domain.INHRepository;
    using YmtSystem.Repository.NH.Context;

    public interface INHUnitOfWork : IUnitOfWork
    {
        DbSessionContext CurrentSession { get; }
    }
}
