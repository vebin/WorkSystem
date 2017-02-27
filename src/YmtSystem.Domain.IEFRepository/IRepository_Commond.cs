namespace YmtSystem.Domain.IEFRepository
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using YmtSystem.Domain.Shard;

    public partial interface IRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// 将指定的实体添加到仓储中。
        /// </summary>
        /// <param name="entity">需要添加到仓储的实体实例。</param>
        void Add(TEntity entity);
        /// <summary>
        ///  批量将指定的实体添加到仓储中。
        ///  <remarks>bulk false，则使用普通批量增加方式；bulk true 则使用 bulk insert 方式</remarks>
        /// </summary>
        /// <param name="entity">>需要添加到仓储的实体实例。</param>
        /// <param name="bulk">大批量添加，则使用bulk insert 写入数据</param>
        void Add(IEnumerable<TEntity> entity, bool bulk = false);
        /// <summary>                            
        /// 将指定的实体从仓储中移除。
        /// </summary>
        /// <param name="entity">需要从仓储中移除的实体。</param>
        void Remove(params TEntity[] entity);
        /// <summary>
        /// 更新指定的实体。
        /// </summary>
        /// <param name="entity">需要更新的实体。</param>
        void Update(TEntity entity);
        /// <summary>
        /// 增加或更新指定的实体
        /// </summary>
        /// <param name="identifierExpression"></param>
        /// <param name="entities"></param>
        void AddOrUpdate(Expression<Func<TEntity, object>> identifierExpression, params TEntity[] entities);
        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="scriptCommand"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        int ExecuteCommand(string scriptCommand, params object[] parameters);
        /// <summary>
        ///  实体当前值覆盖原始值
        /// </summary>
        /// <param name="original"></param>
        /// <param name="current"></param>
        void ApplyCurrentValues(TEntity original, TEntity current);
    }
}
