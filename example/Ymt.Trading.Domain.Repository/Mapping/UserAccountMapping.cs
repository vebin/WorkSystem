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

    public class UserAccountMapping : EntityTypeConfiguration<UserAccount>, IDBMappingRegister
    {
        public UserAccountMapping()
        {
            ToTable("UserAccount");
            HasKey(e => e.Id);

        }



        public void Register(ConfigurationRegistrar cfg)
        {
            cfg.Add(this);
        }
    }
}
