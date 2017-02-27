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
    public class T8Mapping : EntityTypeConfiguration<T8>, IDBMappingRegister
    {
        public T8Mapping()
        {
            Map<T8>(e =>
            {
                e.ToTable("T_8");
            })
            .Map<T9>(e =>
            {
                e.ToTable("T_9");
            })
            .Map<T10>(e =>
            {
                e.ToTable("T_10");
            });
        }

        public void Register(ConfigurationRegistrar cfg)
        {
            cfg.Add(this);
        }
    }
}
