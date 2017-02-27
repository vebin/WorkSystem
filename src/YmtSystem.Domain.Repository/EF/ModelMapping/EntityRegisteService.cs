
namespace YmtSystem.Domain.Repository.EF.ModelMapping
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration.Configuration;
    using System.Threading.Tasks;
    using System.Linq;
    using YmtSystem.CrossCutting;
    using YmtSystem.CrossCutting.Utility;

    public class EntityRegisteService
    {
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
