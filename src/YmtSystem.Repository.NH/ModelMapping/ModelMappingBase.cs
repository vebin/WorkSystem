namespace YmtSystem.Repository.NH.ModelMapping
{
    using System;
    using FluentNHibernate.Mapping;
    /// <summary>
    ///  标记为模型映射基类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class ModelMappingBase<TEntity> : ClassMap<TEntity>
    {
         
    }
}
