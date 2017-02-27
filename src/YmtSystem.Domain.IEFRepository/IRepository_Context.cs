namespace YmtSystem.Domain.IEFRepository
{
    using System;
    using YmtSystem.Domain.Shard;
  
    public partial interface IRepository<TEntity> : IDisposable
        where TEntity : class
    {
        /// <summary>
        /// Context UnitOfWork
        /// </summary>
        [Obsolete("use UnitOfWork")]
        IUnitOfWork Context { get; }
        /// <summary>
        /// UnitOfWork
        /// </summary>
        IUnitOfWork UnitOfWork { get; }
        /// <summary>
        /// 设置连接数据库
        /// </summary>
        /// <param name="connectionstring"></param>
        //[Obsolete("不要使用", true)]
        //void SetConnectionDb(string connectionstring);
    }
}
