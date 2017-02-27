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
    //public class BuyersMappingConfigure : EntityTypeConfiguration<Buyers>, IDBMappingRegister
    //{
    //    public BuyersMappingConfigure()
    //    {
    //        //class table inheritance 类表映射：每个类对应一个表（不包含其父类），表示继承的层次 [.MapInheritedProperties()]
    //        //single table inheritance 单表继承：将类的继承层次，映射为一个表（父类，子类映射到一个表）[.MapInheritedProperties();]
    //        //conncrete table inheritance 具体表继承：每个类对应一个表（包含其父类）,表示继承的层次
    //        //ToTable("ymt_trading_user_3")
    //        HasKey(e => e.Id);
    //        Map<Buyers>(e =>
    //        {
    //            e.MapInheritedProperties();
    //            e.ToTable("ymt_trading_user_3");
    //        }) ;
    //    }
    //    public void Register(ConfigurationRegistrar cfg)
    //    {
    //        cfg.Add(this);
    //    }
    //}
}
