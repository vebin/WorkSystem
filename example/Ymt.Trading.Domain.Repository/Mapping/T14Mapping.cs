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
    public class T14Mapping : EntityTypeConfiguration<T14>, IDBMappingRegister
    {
        public T14Mapping()
        {
            ToTable("T_14");
            Ignore(e => e.Id);
            HasKey(e => e.TId);
            
            Property(e => e.TId).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
        }

        public void Register(ConfigurationRegistrar cfg)
        {
            cfg.Add(this);
        }
    }
}
