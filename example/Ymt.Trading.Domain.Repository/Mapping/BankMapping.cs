

namespace Ymt.Trading.Domain.Repository.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using System.Data.Entity.ModelConfiguration.Configuration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Ymt.Trading.Domain.Model.Account;
    using YmtSystem.Repository.EF.ModelMapping;

    public class BankMapping : EntityTypeConfiguration<Bank>, IDBMappingRegister
    {
        public BankMapping()
        {
            ToTable("Bank");
            HasKey(e => e.Id);
            Property(e => e.Address.City).HasColumnName("City").IsOptional();
            Property(e => e.Address.Street).HasColumnName("Street").IsOptional();
            Property(e => e.Address.Zip).HasColumnName("Zip").IsOptional();
            HasMany(e => e.UserAccount).WithOptional().Map(e => e.MapKey("Bank_Id"));
        }



        public void Register(ConfigurationRegistrar cfg)
        {
            cfg.Add(this);
        }
    }
}
