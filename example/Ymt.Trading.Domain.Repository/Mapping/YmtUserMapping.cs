using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ymt.Trading.Domain.Model.User;
using YmtSystem.Repository.EF.ModelMapping;

namespace Ymt.Trading.Domain.Repository.Mapping
{
    public class YmtUserMappingConfigure : EntityTypeConfiguration<YmtUser>, IDBMappingRegister
    {
        public YmtUserMappingConfigure()
        {
            ToTable("ymt_trading_user_3")
            .HasKey(e => e.Id);
            Map<Buyers>(e => 
            {
                e.MapInheritedProperties();
                e.ToTable("ymt_trading_user_3");
            });
        }

        public void Register(ConfigurationRegistrar cfg)
        {
            cfg.Add(new YmtUserMappingConfigure());
        }
    }
}
