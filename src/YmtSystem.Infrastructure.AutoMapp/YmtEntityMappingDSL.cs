namespace YmtSystem.Infrastructure.AutoMapperAdapter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using AutoMapper;
    /// <summary>
    /// 实体映射DSL
    /// </summary>
    public static class YmtEntityMappingDSL
    {
        public static TTarget MapTo<TSource, TTarget>(this TSource source)
            where TTarget : class, new()
            where TSource : class
        {
            Mapper.CreateMap<TSource, TTarget>();
            return Mapper.Map<TSource, TTarget>(source);
        }

        public static TTarget MapTo<TSource, TTarget>(this TSource source
            , Expression<Func<TTarget, object>> expTarget
            , Expression<Func<TSource, dynamic>> sourceMember
            , bool reset = true)
        {
            if (reset) Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<TSource, TTarget>().ForMember(expTarget, s => s.MapFrom(sourceMember));
            });
            return Mapper.Map<TSource, TTarget>(source);
        }

        public static IMappingExpression<TSource, TTarget> CreateMap<TSource, TTarget>(bool reset = false, bool disableConstructorMapping = false)
        {
            if (reset) Mapper.Reset();
            if (disableConstructorMapping) Mapper.Configuration.DisableConstructorMapping();
            return Mapper.CreateMap<TSource, TTarget>();
        }
        public static IMappingExpression<TSource, TTarget> TypeConvert<TSource, TTarget>(this IMappingExpression<TSource, TTarget> mapp
            , TypeConverter<TSource, TTarget> converter)
        {
            mapp.ConvertUsing(converter);
            return mapp;
        }
        public static IMappingExpression<TSource, TTarget> TypeConvert<TSource, TTarget>(this IMappingExpression<TSource, TTarget> mapp
          , Func<TSource, TTarget> converter)
        {
            mapp.ConvertUsing(converter);
            return mapp;
        }

        public static TTarget MapTo<TSource, TTarget>(this IMappingExpression<TSource, TTarget> mapp, TSource source)
        {
            return Mapper.Map<TSource, TTarget>(source);
        }
    }
}
