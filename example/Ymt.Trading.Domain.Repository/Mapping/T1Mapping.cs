using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ymt.Trading.Domain.Model.Table;
using YmtSystem.Repository.EF.ModelMapping;

namespace Ymt.Trading.Domain.Repository.Mapping
{
    public class T1Mapping : EntityTypeConfiguration<T1>, IDBMappingRegister
    {
        public T1Mapping()
        {
            ToTable("T_1");
            //HasKey(e => e.T1Key);
            //Property(e => e.T1Key).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //Property(e => e.Version).IsConcurrencyToken(); //指定通过 Version 进行版本控制 [ConcurrencyCheck ]Version{get;set;}
            Property(e => e.Version).IsRowVersion(); //指定通过 Version 进行版本控制
            //忽略框架定义的主键
            Ignore(e => e.T1Key);
        }

        public void Register(ConfigurationRegistrar cfg)
        {
            cfg.Add(this);
        }
    }
}
