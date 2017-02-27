namespace YmtSystem.CrossCutting.AutomapperAdapter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AutoMapper;

    public static class YmtObjMapping
    {
        public static TTarget MapTo<TSource, TTarget>(this TSource source)
            where TTarget : class, new()
            where TSource : class
        {
            Mapper.CreateMap<TSource, TTarget>();
            return Mapper.Map<TSource, TTarget>(source);
        }
    }
}
