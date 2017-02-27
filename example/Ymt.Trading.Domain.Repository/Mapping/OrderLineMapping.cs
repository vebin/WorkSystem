namespace Ymt.Trading.Domain.Repository.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using System.Data.Entity.ModelConfiguration.Configuration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Ymt.Trading.Domain.Model.Order;
    using YmtSystem.Repository.EF.ModelMapping;
    public class OrderLineMappingConfigure : EntityTypeConfiguration<OrderLine>, IDBMappingRegister
    {
        public OrderLineMappingConfigure()
        {
            ToTable("ymt_order_line").HasKey(e => e.Id);
        }

        public void Register(ConfigurationRegistrar cfg)
        {
            cfg.Add(this);
        }
    }
}
