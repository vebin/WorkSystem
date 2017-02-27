
namespace YmtSystem.Repository.EF.ModelMapping
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration.Configuration;
    using System.Threading.Tasks;
    using System.Linq;
    using YmtSystem.CrossCutting;
    using YmtSystem.CrossCutting.Utility;
    /// <summary>
    /// 实体DB关系映射注册
    /// </summary>
    internal class EntityRegisteService
    {
        /// <summary>
        /// 注册所有实现IDBMappingRegister的实体映射
        /// </summary>
        /// <param name="cfg"></param>
        public static void Registe(ConfigurationRegistrar cfg)
        {
            BuildManagerWrapper
                .Current
                .PublicTypes
                .AsParallel()
                .Where(e =>
                    e != null
                    && !e.FullName.Contains("IDBMappingRegister")
                    && typeof(IDBMappingRegister).IsAssignableFrom(e))
                .Each(e => ((IDBMappingRegister)Activator.CreateInstance(e))
                .Register(cfg));
        }
    }
}
