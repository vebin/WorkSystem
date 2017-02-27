namespace YmtSystem.Domain.Repository.EF
{
    using System.Data.Entity;
    using YmtSystem.Domain.Repository;

    /// <summary>
    /// Entity framework 仓储上下文
    /// </summary>
    public interface IEFRepositoryContext : IRepositoryContext
    {
        /// <summary>
        /// DbContext
        /// </summary>
        DbContext Context { get; }
    }
}
