using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ymt.Trading.Domain.Model.Table;
using YmtSystem.Repository.EF.ModelMapping;

namespace Ymt.Trading.Domain.Repository.Mapping
{
    public class T11Mapping : EntityTypeConfiguration<T11>, IDBMappingRegister
    {
        public T11Mapping()
        {
            Map<T11>(e =>
            {
                e.ToTable("T_11");
            })
            .Map<T12>(e =>
            {
                e.MapInheritedProperties();
                e.ToTable("T_12");
            })
            .Map<T13>(e =>
            {
                e.MapInheritedProperties();
                e.ToTable("T_13");
            });
        }
        public void Register(ConfigurationRegistrar cfg)
        {
            cfg.Add(this);
        }
    }
}
