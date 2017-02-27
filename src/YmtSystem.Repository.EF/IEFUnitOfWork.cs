namespace YmtSystem.Repository.EF
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using YmtSystem.Domain.Repository;
   
    /// <summary>
    /// entity framework 工作单元模式实现
    /// </summary>
    public interface IEFUnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Context
        /// </summary>
        DbContext Context { get; }
        /// <summary>
        ///  CreateSet
        /// </summary>
        /// <typeparam name="TEntity">TEntity</typeparam>
        /// <returns></returns>
        DbSet<TEntity> CreateSet<TEntity>() where TEntity : class;
        /// <summary>
        /// 实体标记为新增状态
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="obj"></param>
        void RegisterNew<TEntity>(TEntity obj) where TEntity : class;
        /// <summary>
        /// 批量实体编辑为新增状态
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="obj"></param>
        void RegisterNew<TEntity>(IEnumerable<TEntity> obj) where TEntity : class;
        /// <summary>
        /// 大批量实体插入数据库
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="obj"></param>
        void RegisterNewBulk<TEntity>(IEnumerable<TEntity> obj) where TEntity : class;
        /// <summary>
        /// 实体标记为修改状态
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="obj"></param>
        void RegisterModified<TEntity>(TEntity obj) where TEntity : class;
        /// <summary>
        /// 实体标记为删除状态
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="obj"></param>
        void RegisterDeleted<TEntity>(params TEntity[] obj) where TEntity : class;
    }
}
