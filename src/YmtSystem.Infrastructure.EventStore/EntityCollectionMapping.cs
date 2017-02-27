using System;
using System.Linq;
using YmtSystem.CrossCutting.FastReflection;
namespace YmtSystem.Infrastructure.EventStore
{
    internal class DbTb
    {
        public string DbName { get; internal set; }
        public string TbName { get; internal set; }
    }

    //internal class EntityCollectionMapping
    //{
    //    public static DbTb GetCollectionMapping<TEntity>()
    //    {
    //        return GetCollectionMapping(typeof(TEntity));
    //    }
    //    public static DbTb GetCollectionMapping<TEntity>(TEntity entity)
    //    {
    //        return GetCollectionMapping(entity);
    //    }
    //    public static DbTb GetCollectionMapping(object entity)
    //    {
    //        // var mappAttributes = entity.CustomAttributes.OfType<EntityCollectionMappingAttribute>().FirstOrDefault();
    //        var mappAttributes = Attribute.GetCustomAttribute(entity.GetType(), typeof(EntityCollectionMappingAttribute)) as EntityCollectionMappingAttribute;
    //        if (mappAttributes != null)
    //        {
    //            if (mappAttributes.UseStaticMapping)
    //            {
    //                return new DbTb
    //                {
    //                    DbName = mappAttributes.StaticDbName,
    //                    TbName = mappAttributes.StaticTbName
    //                };
    //            }
    //            else
    //            {
    //                return new DbTb
    //                {
    //                    DbName = entity.GetType().GetProperty(mappAttributes.DynamicPropertyDbName).FastGetValue(entity).ToString(),//.GetValue(entity, null).ToString(),
    //                    TbName = entity.GetType().GetProperty(mappAttributes.DynamicPropertyTbName).FastGetValue(entity).ToString()//.GetValue(entity, null).ToString()
    //                };
    //            }
    //        }
    //        else
    //        {
    //            if (typeof(IEntityMappingToCollections).IsAssignableFrom(entity.GetType()))
    //            {
    //                var mongoMapp = entity as IEntityMappingToCollections;
    //                return new DbTb
    //                {
    //                    DbName = mongoMapp.DBName,
    //                    TbName = mongoMapp.TBName
    //                };
    //            }
    //            var tmpEntity = entity.GetType();
    //            if (tmpEntity.Equals(typeof(Entity<>)))
    //            {
    //                while (!tmpEntity.BaseType.Equals(typeof(Entity<>)))
    //                {
    //                    tmpEntity = entity.GetType().BaseType;
    //                }
    //            }
    //            return
    //                new DbTb
    //                {
    //                    TbName = tmpEntity.Name,
    //                    DbName = "Mongo_System"
    //                };
    //        }
    //    }
    //}
}
